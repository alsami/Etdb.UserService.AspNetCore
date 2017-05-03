using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Domain.Enums;
using EntertainmentDatabase.REST.API.Presentation.DataTransferObjects;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EntertainmentDatabase.REST.API.Admin.Controllers.v1
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

        public async Task<MovieFileDTO> Post(Guid movieId, IEnumerable<IFormFile> files)
        {
            MovieFile movieFile = null;
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
                this.movieFileRepository.EnsureChanges();
            }
            return this.mapper.Map<MovieFileDTO>(movieFile);
        }

        [HttpGet("{movieFileId:Guid}/download")]
        public IActionResult Download(Guid movieId, Guid movieFileId)
        {
            var movieFile = this.movieFileRepository.Get(movieFileId);
            var fileResult =
                new FileContentResult(movieFile.File, new MediaTypeHeaderValue("application/octet"))
                {
                    FileDownloadName = movieFile.Name
                };

            return fileResult;
        }
    }
}

