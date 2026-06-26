using FluentValidation;
using VehicleExplorer.Web.Services.Vehicles.Requests;

namespace VehicleExplorer.Web.Services.Vehicles.Validators
{
    public sealed class VehicleModelsRequestValidator : AbstractValidator<VehicleModelsRequest>
    {
        public VehicleModelsRequestValidator()
        {
            RuleFor(request => request.MakeId)
                .GreaterThan(0)
                .WithMessage("Make Id must be bigger than 0");

            RuleFor(request => request.Year)
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
                .WithMessage("Model year is not valid");

            RuleFor(request => request.VehicleType)
                .NotEmpty()
                .WithMessage("Vehicle type is required");
        }
    }
}
