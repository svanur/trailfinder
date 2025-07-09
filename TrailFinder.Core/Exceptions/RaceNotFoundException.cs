namespace TrailFinder.Core.Exceptions;

public class RaceNotFoundException : DomainException
{
    public RaceNotFoundException()
        : base($"No race was found.")
    {
    }
    public RaceNotFoundException(int id)
        : base($"Race with ID {id} was not found.")
    {
    }
    
    public RaceNotFoundException(Guid id)
        : base($"Race with Guid {id} was not found.")
    {
    }

    public RaceNotFoundException(string slug)
        : base($"Race with slug '{slug}' was not found.")
    {
    }

    public RaceNotFoundException(Guid id, Exception exception)
        : base($"Race with id '{id}' was not found.", exception)
    {
    }
}