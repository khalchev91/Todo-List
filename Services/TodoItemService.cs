using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo_Core.Data;
using Todo_Core.Models;

namespace Todo_Core.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext _context)
        {
            this._context = _context;
        }


        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            return await _context.TodoItems.Where(x => x.IsDone == false).ToArrayAsync();
        }
    }
}