using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RazorApp.Models
{
    
    public class Trans
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId InternalId { get; set; }
        public string TransactionId { get; set; }
        public string EmailPost { get; set; }
        public double Summ { get; set; }
        public string Phone { get; set; }
        public string Card { get; set; }
        public string Description { get; set; }
        public DateTime TransactionTime { get; set; }
        public bool IsCompleted { get; set; }
    }
}