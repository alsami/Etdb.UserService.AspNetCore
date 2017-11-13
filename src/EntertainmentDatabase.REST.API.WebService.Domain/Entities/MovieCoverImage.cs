using System;
using EntertainmentDatabase.REST.API.WebService.Domain.Base;
using EntertainmentDatabase.REST.API.WebService.Domain.Enums;

namespace EntertainmentDatabase.REST.API.WebService.Domain.Entities
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
