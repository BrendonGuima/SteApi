using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SteWebApi.Model;
using System.Security.Claims;


namespace SteWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LendingManagerController : ControllerBase
{
    private readonly MongoDbContext _context;

    public LendingManagerController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpPost("Lend/{id}")]
    public async Task<ActionResult> LendForId(string id, [FromBody] LendingManagerDto request)
    {
        var item = await _context.Items
            .FindOneAndUpdateAsync(
                Builders<Item>.Filter.Eq(i => i.Id, id),
                Builders<Item>.Update.Set(i => i.IsLend, true)
            );

        if (item == null) return NotFound();
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

     
     //History
     var historyLending = new ItemTransactionHistory()
     {
         ItemId = item.Id,
         UserId = userId,
         CategoryId = item.CategoryId,
         ItemName = item.Name,
         ItemCode = item.Code,
         DateLend = DateTime.Now,
         DateReturn = null,
         IsLend = true,
         StudentName = request.StudentName,
         StudentId = request.StudentId
     };
     
     await _context.HistoryLendItems.InsertOneAsync(historyLending);

     return NoContent();
    }
    
    [HttpPost("Return/{id}")]
    public async Task<ActionResult> ReturnItem(string id)
    {
        var item = await _context.Items
            .FindOneAndUpdateAsync(
                Builders<Item>.Filter.Eq(i => i.Id, id),
                Builders<Item>.Update.Set(i => i.IsLend, false));

        if (item == null) NotFound();
        //History
        var filter = Builders<ItemTransactionHistory>.Filter.Eq(h => h.ItemId, id);
        var sort = Builders<ItemTransactionHistory> .Sort.Descending(h => h.DateLend);
        var latestHistory = await _context.HistoryLendItems.Find(filter).Sort(sort).FirstOrDefaultAsync();

        if (latestHistory == null)
        {
            return NotFound(); 
        }
        
        var update = Builders<ItemTransactionHistory>.Update.Set(h => h.DateReturn, DateTime.Now)
            .Set(h => h.IsLend, false);


        var updateResult = await _context.HistoryLendItems.UpdateOneAsync(
     Builders<ItemTransactionHistory>.Filter.Eq(h => h.Id, latestHistory.Id),
     update
 );

        if (updateResult.ModifiedCount == 0)
        {
            return NotFound(); 
        }
        return NoContent();
    }


    [HttpGet("History")]
    public async Task<ActionResult> GetHistory()
    {
        var items = await _context.HistoryLendItems.Find(_ => true).ToListAsync();
        return Ok(items);
    }
}