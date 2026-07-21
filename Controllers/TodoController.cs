using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Todo.Models;

namespace Todo.Controllers
{
    [ApiController]
    [Route("Todo")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Todos>>> Items()
        {
            var l = await _context.Todos.ToListAsync();

            if (l.Count == 0)
            {
                return NotFound();
            }

            return l;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todos>> Item(int id)
        {
            try
            {
                var i = await _context.Todos.FindAsync(id);
                return Ok(i);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public async Task<ActionResult> PostItem(Todos item)
        {
            try
            {
                var t = new Todos { Title = item.Title,
                    Description = item.Description, CreatedDate = item.CreatedDate,
                    FinishDate = item.FinishDate
                };
                _context.Todos.Add(t);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);

            }
        }

    }
}
