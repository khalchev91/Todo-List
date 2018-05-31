using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses.ResultOperators;
using Todo_Core.Data;
using Todo_Core.Models;
using Todo_Core.Services;
using Xunit;

namespace Todo_Core.UnitTests
{
    public class TodoItemServiceShould
    {
        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);

                var fakeUser = new ApplicationUser
                {
                    Id = "fake-000",
                    UserName = "fake@exmaple.com"
                };

                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing?"
                },fakeUser);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var itemsInDatabase = await context.TodoItems.CountAsync();
                Assert.Equal(1,itemsInDatabase);

                var item = await context.TodoItems.FirstAsync();
                Assert.Equal("Testing?",item.Title);
                Assert.Equal(false,item.IsDone);
                var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                Assert.True(difference<TimeSpan.FromSeconds(1));
            } 
        }

        
        [Fact]
        public async Task GetIncompleteTaskAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddGetItem").Options;

		    using(var context = new ApplicationDbContext(options))
            {
              var service = new TodoItemService(context);
                var fakeUser = new ApplicationUser
                {
                    Id = "fake-000",
                    UserName = "fake@exmaple.com"
                };

                await service.AddItemAsync(new TodoItem{Title = "Something to do"}, fakeUser);
            }
        
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);

                var fakeUser = new ApplicationUser
                {
                    Id = "fake-000",
                    UserName = "fake@exmaple.com"
                };        
                TodoItem[] items = await service.GetIncompleteItemsAsync(fakeUser);
          	Assert.True(items.Length>=1);      
            }
        }
        
        [Fact]
        public async Task MarkDoneAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddGetItem").Options;

            
            using (var context = new ApplicationDbContext(options))
            {
                var fakeUser = new ApplicationUser
                {
                    Id = "fake-000",
                    UserName = "fake@exmaple.com"
                };
                
                var item = await context.TodoItems.FirstAsync();
                var service = new TodoItemService(context);

                var result = await service.MarkDoneAsync(item.Id, fakeUser);
                Assert.True(result);
            }
        }
        
        [Fact]
        public async Task MarkDoneFailedAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddGetItem").Options;

            
            using (var context = new ApplicationDbContext(options))
            {
                var fakeUser = new ApplicationUser
                {
                    Id = "fake-001",
                    UserName = "fake@exmaple.com"
                };
                
                var item = await context.TodoItems.FirstAsync();
                var service = new TodoItemService(context);

                var result = await service.MarkDoneAsync(item.Id, fakeUser);
                Assert.Equal(false,result);
            }
        }
    }
}
