using DeliveryApp.Application.DTOs.Request.Rentals;
using FluentValidation;

namespace DeliveryApp.Application.Validators.Requests
{
    public class CreateRentalValidator : AbstractValidator<CreateRentalDTO>
    {
        public CreateRentalValidator()
        {
            RuleFor(x => x.DriverId)
               .NotEmpty().WithMessage("DriverId is required.");

            RuleFor(x => x.MotorcycleId)
                .NotEmpty().WithMessage("MotorcycleId is required.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("StartTime is required.")
                .GreaterThan(DateTime.MinValue).WithMessage("StartTime must be a valid date.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("EndTime is required.")
                .GreaterThan(x => x.StartTime).WithMessage("EndTime must be greater than StartTime.");

            RuleFor(x => x.PrevisionTime)
                .NotEmpty().WithMessage("PrevisionTime is required.")
                .GreaterThan(x => x.StartTime).WithMessage("PrevisionTime must be greater than StartTime.");

            RuleFor(x => x.PlanType)
                .Must(plan => plan == 7 || plan == 15 || plan == 30 || plan == 45 || plan == 50)
                .WithMessage("PlanType must be one of the following: 7, 15, 30, 45 or 50.");
        }
    }
}
