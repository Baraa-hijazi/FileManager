namespace MyFileSystem.Core.DTOs
{
    public class CreateFolderDto
    {
        public int? FolderParentId { get; set; }
        public string FolderName { get; set; }
        public string FolderPath { get; set; }
    } 
} 