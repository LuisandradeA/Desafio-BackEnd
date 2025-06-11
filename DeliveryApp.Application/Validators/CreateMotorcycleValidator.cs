using DeliveryApp.Application.DTOs;
using FluentValidation;

namespace DeliveryApp.Application.Validators
{
    public class CreateMotorcycleValidator : AbstractValidator<CreateMotorcycleDTO>
    {
        public CreateMotorcycleValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.");
            
            RuleFor(x => x.Year)
                .NotEmpty()
                    .WithMessage("Year is required.")
                .InclusiveBetween(1900, DateTime.Now.Year)
                    .WithMessage("Year must be between 1900 and the current year.");
            
            RuleFor(x => x.ModelName)
                .NotEmpty().WithMessage("Model is required.");

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("License plate is required.");
        }
    }
}
