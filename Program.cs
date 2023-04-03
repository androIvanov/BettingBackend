using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;
using Users;
using XmlUtil;

var builder = WebApplication.CreateBuilder(args);

const string dataUrl =
    "https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7";

builder.Services.AddDbContext<XmlSportsContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
);

var app = builder.Build();

var timer = new System.Timers.Timer(6000);

Action fether = async () =>
{
    var options = new DbContextOptionsBuilder<XmlSportsContext>() // See if you can declare this at the top
        .UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
        .Options;
    var context = new XmlSportsContext(options);

    WebResponse data = WebRequest.Create(dataUrl).GetResponse();

    StreamReader read = new StreamReader(data.GetResponseStream());

    string responseText = read.ReadToEnd();

    XmlSerializer serializer = new XmlSerializer(typeof(XmlSports));

    using (StringReader reader = new StringReader(responseText))
    {
        XmlSports newData = (XmlSports)serializer.Deserialize(reader);

        XDocument doc = XDocument.Parse(responseText);

        XmlElementExtractor xmlExtractor = new XmlElementExtractor(doc);

        var sport = xmlExtractor.GetSports;

        var events = xmlExtractor.GetEvents;

        var match = xmlExtractor.GetMatches;

        var bets = xmlExtractor.GetBets;

        var odds = xmlExtractor.GetOdds;

        if (context.Sports.FirstOrDefault() == null)
        {
            context.Add(newData.Sport);
            context.SaveChanges();
        }
        else
        {
            var eventsInUrlButNotInDB = events
                .ToList()
                .ExceptBy(context.Events.ToList().Select(x => x.ID), x => x.ID);
            var matchesInUrlButNotInDB = match
                .ToList()
                .ExceptBy(context.Matches.ToList().Select(x => x.ID), x => x.ID);
            var betsInUrlButNotInDB = bets.ToList()
                .ExceptBy(context.Bets.ToList().Select(x => x.ID), x => x.ID);
            var oddsInUrlButNotInDB = odds.ToList()
                .ExceptBy(context.Odds.ToList().Select(x => x.ID), x => x.ID);

            var eventsInDbButNotInURL = context.Events
                .ToList()
                .ExceptBy(events.ToList().Select(x => x.ID), x => x.ID);
            var matchesInDbButNotInURL = context.Matches
                .ToList()
                .ExceptBy(match.ToList().Select(x => x.ID), x => x.ID);
            var betsInDbButNotInURL = context.Bets
                .ToList()
                .ExceptBy(bets.ToList().Select(x => x.ID), x => x.ID);
            var oddsInDbButNotInURL = context.Odds
                .ToList()
                .ExceptBy(odds.ToList().Select(x => x.ID), x => x.ID);

            //Update
            foreach (var item in eventsInUrlButNotInDB)
            {
                if (events.Where(x => x.ID == item.ID).First() != null)
                {
                    context.Events.Add(events.Where(x => x.ID == item.ID).First());
                }
            }
            foreach (var item in matchesInUrlButNotInDB)
            {
                if (match.Where(x => x.ID == item.ID).First() != null)
                {
                    context.Matches.Add(match.Where(x => x.ID == item.ID).First());
                }
            }
            foreach (var item in betsInUrlButNotInDB)
            {
                if (bets.Where(x => x.ID == item.ID).First() != null)
                {
                    context.Bets.Add(bets.Where(x => x.ID == item.ID).First());
                }
            }
            foreach (var item in oddsInUrlButNotInDB)
            {
                if (odds.Where(x => x.ID == item.ID).First() != null)
                {
                    context.Odds.Add(odds.Where(x => x.ID == item.ID).First());
                }
            }
            ;
            await context.SaveChangesAsync();

            Console.WriteLine("");
            Console.WriteLine("Events in URL, but not in DB " + eventsInUrlButNotInDB.Count());
            Console.WriteLine("Events in DB, but not in URL " + eventsInDbButNotInURL.Count());

            Console.WriteLine("");

            Console.WriteLine("Matches in URL, but not in DB " + matchesInUrlButNotInDB.Count());
            Console.WriteLine("Matches in DB, but not in URL " + matchesInDbButNotInURL.Count());

            Console.WriteLine("");

            Console.WriteLine("Bets in URL, but not in DB " + betsInUrlButNotInDB.Count());
            Console.WriteLine("Bets in DB, but not in URL " + betsInDbButNotInURL.Count());

            Console.WriteLine("");

            Console.WriteLine("Odds in URL, but not in DB " + oddsInUrlButNotInDB.Count());
            Console.WriteLine("Odds in DB, but not in URL " + oddsInDbButNotInURL.Count());

            Console.WriteLine("");
            Console.WriteLine("////////////////////////////////////");
            Console.WriteLine("");
        }
    }
};

timer.Elapsed += (sender, e) => fether();

timer.Start();

app.MapGet(
    "/",
    async () =>
    {
        var options = new DbContextOptionsBuilder<XmlSportsContext>()
            .UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
            .Options;
        var context = new XmlSportsContext(options);

        var queriedMatches = context.Matches
            .Where(m => m.StartDate >= DateTime.Now && (m.StartDate <= DateTime.Now.AddHours(24)))
            .Include(m => m.Bets)
            .ThenInclude(b => b.Odds)
            .ToList();

        return queriedMatches;
    }
);

app.MapGet(
    "/search",
    (string matchId) =>
    {
        var options = new DbContextOptionsBuilder<XmlSportsContext>()
            .UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
            .Options;
        var context = new XmlSportsContext(options);

        Console.WriteLine("LOL " + matchId);

        var matchFound = context.Matches.Where(m => m.ID == int.Parse(matchId)).ToList().First();

        return matchFound;
    }
);

app.Run();
