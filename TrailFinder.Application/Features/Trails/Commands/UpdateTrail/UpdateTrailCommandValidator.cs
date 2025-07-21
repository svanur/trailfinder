// TrailFinder.Application/Features/Trails/Commands/UpdateTrail/UpdateTrailCommandValidator.cs
using FluentValidation;
using NetTopologySuite.Geometries; // Assuming this is for LineString

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public class UpdateTrailCommandValidator : AbstractValidator<UpdateTrailCommand>
{
    public UpdateTrailCommandValidator()
    {
        // 1. Validate the TrailId (crucial for update commands)
        RuleFor(t => t.TrailId)
            .NotEmpty().WithMessage("Trail ID cannot be empty")
            .Must(id => id != Guid.Empty).WithMessage("Trail ID cannot be an empty GUID")
            .Must(BeValidGuid).WithMessage("Trail ID must be a valid GUID format");

        // 2. Validate 'Name'
        //    - It's nullable in UpdateTrailCommand, so `NotEmpty()` is conditional.
        //    - `When(x => x.Name is not null)` means "only apply this rule if Name is provided in the request".
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Trail name cannot exceed 100 characters.")
            .When(x => x.Name is not null); // Apply if Name is provided (not null)

        // 3. Validate 'Description'
        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description cannot exceed 2000 characters.")
            .When(x => x.Description is not null); // Apply if Description is provided

        // 4. Validate 'Distance'
        RuleFor(x => x.Distance)
            .GreaterThan(0)
            .WithMessage("Distance must be greater than 0.") // Assuming it's decimal/double, no specific unit in validation
            .When(x => x.Distance.HasValue); // Apply if Distance is provided

        // 5. Validate 'ElevationGain'
        RuleFor(x => x.ElevationGain)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Elevation gain cannot be negative.")
            .When(x => x.ElevationGain.HasValue); // Apply if ElevationGain is provided

        // 7. Validate 'UpdatedBy' (mandatory for audit)
        RuleFor(x => x.UpdatedBy)
            .NotEmpty()
            .WithMessage("User ID is required for update audit.");

        // 8. Validate Nullable Enums
        //    - `IsInEnum()` on a nullable property implicitly handles `null` correctly (it won't fail if null).
        //    - If `null` is a valid "no change" state, this is sufficient.
        //    - If you needed to ensure a value was provided *if the enum property itself was not nullable*, you'd add `.NotNull()` first.
        RuleFor(x => x.DifficultyLevel)
            .IsInEnum()
            .WithMessage("Invalid difficulty level.")
            .When(x => x.DifficultyLevel.HasValue); // Apply only if a value is provided

        RuleFor(x => x.SurfaceType)
            .IsInEnum()
            .WithMessage("Invalid surface type.")
            .When(x => x.SurfaceType.HasValue);

        RuleFor(x => x.RouteType)
            .IsInEnum()
            .WithMessage("Invalid route type.")
            .When(x => x.RouteType.HasValue);

        RuleFor(x => x.TerrainType)
            .IsInEnum()
            .WithMessage("Invalid terrain type.")
            .When(x => x.TerrainType.HasValue);

        // 9. Validate 'RouteGeom' (if present)
        RuleFor(x => x.RouteGeom)
            .Must(geom => geom != null && geom.Coordinates.Length >= 2) // Basic LineString validation
            .When(x => x.RouteGeom is not null) // Only validate if LineString is provided in the update
            .WithMessage("Trail geometry must be a valid LineString with at least two coordinates.");

        // You could also add more advanced geometry validation if needed, e.g.:
        // .Must(geom => geom.IsValid) // Requires NetTopologySuite's IsValid property/method
        // .WithMessage("Trail geometry is not valid.");
    }
    
    private static bool BeValidGuid(Guid id)
    {
        // Additional custom validation, if needed
        // For example, checking if it follows a specific format or pattern
        return id.ToString().Length == 36; // Standard GUID length
    }
}
