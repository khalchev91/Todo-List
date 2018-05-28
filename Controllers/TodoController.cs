using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo_Core.Models;
using Todo_Core.Services;

namespace Todo_Core.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService service)
        {
            _todoItemService = service;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();
            var model = new TodoViewModel
            {
                TodoItems = items
            };
            return View(model);
        }
        
    }
}