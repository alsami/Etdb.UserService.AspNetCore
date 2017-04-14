using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.Domain.Abstractions;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;
using EntertainmentDatabase.REST.ServiceBase.Generics.Enums;

namespace EntertainmentDatabase.REST.Domain.Entities
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
