namespace Etdb.UserService.Cqrs.Abstractions.Models
{
    public class FileDownloadInfo
    {
        public FileDownloadInfo(string mediaType, string name, byte[] file)
        {
            this.MediaType = mediaType;
            this.Name = name;
            this.File = file;
        }

        public string MediaType { get; }

        public string Name { get; }

        public byte[] File { get; }
    }
}