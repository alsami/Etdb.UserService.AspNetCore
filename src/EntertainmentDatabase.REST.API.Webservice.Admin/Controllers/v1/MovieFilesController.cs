using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using EntertainmentDatabase.REST.API.WebService.Domain.Enums;
using EntertainmentDatabase.REST.API.WebService.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentDatabase.REST.API.WebService.Admin.Controllers.v1
{
    [Route("api/admin/v1/movies/{movieId:Guid}/[controller]")]
    public class MovieFilesController : Controller 
    {
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IEntityRepository<Movie> movieRepository;
        private readonly IEntityRepository<MovieFile> movieFileRepository;

        public MovieFilesController(IMapper mapper,
            IHttpContextAccessor contextAccessor, 
            IEntityRepository<Movie> movieRepository, 
            IEntityRepository<MovieFile> movieFileRepository)
        {
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
            this.movieRepository = movieRepository;
            this.movieFileRepository = movieFileRepository;
        }

        [HttpPost]
        public async Task<IEnumerable<MovieFileDTO>> Post(Guid movieId, IEnumerable<IFormFile> files)
        {
            MovieFile movieFile = null;
            List<MovieFile> generatedFiles = new List<MovieFile>();


            foreach (var file in files)
            {
                movieFile = new MovieFile
                {
                    MovieId = movieId,
                    MediaFileType = MediaFileType.Game,
                    Extension = new FileInfo(file.FileName).Extension,
                    Name = file.FileName,
                };

                using (var memoryStream = new MemoryStream(1024))
                {
                    await file.CopyToAsync(memoryStream);
                    movieFile.File = memoryStream.ToArray();
                }
                this.movieFileRepository.Add(movieFile);
                var saved = this.movieFileRepository.EnsureChanges();
                generatedFiles.Add(movieFile);
            }

            return this.mapper.Map<IEnumerable<MovieFileDTO>>(generatedFiles);
        }
    }
}

