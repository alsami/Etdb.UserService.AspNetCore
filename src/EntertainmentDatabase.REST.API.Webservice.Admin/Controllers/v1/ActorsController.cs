using System.Collections.Generic;
using AutoMapper;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using EntertainmentDatabase.REST.API.WebService.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.WebService.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class ActorsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<Actor> actorRepository;

        public ActorsController(IMapper mapper, IEntityRepository<Actor> actorRepository)
        {
            this.mapper = mapper;
            this.actorRepository = actorRepository;
        }

        [HttpGet]
        public IEnumerable<ActorDTO> Get()
        {
            return this.mapper.Map<IEnumerable<ActorDTO>>(this.actorRepository.GetAll());
        }
    }
}
