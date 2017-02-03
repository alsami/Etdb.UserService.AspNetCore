using AutoMapper;
using UltimateCoreWebAPI.Model.Entities;
using UltimateCoreWebAPI.Model.ViewModels;

namespace UltimateCoreWebAPI.Model.Mappings
{
    public class ViewModelToDomainMapping : Profile
    {
        public ViewModelToDomainMapping()
        {
            this.CreateMap<TodoListViewModel, TodoList>();
            this.CreateMap<TodoItemViewModel, TodoItem>();
            this.CreateMap<TodoPriorityViewModel, TodoPriority>();
        }
    }
}
