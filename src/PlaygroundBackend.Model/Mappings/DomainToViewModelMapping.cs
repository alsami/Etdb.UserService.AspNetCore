using AutoMapper;
using PlaygroundBackend.Model.Entities;
using PlaygroundBackend.Model.ViewModels;

namespace PlaygroundBackend.Model.Mappings
{
    public class DomainToViewModelMapping : Profile
    {
        public DomainToViewModelMapping()
        {
            this.CreateMap<TodoList, TodoListViewModel>();
            this.CreateMap<TodoItem, TodoItemViewModel>();
            this.CreateMap<TodoPriority, TodoPriorityViewModel>();
        }
    }
}
