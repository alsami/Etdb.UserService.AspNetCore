using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundBackend.Model.Entities;
using PlaygroundBackend.Model.ViewModel;

namespace PlaygroundBackend.Model.Mapping
{
    public class DomainToViewModelMapping : Profile
    {
        public DomainToViewModelMapping()
        {
            this.CreateMap<Test, TestViewModel>();
        }
    }
}
