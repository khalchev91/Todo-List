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


        public async Task<TodoItem[]> GetIncompleteItemsAsync(ApplicationUser user)
        {
            return await _context.TodoItems.Where(x => x.IsDone == false && x.UserId == user.Id).ToArrayAsync();
        }

        public async Task<bool> AddItemAsync(TodoItem item,ApplicationUser user)
        {
            item.Id = Guid.NewGuid();
            item.IsDone = false;
            item.DueAt = DateTimeOffset.Now.AddDays(3);
            item.UserId = user.Id;
            _context.Add(item);

            var successful =await _context.SaveChangesAsync();
            return successful == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id,ApplicationUser user)
        {
            var item = await _context.TodoItems.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefaultAsync();
                
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