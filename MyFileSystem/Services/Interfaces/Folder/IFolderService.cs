using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;

namespace MyFileSystem.Services.Interfaces.Folder
{
    public interface IFolderService
    {
        Task<FolderDto> CreateFolders(CreateFolderDto cFolderDto);
        Task<FolderDto> GetFolder(int id); 
        Task<List<FolderParentDto>> GetFolders();
        Task<string> DeleteFolders(int id);
        Task<string> UpdateFolders(int id, [FromForm] string path2);
    }
}
