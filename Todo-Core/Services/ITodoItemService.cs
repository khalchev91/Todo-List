using System;
using System.Threading.Tasks;
using Todo_Core.Models;

namespace Todo_Core.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync(ApplicationUser user);
        
        Task<bool> AddItemAsync(TodoItem item,ApplicationUser user);
        
        Task<bool> MarkDoneAsync(Guid id,ApplicationUser user);
    }
}