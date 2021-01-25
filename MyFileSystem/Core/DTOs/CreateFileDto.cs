using Microsoft.AspNetCore.Http;

namespace MyFileSystem.Core.DTOs
{
    public class CreateFileDto
    {
        public int FolderId { get; set; }
        public IFormFile PhFile { get; set; }
    }
}
