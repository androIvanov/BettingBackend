using Microsoft.EntityFrameworkCore;
using System.Net;
using BettingEntities;
using XmlUtil;
using DatabaseMessages;

var builder = WebApplication.CreateBuilder(args);

const string dataUrl =
    "https://sports.ultraplay.net/sportsxml?clientKey=9C5E796D-4D54-42FD-A535-D7E77906541A&sportId=2357&days=7";

builder.Services.AddDbContext<XmlSportsContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
);

var options = new DbContextOptionsBuilder<XmlSportsContext>() // See if you can declare this at the top
    .UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
    .Options;

var app = builder.Build();

const int updateInterval = 60000;

var timer = new System.Timers.Timer(updateInterval);

Action dataFetch = async () =>
{
    var context = new XmlSportsContext(options);

    WebResponse data = WebRequest.Create(dataUrl).GetResponse();

    StreamReader read = new StreamReader(data.GetResponseStream());

    string responseText = read.ReadToEnd();

    XmlElementExtractor xmlExtractor = new XmlElementExtractor(responseText);

    Sport[] sport = xmlExtractor.GetSports;

    Event[] events = xmlExtractor.GetEvents;

    Match[] match = xmlExtractor.GetMatches;

    Bet[] bets = xmlExtractor.GetBets;

    Odd[] odds = xmlExtractor.GetOdds;

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

    Action updateDatabase = () =>
    {
        foreach (var item in eventsInUrlButNotInDB)
        {
            if (events.Where(x => x.ID == item.ID).First() != null)
            {
                context.DatabaseMessages.Add(
                    new DatabaseMessage("Event", true, item.ID, DateTime.Now)
                );
                context.Events.Add(events.Where(x => x.ID == item.ID).First());
            }
        }
        foreach (var item in matchesInUrlButNotInDB)
        {
            if (match.Where(x => x.ID == item.ID).First() != null)
            {
                context.DatabaseMessages.Add(
                    new DatabaseMessage("Match", true, item.ID, DateTime.Now)
                );
                context.Matches.Add(match.Where(x => x.ID == item.ID).First());
            }
        }
        foreach (var item in betsInUrlButNotInDB)
        {
            if (bets.Where(x => x.ID == item.ID).First() != null)
            {
                context.DatabaseMessages.Add(
                    new DatabaseMessage("Bet", true, item.ID, DateTime.Now)
                );
                context.Bets.Add(bets.Where(x => x.ID == item.ID).First());
            }
        }
        foreach (var item in oddsInUrlButNotInDB)
        {
            if (odds.Where(x => x.ID == item.ID).First() != null)
            {
                context.DatabaseMessages.Add(
                    new DatabaseMessage("Odd", true, item.ID, DateTime.Now)
                );
                context.Odds.Add(odds.Where(x => x.ID == item.ID).First());
            }
        }
        ;
    };

    if (!context.Sports.ToList().Any())
    {
        //Seed the database if its empty
        context.Add(xmlExtractor.GetAllEntities.Sport);
    }
    else
    {
        updateDatabase();
    }
    await context.SaveChangesAsync();
};

timer.Elapsed += (sender, e) => dataFetch();

timer.Start();

app.MapGet(
    "/matches",
    () =>
    {
        var context = new XmlSportsContext(options);

        // Returns all matches that wll start in the next 24h
        var queriedMatches = context.Matches
            .Where(m => m.StartDate >= DateTime.Now && (m.StartDate <= DateTime.Now.AddHours(24)))
            .Include(m => m.Bets)
            .ThenInclude(b => b.Odds)
            .ToList();

        return queriedMatches;
    }
);

app.MapGet(
    "/match",
    (string matchId) =>
    {
        var context = new XmlSportsContext(options);

        var matchFound = context.Matches.Where(m => m.ID == int.Parse(matchId)).ToList().First();

        return matchFound;
    }
);

app.MapGet(
    "/databaseMessages",
    () =>
    {
        var context = new XmlSportsContext(options);

        return context.DatabaseMessages.ToList();
    }
);

app.Run();
