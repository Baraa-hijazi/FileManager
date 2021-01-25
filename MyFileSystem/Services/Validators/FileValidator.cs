using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MyFileSystem.Validators
{
    public class CreateFileValidator : AbstractValidator<IFormFile>
    {
        public CreateFileValidator()
        {
            RuleFor(f => f.FileName).NotEmpty().MinimumLength(1).MaximumLength(20).WithMessage("File Name Must be between 1 and 20 chars. ");
        }
    }
}
