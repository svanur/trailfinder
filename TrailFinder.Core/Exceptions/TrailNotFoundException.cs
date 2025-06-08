namespace TrailFinder.Core.Exceptions;

public class TrailNotFoundException : DomainException
{
    public TrailNotFoundException(Guid id)
        : base($"Trail with ID {id} was not found.")
    {
    }

    public TrailNotFoundException(string slug)
        : base($"Trail with slug '{slug}' was not found.")
    {
    }
}