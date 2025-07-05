namespace TrailFinder.Core.Exceptions;

public class LocationNotFoundException : DomainException
{
    public LocationNotFoundException(int id)
        : base($"Location with ID {id} was not found.")
    {
    }
    
    public LocationNotFoundException(Guid id)
        : base($"Location with Guid {id} was not found.")
    {
    }

    public LocationNotFoundException(string slug)
        : base($"Location with slug '{slug}' was not found.")
    {
    }

    public LocationNotFoundException(Guid id, Exception exception)
        : base($"Location with id '{id}' was not found.", exception)
    {
    }
}