using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseMessages {
    public class DatabaseMessage {
        public string Type { get; set; }
        // Currently I'm only supporting insertions to the DB so this will always be set to true, but if I implement
        // record deleting this can be set to false to indicate the deletion
        public bool IsAdded { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EntitiyId { get; set; }
        public DateTime TimeOfRecord { get; set; }

        public DatabaseMessage(string type, bool isAdded, int EntitiyId, DateTime timeOfRecord){
            this.Type = type;
            this.IsAdded = isAdded;
            this.EntitiyId = EntitiyId;
            this.TimeOfRecord = timeOfRecord;
        }
    }
}