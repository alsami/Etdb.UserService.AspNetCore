using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.API.ServiceBase.Generics.Base;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Entities
{
    public class Actor : IEntity
    {
        public Actor()
        {
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

        public string Name
        {
            get;
            set;
        }

        public string LastName
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
