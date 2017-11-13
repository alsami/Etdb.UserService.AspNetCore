using AutoMapper;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.WebService.Admin.Controllers.v1
{
    [Route("api/admin/v1/[controller]")]
    public class MovieActorsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<MovieActor> actorMovieRepository;

        public MovieActorsController(IMapper mapper, IEntityRepository<MovieActor> actorMovieRepository)
        {
            this.mapper = mapper;
            this.actorMovieRepository = actorMovieRepository;
        }
    }
}
