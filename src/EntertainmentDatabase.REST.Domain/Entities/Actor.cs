using System;
using System.Collections.Generic;
using EntertainmentDatabase.REST.ServiceBase.Generics.Abstractions;

namespace EntertainmentDatabase.REST.Domain.Entities
{
    public class Actor : IEntity
    {
        public Actor()
        {
            this.ActorMovies = new List<ActorMovies>();
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

        public ICollection<ActorMovies> ActorMovies
        {
            get;
            set;
        }
    }
}
