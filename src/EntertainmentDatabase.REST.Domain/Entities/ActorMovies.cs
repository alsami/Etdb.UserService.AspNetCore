using System;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;

namespace EntertainmentDatabase.REST.Domain.Entities
{
    public class ActorMovies : IEntity
    {
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

        public Guid ActorId
        {
            get;
            set;
        }

        public Guid MovieId
        {
            get;
            set;
        }

        public Actor Actor
        {
            get;
            set;
        }

        public Movie Movie
        {
            get;
            set;
        }
    }
}
