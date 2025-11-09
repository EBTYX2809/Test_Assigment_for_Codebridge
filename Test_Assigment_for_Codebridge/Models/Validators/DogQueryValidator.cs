using FluentValidation;
using Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;

namespace Test_Assigment_for_Codebridge.Models.Validators;

public class DogQueryValidator : AbstractValidator<DogQueryDTO>
{
    public DogQueryValidator()
    {
        RuleFor(d => d.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(d => d.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");

        RuleFor(d => d.SortBy)
            .Must(sortBy => new[] { "Id", "Name", "Color", "TailLength", "Weight" }
                .Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of the following: Id, Name, Color, TailLength, Weight.");

        RuleFor(d => d.OrderBy)
            .Must(orderBy => new[] { "Ascending", "Descending" }
                .Contains(orderBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage("OrderBy must be either 'Ascending' or 'Descending'.");
    }
}
