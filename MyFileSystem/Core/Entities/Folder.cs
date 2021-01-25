using System.Collections.Generic;

namespace MyFileSystem.Entities
{

    public class Folder
    {
        public int FolderId { get; set; }
        public int? FolderParentId { get; set; }
        public Folder FolderParent { get; set; }
        public string FolderName { get; set; }
        public string FolderPath { get; set; }
        public ICollection<File> Files { get; set; }

        public Folder CheckParentId()
        {
            while (true)
            {
                if (FolderParentId == null) return null;
            }
        }
    }
}
