using DeliveryApp.Application.DTOs;
using DeliveryApp.Application.DTOs.Request;
using FluentValidation;

namespace DeliveryApp.Application.Validators.Requests
{
    public class UpdateLicensePlateValidator : AbstractValidator<UpdateLicensePlateDTO>
    {
        public UpdateLicensePlateValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.LicensePlate)
                .NotNull()
                .NotEmpty()
                .WithMessage("License plate is required.");
        }
    }
}
