using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Etdb.UserService.AspNetCore.Extensions
{
    public static class FormFileExtensions
    {
        public static async ValueTask<ReadOnlyMemory<byte>> ReadFileBytesAsync(this IFormFile file)
        {
            var fileBytes = new Memory<byte>(new byte[file.Length]) ;

            await using (var fileStream = file.OpenReadStream())
            {
                await fileStream.ReadAsync(fileBytes);
            }

            return fileBytes;
        }
    }
}