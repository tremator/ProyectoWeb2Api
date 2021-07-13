using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoWeb2.Models;
using System.Linq;
namespace ProyectoWeb2.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class UsersController: ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context){
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id){
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            user.role = await _context.Roles.FindAsync(user.roleId);
            return user;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusers(){
            return await _context.Users.ToArrayAsync();
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserInfo info){
            var results = from users in _context.Users select users;
            var user = await results.Where((user) => user.email == info.email && user.password == info.password)?.SingleAsync();
            return user;

        }

        [HttpPost]
        public async Task<ActionResult<User>> Register(User user){
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new{id = user.id}, user);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(long id, User user){
            if (id != user.id) {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id){
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}