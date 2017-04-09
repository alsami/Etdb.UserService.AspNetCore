using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EntertainmentDatabase.REST.API.Controllers
{
    [Route("api/[controller]")]
    public class ActorMoviesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<ActorMovie> actorMovieRepository;

        public ActorMoviesController(IMapper mapper, IEntityRepository<ActorMovie> actorMovieRepository)
        {
            this.mapper = mapper;
            this.actorMovieRepository = actorMovieRepository;
        }
    }
}
