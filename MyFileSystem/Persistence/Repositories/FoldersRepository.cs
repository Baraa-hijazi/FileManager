using AutoMapper;
using MyFileSystem.Entities;
using MyFileSystem.Persistence.Repositories;

namespace MyFileSystem.Persistence
{
    public class FoldersRepository : BaseRepository<Folder> , IFoldersRepository
    {
        private readonly FileSystemDbContext _context;
        private readonly IMapper _mapper;

        public FoldersRepository(FileSystemDbContext context ,IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
