using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RazorApp.Models
{
    [BsonIgnoreExtraElements]
    public class Operation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId InternalId { get; set; }
        public DateTime OperationTime { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public double Summ { get; set; }
        public Type OperationType { get; set; }
        public enum Type
        {
            Replenishment,
            Debiting
        }
        public bool IsCompleted { get; set; }

    }
}