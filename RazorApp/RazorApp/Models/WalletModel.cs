using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RazorApp.Models
{
    [BsonIgnoreExtraElements]
    public class Wallet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId InternalId { get; set;}
        public string Id { get; set; }
        public string СardNumber { get; set; }
        public string Pass { get; set; }
        public string PhoneNumber { get; set; }
        public float Balance { get; set; } 
        public Status WalletStatus { get; set; }
        public enum Status
        {
            Active,
            Deleted,
            Returned
        }
    }
}