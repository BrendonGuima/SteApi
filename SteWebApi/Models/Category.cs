using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SteWebApi.Model;

public class Category
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
      
    public string Name { get; set; }
    
    public List<Item> Items { get; set; } = new List<Item>();
}