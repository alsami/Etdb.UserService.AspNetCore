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
        private readonly IDataRepository<TodoList> todoListDataRepository;
        private readonly IMapper mapper;

        public TodoListsController(IDataRepository<TodoList> todoListDataRepository, IMapper mapper)
        {
            this.todoListDataRepository = todoListDataRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TodoListViewModel> GetAll()
        {
            return
                this.mapper.Map<IEnumerable<TodoList>, IEnumerable<TodoListViewModel>>(this.todoListDataRepository
                    .GetAll()
                    .ToList());
        }

        [HttpGet("{id:int}")]
        public TodoListViewModel GetSingle(int id)
        {
            return
                this.mapper.Map<TodoList, TodoListViewModel>(this.todoListDataRepository.GetIncluding(id,
                    tdl => tdl.TodoItems));
        }

        [HttpPost]
        public TodoListViewModel Create([FromBody] TodoListViewModel todoListViewModel)
        {
            var todoList = this.mapper.Map<TodoListViewModel, TodoList>(todoListViewModel);
            this.todoListDataRepository.Add(todoList);
            this.todoListDataRepository.EnsureChanges();
            return this.mapper.Map<TodoList, TodoListViewModel>(todoList);
        }

        [HttpPut]
        public TodoListViewModel Update([FromBody] TodoListViewModel todoListViewModel)
        {
            var todoList = this.todoListDataRepository
                .GetIncluding(tdl => tdl.Id == todoListViewModel.Id, tdl => tdl.TodoItems);

            todoList.Designation = todoListViewModel.Designation;
            this.todoListDataRepository.Edit(todoList);
            this.todoListDataRepository.EnsureChanges();

            return this.mapper.Map<TodoList, TodoListViewModel>(todoList);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var todoList = this.todoListDataRepository.Get(tdl => tdl.Id == id);
            this.todoListDataRepository.Delete(todoList);
            this.todoListDataRepository.EnsureChanges();

            return Ok();
        }
    }
}
