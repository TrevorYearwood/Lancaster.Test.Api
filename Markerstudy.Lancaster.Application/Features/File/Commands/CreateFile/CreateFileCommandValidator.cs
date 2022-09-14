using FluentValidation;

namespace Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile
{
    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        public CreateFileCommandValidator()
        {
            RuleFor(command => command).NotNull();

            RuleFor(command => command.Filename)
                .NotEmpty()
                .WithMessage("{PropertyName} is required and can not be blank.");

            //RuleFor(command => command.FileStream)
            //    .NotEmpty()
            //    .WithMessage("{PropertyName} is required and can not be blank.");
        }
    }
}
