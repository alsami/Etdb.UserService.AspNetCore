using System;
using EntertainmentDatabase.REST.ServiceBase.DataStructure.Abstraction;

namespace EntertainmentDatabase.REST.API.Entities
{
    public class Movie : IEntity
    {
        public Guid Id { get; set; }

        public byte[] RowVersion
        {
            get;
            set;
        }

        public string Title { get; set; }
    }
}
