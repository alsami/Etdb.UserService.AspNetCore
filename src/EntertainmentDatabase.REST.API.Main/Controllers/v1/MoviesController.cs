using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Misc.Exceptions;
using EntertainmentDatabase.REST.API.Presentation.DataTransferObjects;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace EntertainmentDatabase.REST.API.Main.Controllers.v1
{
    [Authorize]
    [Route("api/main/v1/[controller]")]
    public class MoviesController
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
            var movies = this.movieRepository.GetAllIncluding(movie => movie.MovieCoverImage)
                .OrderBy(movie => movie.Title);
            return this.mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        [HttpGet("search/{searchTerm}")]
        public IEnumerable<MovieDTO> Search(string searchTerm)
        {
            var movies = this.movieRepository
                .GetAll()
                .Where(movie => movie.Title.ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(movie => movie.Title)
                .ThenBy(movie => movie.ReleasedOn);

            return this.mapper.Map<IEnumerable<MovieDTO>>(movies);
        }

        [HttpGet("{movieId:Guid}")]
        public MovieDTO Get(Guid movieId)
        {
            var movie = this.movieRepository
                .GetIncluding(movieId, m => m.MovieCoverImage, m => m.MovieFiles, m => m.ActorMovies);

            if (movie == null)
            {
                throw new RessourceNotFoundException($"The requested movie with id {movieId} could not be found!");
            }

            return this.mapper.Map<MovieDTO>(movie);
        }
    }
}
