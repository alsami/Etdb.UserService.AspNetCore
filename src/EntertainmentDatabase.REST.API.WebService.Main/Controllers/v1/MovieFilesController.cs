using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Entities;
using EntertainmentDatabase.REST.API.WebService.Presentation.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EntertainmentDatabase.REST.API.WebService.Main.Controllers.v1
{
    [Route("api/main/v1/movies/{movieId:Guid}/[controller]")]
    public class MovieFilesController : Controller
    {
        private readonly IEntityRepository<MovieFile> movieFileRepository;
        private readonly IMapper mapper;

        public MovieFilesController(IMapper mapper, IEntityRepository<MovieFile> movieFileRepository)
        {
            this.mapper = mapper;
            this.movieFileRepository = movieFileRepository;
        }

        [HttpGet]
        public IEnumerable<MovieFileDTO> Get(Guid movieId)
        {
            return this.mapper.Map<IEnumerable<MovieFile>, IEnumerable<MovieFileDTO>>(this.movieFileRepository
                .GetAll()
                .Where(movieFile => movieFile.MovieId == movieId));
        }

        [HttpGet("{movieFileId:Guid}/download")]
        public FileContentResult Download(Guid movieId, Guid movieFileId)
        {
            var movieFile = this.movieFileRepository.Get(movieFileId);
            return new FileContentResult(movieFile.File, new MediaTypeHeaderValue("application/octet"))
            {
                FileDownloadName = movieFile.Name
            };
        }
    }
}

