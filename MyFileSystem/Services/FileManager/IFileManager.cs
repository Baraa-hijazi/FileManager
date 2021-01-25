using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Interfaces.File
{
    public interface IFileManager
    {
        string GetRootPath();
        void CreateDirectory(string path);
        void DeleteDirectory(string path);
        void UploadFile(IFormFile file, string path);
        void DeleteFile(string path);
    }
}
