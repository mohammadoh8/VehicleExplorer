using FluentValidation;
using VehicleExplorer.Web.Services.Vehicles.Requests;

namespace VehicleExplorer.Web.Services.Vehicles.Validators
{
    public class VehicleMakesRequestValidator : AbstractValidator<VehicleMakesRequest>
    {
        public VehicleMakesRequestValidator()
        {
            RuleFor(request => request.Page)
                .GreaterThan(0)
                .WithMessage("Page must be bigger than 0");

            RuleFor(request => request.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100");

            RuleFor(request => request.Search)
                .MaximumLength(50)
                .WithMessage("Search must be 50 characters or less");
        }
    }
}
