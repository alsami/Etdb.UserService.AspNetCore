using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.Controllers
{
    [Route("api/[controller]")]
    public class MovieActorsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<MovieActors> actorMovieRepository;

        public MovieActorsController(IMapper mapper, IEntityRepository<MovieActors> actorMovieRepository)
        {
            this.mapper = mapper;
            this.actorMovieRepository = actorMovieRepository;
        }
    }
}
