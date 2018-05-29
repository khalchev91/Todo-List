using System;
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

        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.AddItemAsync(item);
            if (!successful)
            {
                return BadRequest("Could not add item");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var successful = await _todoItemService.MarkDoneAsync(id);
            if (!successful)
            {
                return BadRequest("Could not mark as done");
            }

            return RedirectToAction("Index");
        }
        
    }
}