namespace MyFileSystem.Core.DTOs
{
    public class FileDto 
    {
        public int FileId { get; set; }
        public int FolderId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; } 
        public int FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
