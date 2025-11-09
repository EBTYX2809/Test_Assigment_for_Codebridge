using FluentValidation;

namespace Test_Assigment_for_Codebridge.Models.Validators;

public class DogValidator : AbstractValidator<Dog>
{    
    public DogValidator()
    {      
        RuleFor(d => d.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty.")
            .MaximumLength(20)
            .WithMessage("Name cannot be longer than 20 characters.");            

        RuleFor(d => d.Color)
            .NotEmpty()
            .WithMessage("Color cannot be empty.")
            .MaximumLength(20)
            .WithMessage("Color cannot be longer than 20 characters.");

        RuleFor(d => d.TailLength)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Tail length must be greater than 0.");

        RuleFor(d => d.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.");
    }
}
