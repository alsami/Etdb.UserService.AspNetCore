using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.DataTransferObjects;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.Controllers
{
    [Route("api/[controller]")]
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
