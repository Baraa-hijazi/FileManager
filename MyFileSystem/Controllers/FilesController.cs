using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Services.Interfaces.File;
using System.Threading.Tasks;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace MyFileSystem.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {_fileService = fileService;}

        [HttpGet]
        public async Task<IActionResult> GetFiles() => Ok(await _fileService.GetFiles());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFiles(int id) => Ok(await _fileService.GetFiles(id));
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFiles(int id, [FromBody] UpdateFileDto updateFileDto) => Ok(await _fileService.UpdateFiles(id, updateFileDto));
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFiles(int id) => Ok(await _fileService.DeleteFiles(id));
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] CreateFileDto createFileDto) => Ok(await _fileService.UploadFile(createFileDto));
    }
}