using AutoMapper;
using UltimateCoreWebAPI.Model.Entities;
using UltimateCoreWebAPI.Model.ViewModels;

namespace UltimateCoreWebAPI.Model.Mappings
{
    public class TodoListMapping : Profile
    {
        public TodoListMapping()
        {
            this.CreateMap<TodoList, TodoListViewModel>()
                .ReverseMap();
        }
    }
}
