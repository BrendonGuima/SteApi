using System.IdentityModel.Tokens.Jwt;
using Amazon.Runtime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SteWebApi.DtoModels;
using SteWebApi.Model;
using SteWebApi.Services;

namespace SteWebApi.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MongoDbContext _MongoDbContext;

        public UserController(MongoDbContext context)
        {
            _MongoDbContext = context;
        }
 
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate(AuthenticateDto model)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Name, model.Name);
            var userDb = await _MongoDbContext.Users.Find(filter).FirstOrDefaultAsync();
            if (userDb == null || userDb.Password != model.Password) return Unauthorized();
            JwtTokenGeneration jwtTokenGenerate = new JwtTokenGeneration();
            var jwt = jwtTokenGenerate.GenerateJwt(userDb);

            return Ok(new { access = jwt });
        }
        
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody]UserDto model)
        {

            var user = new User
            {
                Name = model.Name,
                Password = model.Password
            };
            
            await _MongoDbContext.Users.InsertOneAsync(user);
            return Ok(user);
        }
        
        [Authorize]
        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> UpdateItemName(string id, [FromBody] UserDto newUser)
        {
            var user = await _MongoDbContext.Users.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(newUser.Name))
            {
                user.Name = newUser.Name;
            }
            if (!string.IsNullOrWhiteSpace(newUser.Password))
            {
                user.Password = newUser.Password;
            }
            
            await _MongoDbContext.Users.ReplaceOneAsync(i => i.Id == id, user);

            return NoContent();
        }
        
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _MongoDbContext.Users.FindOneAndDeleteAsync(x => x.Id == id);
            if (user == null) return NotFound();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAll()
        {
            var model = await _MongoDbContext.Users.Find(_ => true).ToListAsync();
            return Ok(model);
        }
    }