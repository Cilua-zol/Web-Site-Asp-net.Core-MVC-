using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


[BsonIgnoreExtraElements]
public class SessionModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId InternalId { get; set;}
    public string Id { get; set; }
    public string Email { get; set; }
    public DateTime SessionTime { get; set; }
    public Status SessionStatus { get; set; }
    public enum Status{
        Active,
        Ended
    }
    
}