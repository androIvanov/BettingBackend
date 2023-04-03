namespace Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Serialization;

    [XmlRoot("XmlSports")]
    public class XmlSports
    {
        [XmlElement("Sport")]
        public Sport Sport { get; set; }
    }

    public class Sport
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [XmlElement(ElementName = "Event")]
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }

        [XmlAttribute(AttributeName = "CategoryID")]
        public int CategoryID { get; set; }

        [XmlElement(ElementName = "Match")]
        public List<Match> Matches { get; set; }
    }

    public class Match
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [XmlAttribute(AttributeName = "StartDate")]
        public DateTime StartDate { get; set; }

        [XmlAttribute(AttributeName = "MatchType")]
        public string MatchType { get; set; }

        [XmlElement(ElementName = "Bet")]
        public List<Bet> Bets { get; set; }
    }

    public class Bet
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [XmlAttribute(AttributeName = "IsLive")]
        public bool IsLive { get; set; }

        [XmlElement(ElementName = "Odd")]
        public List<Odd> Odds { get; set; }
    }

    public class Odd
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [XmlAttribute(AttributeName = "Value")]
        public decimal Value { get; set; }

        [XmlAttribute(AttributeName = "SpecialBetValue")]
        public decimal SpecialBetValue { get; set; }
    }
}
