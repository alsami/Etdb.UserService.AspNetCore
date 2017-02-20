using System;
using System.Collections;
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
    [Route("api/todolists/{todoListId:Guid}/[controller]")]
    public class TodosController : Controller
    {
        private readonly IEntityRepository<Todo> todosRepository;
        private readonly IEntityRepository<TodoList> todoListsRepository;
        private readonly IEntityRepository<TodoPriority> todoPrioritiesRepository;
        private readonly IMapper mapper;

        public TodosController(IEntityRepository<Todo> todosRepository, IEntityRepository<TodoList> todoListsRepository,
            IEntityRepository<TodoPriority> todoPrioritiesRepository, IMapper mapper)
        {
            this.todosRepository = todosRepository;
            this.todoListsRepository = todoListsRepository;
            this.todoPrioritiesRepository = todoPrioritiesRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TodoViewModel> Get(Guid todoListId, bool includeParent = false)
        {
            var todos = includeParent
            ? this.todosRepository
                .GetAllIncluding(todoItem => todoItem.TodoListId == todoListId, todoItem => todoItem.TodoPriority)
            : this.todosRepository.GetAllIncluding(todoItem => todoItem.TodoListId == todoListId, todoItem => todoItem.TodoPriority);

            return this.mapper.Map<IEnumerable<TodoViewModel>>(todos);
        }

        [HttpPost]
        public TodoViewModel Post(Guid todoListId, [FromBody] TodoViewModel todoViewModel)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("LOL");
            }

            var todo = this.mapper.Map<Todo>(todoViewModel);
            todo.TodoList = this.todoListsRepository.Get(todoListId);
            todo.TodoPriority =
                this.todoPrioritiesRepository.Get(todoPrio => todoPrio.Id == todoViewModel.TodoPriorityId);
            this.todosRepository.Add(todo);
            this.todosRepository.EnsureChanges();
            return this.mapper.Map<TodoViewModel>(todo);
        }

        [HttpPut("{id:guid}")]
        public TodoViewModel Put(Guid todoListId, Guid id, [FromBody] TodoViewModel todoViewModel)
        {
            //TODO check the id
            var todo = this.todosRepository.Get(id);
            this.mapper.Map(todoViewModel, todo);
            this.todosRepository.Edit(todo);
            this.todosRepository.EnsureChanges();
            return this.mapper.Map<TodoViewModel>(todo);
        }

    }
}
