using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        /*
         GET /api/Todo HTTP/1.1
        */
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }


        /*
         GET /api/Todo/1 HTTP/1.1
         */
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        /*
            POST /api/todo HTTP/1.1
            Content-Type: application/json
	            {
        
                    "name": "Teste",
                    "isComplete": true
                }
         */
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
            //http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html
        }


        /*
        PUT /api/Todo/2 HTTP/1.1
        Content-Type: application/json
        {
           "name": "Teste - alt",
	        "isComplete": false
        }
         */
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            item.Id = id;
            var todo = _context.TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);

            //_context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();
            return new NoContentResult();
            //https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html
        }

        /*
         DELETE /api/Todo/2 HTTP/1.1
        */
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
            //http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html
        }


    }
}
