using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Domain.Enums;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace EntertainmentDatabase.REST.API.Main.Controllers.v1
{
    [Route("api/main/v1/movies/{movieId:Guid}/[controller]")]
    public class MovieFilesController : Controller
    {
        private readonly IEntityRepository<MovieFile> movieFileRepository;

        public MovieFilesController(IEntityRepository<MovieFile> movieFileRepository)
        {
            this.movieFileRepository = movieFileRepository;
        }

        [HttpGet("{movieFileId:Guid}/download")]
        public IActionResult Download(Guid movieId, Guid movieFileId)
        {
            var movieFile = this.movieFileRepository.Get(movieFileId);
            return new FileContentResult(movieFile.File, new MediaTypeHeaderValue("application/octet"))
            {
                FileDownloadName = movieFile.Name
            };
        }
    }
}

