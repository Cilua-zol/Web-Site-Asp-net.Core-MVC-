using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RazorApp.Models
{
    [BsonIgnoreExtraElements]
    public class Account
    {
        [BsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

    }
}