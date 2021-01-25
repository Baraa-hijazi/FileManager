using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Services.Interfaces.Folder;
using System.Threading.Tasks;

namespace MyFileSystem.Controllers
{
    [Route("api/folders")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        { 
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolders([FromBody] CreateFolderDto cFolderDto) => Ok(await _folderService.CreateFolders(cFolderDto));
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolders(int id) => Ok(await _folderService.GetFolder(id));
        [HttpGet]
        public async Task<IActionResult> GetFolders() => Ok(await _folderService.GetFolders());
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFolders(int id, [FromForm] string path2) => Ok(await _folderService.UpdateFolders(id, path2));
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolders(int id) => Ok(await _folderService.DeleteFolders(id));
    } 
}