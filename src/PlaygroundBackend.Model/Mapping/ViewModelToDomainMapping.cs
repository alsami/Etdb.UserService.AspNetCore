using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundBackend.Model.Entities;
using PlaygroundBackend.Model.ViewModel;

namespace PlaygroundBackend.Model.Mapping
{
    public class ViewModelToDomainMapping : Profile
    {
        public ViewModelToDomainMapping()
        {
            this.CreateMap<TestViewModel, Test>();
        }
    }
}
