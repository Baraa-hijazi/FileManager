using MyFileSystem.Entities;
using MyFileSystem.Persistence.Repositories;
using System.Threading.Tasks;

namespace MyFileSystem.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBaseRepository<File> FileRepository { get; }
        IBaseRepository<Folder> FoldersRepository { get; }
        Task CompleteAsync();
    }
}
