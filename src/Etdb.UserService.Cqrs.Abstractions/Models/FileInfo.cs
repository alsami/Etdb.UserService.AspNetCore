namespace Etdb.UserService.Cqrs.Abstractions.Models
{
    public class FileInfo
    {
        public string MediaType { get; }

        public string Name { get; }

        public byte[] File { get; }

        public FileInfo(string mediaType, string name, byte[] file)
        {
            this.MediaType = mediaType;
            this.Name = name;
            this.File = file;
        }
    }
}