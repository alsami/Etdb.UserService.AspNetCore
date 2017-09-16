using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.API.Domain.Base;
using EntertainmentDatabase.REST.API.Domain.Enums;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.Domain.Entities
{
    public class Movie : IEntity, IConsumerMedia
    {
        public Movie()
        {
            this.MovieFiles = new List<MovieFile>();
            this.ActorMovies = new List<MovieActors>();
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

        public ICollection<MovieActors> ActorMovies
        {
            get;
            set;
        }
    }
}
