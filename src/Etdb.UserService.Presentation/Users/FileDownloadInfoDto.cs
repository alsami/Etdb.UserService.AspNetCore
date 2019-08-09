namespace Etdb.UserService.Presentation.Users
{
    public class FileDownloadInfoDto
    {
        public string MediaType { get; set; } = null!;

        public string Name { get; set; } = null!;

        public byte[] File { get; set; } = null!;

        public FileDownloadInfoDto(string mediaType, string name, byte[] file)
        {
            this.MediaType = mediaType;
            this.Name = name;
            this.File = file;
        }

        public FileDownloadInfoDto()
        {
        }
    }
}