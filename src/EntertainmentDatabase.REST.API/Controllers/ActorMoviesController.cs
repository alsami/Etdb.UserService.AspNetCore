using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.Controllers
{
    [Route("api/[controller]")]
    public class ActorMoviesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<ActorMovies> actorMovieRepository;

        public ActorMoviesController(IMapper mapper, IEntityRepository<ActorMovies> actorMovieRepository)
        {
            this.mapper = mapper;
            this.actorMovieRepository = actorMovieRepository;
        }
    }
}
