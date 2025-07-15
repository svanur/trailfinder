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

        /*
         //TODO: add new rules instead? For StartPoint and endPoint?
        RuleFor(x => x.StartPointLatitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90 degrees");

        RuleFor(x => x.StartPointLongitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180 degrees");
            */
        

        RuleFor(x => x.WebUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.WebUrl))
            .WithMessage("Web URL must be a valid URL");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

            //RuleFor(x => x.DifficultyLevel)
            //.IsInEnum()
            //.WithMessage("Invalid difficulty level");
    }
}