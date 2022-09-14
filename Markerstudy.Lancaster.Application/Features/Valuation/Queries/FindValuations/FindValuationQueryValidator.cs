using FluentValidation;

namespace Markerstudy.Lancaster.Application.Features.Valuation.Queries.FindValuations
{
    public class FindValuationQueryValidator : AbstractValidator<FindValuationsQuery>
    {
        public FindValuationQueryValidator()
        {
            RuleFor(command => command).NotNull();

            RuleFor(command => command.Filename)
                .NotEmpty()
                .WithMessage("{PropertyName} is required and cannot be blank.");
        }
    }
}