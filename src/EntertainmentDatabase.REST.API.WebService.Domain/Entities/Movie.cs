using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Enums;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Entities
{
    public class Movie : IEntity, IConsumerMedia
    {
        public Movie()
        {
            this.MovieFiles = new List<MovieFile>();
            this.ActorMovies = new List<MovieActor>();
        }

        public Guid Id
        {
            get;
            set;
        }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public ConsumerMediaType ConsumerMediaType
        {
            get;
            set;
        }

        public DateTime? ReleasedOn
        {
            get;
            set;
        }

        public MovieCoverImage MovieCoverImage
        {
            get;
            set;
        }

        public ICollection<MovieFile> MovieFiles
        {
            get;
            set;
        }

        public ICollection<MovieActor> ActorMovies
        {
            get;
            set;
        }
    }
}
