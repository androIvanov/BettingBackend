using System.Xml.Linq;
using Users;

namespace XmlUtil
{
    public class XmlElementExtractor
    {
        private XDocument Doc;

        public XmlElementExtractor(XDocument doc)
        {
            this.Doc = doc;
        }

        public Sport[] GetSports
        {
            get { return ExtractAllSportElementsFromXml(); }
        }
        public Event[] GetEvents
        {
            get { return ExtractAllEventElementsFromXml(); }
        }
        public Match[] GetMatches
        {
            get { return ExtractAllMatchElementsFromXml(); }
        }
        public Bet[] GetBets
        {
            get { return ExtractAllBetElementsFromXml(); }
        }
        public Odd[] GetOdds
        {
            get { return ExtractAllOddElementsFromXml(); }
        }

        private Sport[] ExtractAllSportElementsFromXml()
        {
            var result = Doc.Descendants("Sport")
                .Select(
                    b =>
                        new Sport
                        {
                            Name = b.Attribute("Name") != null ? (string)b.Attribute("Name") : "",
                            ID = b.Attribute("ID") != null ? (int)b.Attribute("ID") : 0,
                        }
                )
                .ToArray();

            return result;
        }

        private Event[] ExtractAllEventElementsFromXml()
        {
            var result = Doc.Descendants("Event")
                .Select(
                    b =>
                        new Event
                        {
                            Name = b.Attribute("Name") != null ? (string)b.Attribute("Name") : "",
                            ID = b.Attribute("ID") != null ? (int)b.Attribute("ID") : 0,
                            IsLive =
                                b.Attribute("IsLive") != null ? (bool)b.Attribute("IsLive") : false,
                            CategoryID =
                                b.Attribute("CategoryID") != null
                                    ? (int)b.Attribute("CategoryID")
                                    : 0,
                        }
                )
                .ToArray();

            return result;
        }

        private Match[] ExtractAllMatchElementsFromXml()
        {
            var result = Doc.Descendants("Match")
                .Select(
                    b =>
                        new Match
                        {
                            Name = b.Attribute("Name") != null ? (string)b.Attribute("Name") : "",
                            ID = b.Attribute("ID") != null ? (int)b.Attribute("ID") : 0,
                            StartDate =
                                b.Attribute("StartDate") != null
                                    ? (DateTime)b.Attribute("StartDate")
                                    : DateTime.Now,
                            MatchType =
                                b.Attribute("MatchType") != null
                                    ? (string)b.Attribute("MatchType")
                                    : "",
                        }
                )
                .ToArray();

            return result;
        }

        private Bet[] ExtractAllBetElementsFromXml()
        {
            var result = Doc.Descendants("Bet")
                .Select(
                    b =>
                        new Bet
                        {
                            Name = b.Attribute("Name") != null ? (string)b.Attribute("Name") : "",
                            ID = b.Attribute("ID") != null ? (int)b.Attribute("ID") : 0,
                            IsLive =
                                b.Attribute("IsLive") != null ? (bool)b.Attribute("IsLive") : false,
                        }
                )
                .ToArray();

            return result;
        }

        private Odd[] ExtractAllOddElementsFromXml()
        {
            var result = Doc.Descendants("Odd")
                .Select(
                    o =>
                        new Odd
                        {
                            Name = o.Attribute("Name") != null ? (string)o.Attribute("Name") : "",
                            ID = o.Attribute("ID") != null ? (int)o.Attribute("ID") : 0,
                            Value =
                                o.Attribute("Value") != null ? (decimal)o.Attribute("Value") : 0,
                            SpecialBetValue =
                                o.Attribute("SpecialBetValue") != null
                                    ? (decimal)o.Attribute("SpecialBetValue")
                                    : 0,
                        }
                )
                .ToArray();
            return result;
        }
    }
}
