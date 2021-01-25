using Microsoft.AspNetCore.Http;
using MyFileSystem.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Interfaces.File
{
    public interface IFileService
    {
        Task<List<FileDto>> GetFiles();
        Task<FileDto> GetFiles(int id);
        Task<FileDto> UploadFile(CreateFileDto createFileDto/*IFormFile file, int folderId*/);
        Task<string> UpdateFiles(int id, UpdateFileDto createFileDto);
        Task<string> DeleteFiles(int id);
    }
}
