using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Persistence.UnitOfWork;
using MyFileSystem.Services.Interfaces.File;
using MyFileSystem.Services.Interfaces.Folder;
using MyFileSystem.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFileSystem.Services.Folder
{
    public class FolderService : IFolderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManager  _fileManager;
 
        public FolderService(IMapper mapper, IUnitOfWork unitOfWork, IFileManager fileManager)
        {
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<FolderDto> CreateFolders(CreateFolderDto cFolderDto)
        {
            CreateFolderValidator createFolderValidator = new CreateFolderValidator();
            if (!createFolderValidator.Validate(cFolderDto).IsValid) throw new Exception("Not Valid... ");
            var path = _fileManager.GetRootPath() + cFolderDto.FolderName;
            if (cFolderDto.FolderParentId == null)
            {
                _fileManager.CreateDirectory(path);
                var folder = _mapper.Map<CreateFolderDto, Entities.Folder>(cFolderDto);
                folder.FolderPath = path;
                _unitOfWork.FoldersRepository.Add(folder);
                await _unitOfWork.CompleteAsync();
                var x = _mapper.Map<Entities.Folder, FolderDto>(folder);
                return x;
            }
            else 
            {
                var pFolder = await _unitOfWork.FoldersRepository.GetById(cFolderDto.FolderParentId);
                path = pFolder.FolderPath + "\\" + cFolderDto.FolderName; 
                _fileManager.CreateDirectory(path);
                var folder = _mapper.Map<CreateFolderDto, Entities.Folder>(cFolderDto);
                folder.FolderPath = path;
                _unitOfWork.FoldersRepository.Add(folder);
                await _unitOfWork.CompleteAsync();
                return _mapper.Map<Entities.Folder, FolderDto>(folder);
            }
        }
        public async Task<FolderDto> GetFolder(int id)
        {
            var folder = (await _unitOfWork.FoldersRepository
                .GetAllIncluded(f => f.FolderId == id, o => o.Files)).SingleOrDefault();
            var folders = await _unitOfWork.FoldersRepository.GetAll(f => f.FolderParentId == id);
            if (folder == null)
                throw new Exception("Not Found... ");
            var folderDto = _mapper.Map<Entities.Folder, FolderDto>(folder);
            folderDto.Folders = _mapper.Map<List<Entities.Folder>, List<FolderDto>>(folders.ToList());
            return folderDto;
        }
        public async Task<List<FolderParentDto>> GetFolders()
        {
            var folder = await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderId != null, o => o.Files);
            var folders = await _unitOfWork.FoldersRepository.GetAll();
            if (folder == null)
                throw new Exception("Not Found... ");
            return _mapper.Map<List<Entities.Folder>, List<FolderParentDto>>(folders.ToList());
        }
        public async Task<string> UpdateFolders(int id, [FromForm] string path2)
        {
            var folder = (await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderId == id, o => o.Files)).SingleOrDefault();
            if (folder == null)
                throw new Exception("Not Found... ");
            try
            {
                if (!Directory.Exists(path2))
                {
                    Directory.Move(folder.FolderPath, folder.FolderPath + "\\" + path2);
                    await _unitOfWork.CompleteAsync();
                    _mapper.Map<Entities.Folder, CreateFolderDto>(folder);
                    return ("Directory was Moved... ");
                }
                else throw new Exception("Not Found... "); ;
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }
        public async Task<string> DeleteFolders(int id)
        {
            var folder = await _unitOfWork.FoldersRepository.GetById(id);
            if (folder == null)
                throw new Exception("Not Found... ");
            await DeleteTree(id);
            _unitOfWork.FoldersRepository.Delete(folder);
            var path = folder.FolderPath;
            _fileManager.DeleteDirectory(path);
            await _unitOfWork.CompleteAsync();
            return ("Folder and it's contents were deleted... ");
        }
        public async Task DeleteTree(int fId)
        {
            var folders = (await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderParentId == fId)).ToList();
            var firstLevelFiles = (await _unitOfWork.FileRepository.GetAllIncluded(fi => fi.FolderId == fId)).ToList();
            if (firstLevelFiles.Count > 0)
            {  await _unitOfWork.FileRepository.DeleteRange(firstLevelFiles); }

            foreach (var folder in folders)
            {
                await DeleteTree(folder.FolderId);
                var files = (await _unitOfWork.FileRepository.GetAllIncluded(fi => fi.FolderId == fId)).ToList();
                if (files.Count > 0)
                {
                 await  _unitOfWork.FileRepository.DeleteRange(files);
                }
               _unitOfWork.FoldersRepository.Delete(folder);
            }
        }
    }
}
