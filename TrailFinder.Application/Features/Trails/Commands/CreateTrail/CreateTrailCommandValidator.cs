// TrailFinder.Application/Features/Trails/Commands/CreateTrail/CreateTrailCommandValidator.cs
using FluentValidation;

namespace TrailFinder.Application.Features.Trails.Commands.CreateTrail;

public class CreateTrailCommandValidator : AbstractValidator<CreateTrailCommand>
{
    public CreateTrailCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Trail name must not be empty and cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000)
            .WithMessage("Description must not be empty and cannot exceed 2000 characters");

        RuleFor(x => x.Distance)
            .GreaterThan(0)
            .WithMessage("Distance must be greater than 0 meters");

        RuleFor(x => x.ElevationGain)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Elevation gain cannot be negative");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.DifficultyLevel)
            .NotNull().WithMessage("Difficulty level is required")
            .IsInEnum().WithMessage("Invalid difficulty level");

        RuleFor(x => x.SurfaceType)
            .NotNull().WithMessage("Surface type is required")
            .IsInEnum().WithMessage("Invalid surface type");

        RuleFor(x => x.RouteType)
            .NotNull().WithMessage("Route type is required")
            .IsInEnum().WithMessage("Invalid route type");

        RuleFor(x => x.TerrainType)
            .NotNull().WithMessage("Terrain type is required")
            .IsInEnum().WithMessage("Terrain type level");
        
        RuleFor(x => x.RouteGeom)
            .NotNull().WithMessage("Trail geometry is required.") // If mandatory
            .Must(geom => geom != null && geom.Coordinates.Length >= 2) // Basic LineString validation
            .WithMessage("Trail geometry must be a valid LineString with at least two coordinates.");
    }
}