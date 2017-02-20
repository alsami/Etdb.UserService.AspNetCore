using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UltimateCoreWebAPI.Infrastructure.Abstractions;
using UltimateCoreWebAPI.Model.Entities;
using UltimateCoreWebAPI.Model.ViewModels;

namespace UltimateCoreWebAPI.API.Controllers
{
    [Route("api/[controller]")]
    public class TodoListsController : Controller
    {
        private readonly IEntityRepository<TodoList> todoListEntityRepository;
        private readonly IMapper mapper;

        public TodoListsController(IEntityRepository<TodoList> todoListEntityRepository, IMapper mapper)
        {
            this.todoListEntityRepository = todoListEntityRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TodoListViewModel> GetAll()
        {
            return
                this.mapper.Map<IEnumerable<TodoList>, IEnumerable<TodoListViewModel>>(this.todoListEntityRepository
                    .GetAll()
                    .ToList());
        }

        [HttpGet("{id:int}")]
        public TodoListViewModel GetSingle(Guid id)
        {
            return
                this.mapper.Map<TodoList, TodoListViewModel>(this.todoListEntityRepository.GetIncluding(id,
                    tdl => tdl.TodoItems));
        }

        [HttpPost]
        public TodoListViewModel Create([FromBody] TodoListViewModel todoListViewModel)
        {
            var todoList = this.mapper.Map<TodoListViewModel, TodoList>(todoListViewModel);
            this.todoListEntityRepository.Add(todoList);
            this.todoListEntityRepository.EnsureChanges();
            return this.mapper.Map<TodoList, TodoListViewModel>(todoList);
        }

        [HttpPut]
        public TodoListViewModel Update([FromBody] TodoListViewModel todoListViewModel)
        {
            var todoList = this.todoListEntityRepository
                .GetIncluding(tdl => tdl.Id == todoListViewModel.Id, tdl => tdl.TodoItems);

            this.mapper.Map(todoListViewModel, todoList);
            this.todoListEntityRepository.Edit(todoList);
            this.todoListEntityRepository.EnsureChanges();

            return this.mapper.Map<TodoList, TodoListViewModel>(todoList);
        }

        [HttpDelete("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var todoList = this.todoListEntityRepository.Get(tdl => tdl.Id == id);
            this.todoListEntityRepository.Delete(todoList);
            this.todoListEntityRepository.EnsureChanges();

            return Ok();
        }
    }
}
