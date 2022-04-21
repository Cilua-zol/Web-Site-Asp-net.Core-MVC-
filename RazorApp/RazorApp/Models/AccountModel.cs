using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RazorApp.Models
{
    [BsonIgnoreExtraElements]
    
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId InternalId { get; set;}
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}   