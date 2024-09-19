using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SteWebApi.Model;

public class ItemTransactionHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [Required]
    public string ItemId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Code { get; set; }
    [Required] 
    public string CategoryId { get; set; }
    [Required]
    public DateTime DateLend { get; set; }
    [Required]
    public DateTime? DateReturn { get; set; }
    [Required]
    public string StudentName { get; set; }
    [Required]
    public string StudentId { get; set; }
}