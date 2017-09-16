using EntertainmentDatabase.REST.API.Domain.Entities;
using EntertainmentDatabase.REST.API.Domain.Enums;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntertainmentDatabase.REST.API.Admin.Controllers.v1
{
    [Route("api/admin/v1/movies/{movieId:Guid}/[controller]")]
    public class MovieCoverImagesController : Controller
    {
        private readonly IEntityRepository<MovieCoverImage> movieCoverImageRepo;

        public MovieCoverImagesController(IEntityRepository<MovieCoverImage> movieCoverImageRepo)
        {
            this.movieCoverImageRepo = movieCoverImageRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Guid movieId, IFormFile file)
        {
            var existingCoverImage = this.movieCoverImageRepo.GetAll()
                .FirstOrDefault(coverImage => coverImage.MovieId == movieId);

            if(existingCoverImage != null)
            {
                this.movieCoverImageRepo.Delete(existingCoverImage);
                this.movieCoverImageRepo.EnsureChanges();
            }

            var movieCoverImage = new MovieCoverImage
            {
                MovieId = movieId,
                MediaFileType = MediaFileType.Image,
                Extension = new FileInfo(file.FileName).Extension,
                Name = file.FileName,
            };

            using (var memoryStream = new MemoryStream(1024))
            {
                await file.CopyToAsync(memoryStream);
                movieCoverImage.File = memoryStream.ToArray();
            }
            this.movieCoverImageRepo.Add(movieCoverImage);
            var saved = this.movieCoverImageRepo.EnsureChanges();

            return Ok();
        }
    }
}
