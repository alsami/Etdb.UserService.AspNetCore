namespace Etdb.UserService.Presentation.Users
{
    public class FileDownloadInfoDto
    {
        public string MediaType { get; }

        public string Name { get; }

        public byte[] File { get; }

        public FileDownloadInfoDto(string mediaType, string name, byte[] file)
        {
            this.MediaType = mediaType;
            this.Name = name;
            this.File = file;
        }
    }
}