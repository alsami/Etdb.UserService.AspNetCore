using AutoMapper;
using UltimateCoreWebAPI.Model.Entities;
using UltimateCoreWebAPI.Model.ViewModels;

namespace UltimateCoreWebAPI.Model.Mappings
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
