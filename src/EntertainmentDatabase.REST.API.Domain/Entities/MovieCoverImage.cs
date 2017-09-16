using EntertainmentDatabase.REST.API.Domain.Base;
using EntertainmentDatabase.REST.API.Domain.Enums;
using EntertainmentDatabase.REST.ServiceBase.Generics.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntertainmentDatabase.REST.API.Domain.Entities
{
    public class MovieCoverImage : IMediaFile
    {
        public Guid MovieId
        {
            get;
            set;
        }

        public Movie Movie
        {
            get;
            set;
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

        public string Extension
        {
            get;
            set;
        }

        public MediaFileType MediaFileType
        {
            get;
            set;
        }

        public byte[] File
        {
            get;
            set;
        }
    }
}
