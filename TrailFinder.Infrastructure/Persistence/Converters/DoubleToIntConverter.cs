using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TrailFinder.Infrastructure.Persistence.Converters;

/// <summary>
/// Provides a converter that transforms a double value to an integer by rounding
/// the double value to the nearest integer value. This conversion can be used
/// for database or persistence purposes where only integer values are allowed.
/// </summary>
/// <remarks>
/// This implementation leverages the <see cref="ValueConverter{TModel, TProvider}"/>
/// class from Entity Framework Core to perform the conversion, ensuring compatibility
/// with EF Core's value conversion framework.
/// </remarks>
public class DoubleToIntConverter() : 
    ValueConverter<double, int>(
        v => (int)Math.Round(v), v => v)
{
    // Convert double to int (round)
    // Convert int back to double (implicit cast is fine)
}