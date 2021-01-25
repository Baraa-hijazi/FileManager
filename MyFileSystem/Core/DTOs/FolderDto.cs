using MyFileSystem.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = MyFileSystem.Entities.File;

namespace MyFileSystem.Core.DTOs
{
    public class FolderDto
    {
        public int FolderId { get; set; }
        public int? FolderParentId { get; set; }
        public string FolderName { get; set; }
        public ICollection<FileDto> Files { get; set; }
        public ICollection<FolderDto> Folders { get; set; }
    }

    public class FoldersDto
    {
        public int FolderId { get; set; }
        public int? FolderParentId { get; set; }
        public string FolderName { get; set; }
    }
}
