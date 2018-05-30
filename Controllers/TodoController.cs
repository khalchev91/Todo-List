using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todo_Core.Models;
using Todo_Core.Services;

namespace Todo_Core.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        
        public TodoController(ITodoItemService service,UserManager<ApplicationUser> userManager)
        {
            _todoItemService = service;
            _userManager = userManager;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var successful = await _todoItemService.AddItemAsync(item,currentUser);
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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var successful = await _todoItemService.MarkDoneAsync(id,currentUser);
            if (!successful)
            {
                return BadRequest("Could not mark as done");
            }

            return RedirectToAction("Index");
        }
        
    }
}