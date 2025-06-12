using DeliveryApp.Application.DTOs.Request.Driver;
using FluentValidation;

namespace DeliveryApp.Application.Validators.Requests
{
    public class CreateDriverValidator : AbstractValidator<CreateDriverDTO>
    {
        public CreateDriverValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Identifier is required.");

            RuleFor(x => x.Name).NotNull()
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Document)
                .NotNull()
                .NotEmpty().WithMessage("Document is required.")
                .MaximumLength(14).WithMessage("Document must not exceed 14 characters.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.");

            RuleFor(x => x.LicenseNumber)
                .NotEmpty().WithMessage("License number is required.");

            RuleFor(x => x.LicenseType)
                .NotEmpty().WithMessage("License type is required.")
                .Must(type => type == "A" || type == "B" || type == "A+B")
                .WithMessage("License type must be one of the following: A, B, A+B");
        }
    }
}
