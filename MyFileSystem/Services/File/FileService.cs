using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Persistence.UnitOfWork;
using MyFileSystem.Services.Interfaces.File;
using MyFileSystem.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFileSystem.Services.File
{
    public class FileService : IFileService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManager _fileManager;

        public FileService(IMapper mapper, IUnitOfWork unitOfWork, IFileManager fileManager)
        {
            _unitOfWork = unitOfWork;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<List<FileDto>> GetFiles()
        {
            var files = await _unitOfWork.FileRepository.GetAll();
            if (files == null)
                throw new Exception("Not Found... ");
            return _mapper.Map<List<Entities.File>, List<FileDto>>((List<Entities.File>)files);
        }
        public async Task<FileDto> GetFiles(int id)
        {
            var file = await _unitOfWork.FileRepository.GetById(id);
            if (file == null)
                throw new Exception("Not Found... ");
            return _mapper.Map<Entities.File, FileDto>(file);
        }
        public async Task<FileDto> UploadFile([FromForm] CreateFileDto createFileDto)
        {
            CreateFileValidator createfileValidator = new CreateFileValidator();
            if (!createfileValidator.Validate(createFileDto.PhFile).IsValid) throw new Exception("Name Not Valid... ");

            //--------------- Upload Physical File ------------------//
            var path = "";
            if (createFileDto.FolderId > 0)
            {
                var rootFolder = (await _unitOfWork.FoldersRepository.GetAllIncluded(f => f.FolderId == createFileDto.FolderId)).SingleOrDefault();
                if (rootFolder == null) { throw new Exception("Folder not found"); }
                path = rootFolder.FolderPath + '\\' + createFileDto.PhFile.FileName;
            }
            else
            {
                path = _fileManager.GetRootPath() + createFileDto.PhFile.FileName;
            }
            _fileManager.UploadFile(createFileDto.PhFile, path);

            //----------------------- Save to Db --------------------//
            var entityFile = new Entities.File
            {
                FileName = Path.GetFileNameWithoutExtension(path),
                FileExtension = Path.GetExtension(createFileDto.PhFile.FileName),
                FileSize = int.Parse(createFileDto.PhFile.Length.ToString()),
                FilePath = Path.GetFullPath(path),
                FolderId = createFileDto.FolderId > 0 ? createFileDto.FolderId : default(int?)
            };

            var f = new FileInfo(Path.GetFullPath(path));
            _unitOfWork.FileRepository.Add(entityFile);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<Entities.File, FileDto>(entityFile);
        }
        public async Task<string> UpdateFiles(int id, [FromBody] UpdateFileDto updateFileDto)
        {
            var file = await _unitOfWork.FileRepository.GetById(id);
            if (file == null)
                throw new Exception("Not Found... ");
            
            file = _mapper.Map(updateFileDto, file);
            await _unitOfWork.CompleteAsync();
            _mapper.Map<Entities.File, UpdateFileDto>(file);
            return ("File was updated... ");
        }
        public async Task<string> DeleteFiles(int id)
        {
            var file = await _unitOfWork.FileRepository.GetById(id);
            if (file == null)
                throw new Exception("Not Found... ");

            var path = file.FilePath;
            
            _fileManager.DeleteFile(path);
            
            _unitOfWork.FileRepository.Delete(file);
            await _unitOfWork.CompleteAsync();

            return ("File was deleted... ");
        }
    }
}