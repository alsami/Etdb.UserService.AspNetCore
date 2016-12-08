using AutoMapper;
using PlaygroundBackend.Model.Entities;
using PlaygroundBackend.Model.ViewModels;

namespace PlaygroundBackend.Model.Mappings
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
