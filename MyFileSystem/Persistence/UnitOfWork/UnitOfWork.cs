using AutoMapper;
using MyFileSystem.Entities;
using MyFileSystem.Persistence.Repositories;
using System.Threading.Tasks;

namespace MyFileSystem.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    { 
        public  readonly FileSystemDbContext _context;
        public IBaseRepository<File> FileRepository { get; }
        public IBaseRepository<Folder> FoldersRepository { get; }
        public UnitOfWork(FileSystemDbContext context, IMapper mapper)
        {
            FileRepository = new BaseRepository<File>(context);
            FoldersRepository = new BaseRepository<Folder>(context);
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
