using SteWebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SteWebApi.DtoModels;

namespace SteWebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]

public class ItemController : ControllerBase
{
    private readonly MongoDbContext _MongoDbContext;
    
    public ItemController(MongoDbContext context)
    {
        _MongoDbContext = context;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> CreateItemAndLinkToCategory([FromBody]ItemDto model)
    {
        var existingCode = await _MongoDbContext.Items
            .Find(i => i.Code == model.Code && i.CategoryId == model.CategoryId)
            .FirstOrDefaultAsync();
        
        if (existingCode != null)
        {
            return BadRequest(new { message = "Já existe um item com esse Code na categoria especificada." });
        }

        var item = new Item
        {
            Name = model.Name,
            Code = model.Code,
            CategoryId = model.CategoryId,
            IsLend = false,
        };
        await _MongoDbContext.Items.InsertOneAsync(item);

        var categoryFilter = Builders<Category>.Filter.Eq(c => c.Id, item.CategoryId);
        await _MongoDbContext.Category.FindOneAndUpdateAsync(
            categoryFilter,
            Builders<Category>.Update.Push(c => c.Items, item),
            new FindOneAndUpdateOptions<Category> { ReturnDocument = ReturnDocument.After }
        );

        return Ok(item);
    }
    
    [HttpPut("Edit/{id}")]
    public async Task<ActionResult> UpdateItemName(string id, [FromBody] ItemDto newItem)
    {
        var existingCode = await _MongoDbContext.Items
            .Find(i => i.Code == newItem.Code && i.CategoryId == newItem.CategoryId)
            .FirstOrDefaultAsync();
        
        if (existingCode != null)
        {
            return BadRequest(new { message = "Já existe um item com esse Code na categoria especificada." });
        }
        var item = await _MongoDbContext.Items.Find(i => i.Id == id).FirstOrDefaultAsync();
        if (item == null) return NotFound();

        item.Name = newItem.Name;
        item.Code = newItem.Code;
        item.CategoryId = newItem.CategoryId;
        await _MongoDbContext.Items.ReplaceOneAsync(i => i.Id == id, item);

        return NoContent();
    }
    
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var item = await _MongoDbContext.Items.FindOneAndDeleteAsync(x => x.Id == id);
        if (item == null) return NotFound();

        var category = await _MongoDbContext.Category
            .Find(c => c.Items.Any(i => i.Id == id))
            .FirstOrDefaultAsync();

    
        if (category == null) return NotFound();

        var updateDefinition = Builders<Category>.Update.PullFilter(c => c.Items, i => i.Id == id);
        var updatedCategory = await _MongoDbContext.Category.FindOneAndUpdateAsync(
            Builders<Category>.Filter.Where(c => c.Id == category.Id),
            updateDefinition,
            new FindOneAndUpdateOptions<Category> { ReturnDocument = ReturnDocument.After });
        

        return Ok();
    }


    [HttpGet("Get/{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        var items = await _MongoDbContext.Items.Find(i => i.Id == id).FirstOrDefaultAsync();
        if (items == null)return NotFound();
        return Ok(items);
    }
    
    [HttpGet("GetAll")]
    public async Task<ActionResult> GetAll()
    {
        var model = await _MongoDbContext.Items.Find(_ => true).ToListAsync();
        return Ok(model);
    }
}