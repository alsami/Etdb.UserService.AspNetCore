using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UltimateCoreWebAPI.Model.Entities;
using UltimateCoreWebAPI.Model.ViewModels;

namespace UltimateCoreWebAPI.Model.Mappings
{
    public class TodoPriorityMapping : Profile
    {
        public TodoPriorityMapping()
        {
            this.CreateMap<TodoPriority, TodoPriorityViewModel>()
                .ReverseMap();
        }
    }
}
