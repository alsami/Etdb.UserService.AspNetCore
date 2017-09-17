using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Main.Controllers.v1
{
    [Route("api/main/v1/movies/{movieId:Guid}/[controller]")]
    public class MovieCoverImagesController : Controller
    {
        private readonly IEntityRepository<MovieCoverImage> movieCoverImageRepo;

        public MovieCoverImagesController(IEntityRepository<MovieCoverImage> movieCoverImageRepo)
        {
            this.movieCoverImageRepo = movieCoverImageRepo;
        }

        [HttpGet("download/{movieCoverImageId:Guid}")]
        public FileContentResult Download(Guid movieId, Guid movieCoverImageId)
        {
            var movieCoverImage = this.movieCoverImageRepo.Get(movieCoverImageId);
            return new FileContentResult(movieCoverImage.File, new MediaTypeHeaderValue("application/octet"))
            {
                FileDownloadName = movieCoverImage.Name,
            };
        }
    }
}
