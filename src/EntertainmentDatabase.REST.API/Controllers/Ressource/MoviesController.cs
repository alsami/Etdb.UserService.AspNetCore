using System.Collections.Generic;
using AutoMapper;
using EntertainmentDatabase.REST.API.DataTransferObjects;
using EntertainmentDatabase.REST.API.Entities;
using EntertainmentDatabase.REST.API.Entities.ConsumerMedia;
using EntertainmentDatabase.REST.ServiceBase.DataAccess.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.Controllers.Ressource
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<Movie> movieRepository;

        public MoviesController(IMapper mapper, IEntityRepository<Movie> movieRepository)
        {
            this.mapper = mapper;
            this.movieRepository = movieRepository;
        }

        [HttpGet]
        public IEnumerable<MovieDTO> Get()
        {
            return this.mapper.Map<IEnumerable<Movie>, IEnumerable<MovieDTO>>(this.movieRepository.GetAll());
        }
    }
}
