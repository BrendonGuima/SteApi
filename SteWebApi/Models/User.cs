using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SteWebApi.Model;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    [BsonRequired]
    public string? Name { get; set; }
    [BsonRequired]
    public string? Password { get; set; }
}