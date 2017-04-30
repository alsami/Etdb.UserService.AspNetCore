using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Admin.DataTransferObjects;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.Admin.Controllers.v1
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
        public IEnumerable<MovieDTO> GetAll()
        {
            return this.mapper.Map<IEnumerable<MovieDTO>>(this.movieRepository.GetAll());
        }

        [HttpPost]
        public MovieDTO Post([FromBody] MovieDTO movieDTO)
        {
            var movie = this.mapper.Map<Movie>(movieDTO);
            this.movieRepository.Add(movie);

            return this.mapper.Map<MovieDTO>(movie);
        }

        [HttpPut("{id:Guid}")]
        public async Task<MovieDTO> Put(Guid id, [FromBody] MovieDTO movieDTO)
        {
            var movie = this.movieRepository.Get(id);
            this.mapper.Map(movieDTO, movie);
            await this.movieRepository.EnsureChangesAsync();

            return this.mapper.Map<MovieDTO>(movie);
        }

        [HttpDelete("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            var movie = this.movieRepository.Get(id);
            this.movieRepository.Delete(movie);

            return Ok();
        }
    }
}
