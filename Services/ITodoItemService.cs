using System;
using System.Threading.Tasks;
using Todo_Core.Models;

namespace Todo_Core.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync();
        
        Task<bool> AddItemAsync(TodoItem item);
        
        Task<bool> MarkDoneAsync(Guid id);
    }
}