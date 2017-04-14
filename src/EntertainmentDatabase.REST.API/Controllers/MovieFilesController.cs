using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EntertainmentDatabase.REST.API.DataTransferObjects;
using EntertainmentDatabase.REST.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using EntertainmentDatabase.REST.ServiceBase.Generics.Enums;

namespace EntertainmentDatabase.REST.API.Controllers
{
    [Route("api/movies/{movieId:Guid}/[controller]")]
    public class MovieFilesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEntityRepository<Movie> movieRepository;
        private readonly IEntityRepository<MovieFile> movieFileRepository;

        public MovieFilesController(IMapper mapper, 
            IEntityRepository<Movie> movieRepository, 
            IEntityRepository<MovieFile> movieFileRepository)
        {
            this.mapper = mapper;
            this.movieRepository = movieRepository;
            this.movieFileRepository = movieFileRepository;
        }

        public async Task<MovieFileDTO> Post(Guid movieId, IEnumerable<IFormFile> files)
        {
            MovieFile movieFile = null;
            foreach (var file in files)
            {
                movieFile = new MovieFile
                {
                    MovieId = movieId,
                    MediaType = MediaType.Game,
                    Extension = "jpg",
                    IsCover = false,
                    Name = file.FileName,
                };

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    movieFile.File = memoryStream.ToArray();
                    this.movieFileRepository.Add(movieFile);
                    this.movieFileRepository.EnsureChanges();
                }
            }
            return this.mapper.Map<MovieFileDTO>(movieFile);
        }
    }
}
