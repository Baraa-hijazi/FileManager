using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MyFileSystem.Core.DTOs;
using MyFileSystem.Entities;

namespace MyFileSystem.Validators
{
    public class CreateFolderValidator : AbstractValidator<CreateFolderDto>
    {
        public CreateFolderValidator()
        {
            RuleFor(f => f.FolderName).NotEmpty().MinimumLength(1).MaximumLength(20).WithMessage("Folder Name Must be filled. ");
        }
    }
}
