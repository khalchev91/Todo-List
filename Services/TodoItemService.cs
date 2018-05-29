using System;
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

        public async Task<bool> AddItemAsync(TodoItem item)
        {
            item.Id = Guid.NewGuid();
            item.IsDone = false;
            item.DueAt = DateTimeOffset.Now.AddDays(3);
            _context.Add(item);

            var successful =await _context.SaveChangesAsync();
            return successful == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id)
        {
            var item = await _context.TodoItems.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (item == null)
            {
                return false;
            }

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}