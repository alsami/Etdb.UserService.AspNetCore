using System;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Entities
{
    public class MovieActors : IEntity
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
