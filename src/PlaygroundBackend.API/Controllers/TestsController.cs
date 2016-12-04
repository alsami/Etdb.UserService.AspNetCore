using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlaygroundBackend.Infrastructure.Abstractions;
using PlaygroundBackend.Model.Entities;
using PlaygroundBackend.Model.ViewModel;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PlaygroundBackend.API.Controllers
{
    [Route("api/[controller]")]
    public class TestsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IDataRepository<Test> testsRepo;

        public TestsController(IDataRepository<Test> testsRepo, IMapper mapper )
        {
            this.testsRepo = testsRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<TestViewModel> GetAll()
        {
            return this.mapper.Map<IEnumerable<Test>, IEnumerable<TestViewModel>>(this.testsRepo.GetAll());
        }

        [HttpPost]
        public TestViewModel Create([FromBody] TestViewModel testViewModel)
        {
            var test = this.mapper.Map<TestViewModel, Test>(testViewModel);
            this.testsRepo.Add(test);
            this.testsRepo.EnsureChanges();

            return this.mapper.Map<TestViewModel>(test);
        }
    }
}
