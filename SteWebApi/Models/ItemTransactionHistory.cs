using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SteWebApi.Model;

public class ItemTransactionHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? UserId { get; set; }
   
    public string? ItemId { get; set; }
   
    public string? ItemName { get; set; }
    
    public string? ItemCode { get; set; }
   
    public string? CategoryId { get; set; }

    public bool? IsLend { get; set; }

    public DateTime? DateLend { get; set; }
    
    public DateTime? DateReturn { get; set; }
    
    public string? StudentName { get; set; }
   
    public string? StudentId { get; set; }
}