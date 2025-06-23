using Npgsql;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure.Persistence.PostgreSQL;

namespace TrailFinder.Infrastructure.Persistence.Extensions;

public static class NpgsqlTrailFinderExtensions
{
    public static NpgsqlDataSource CreateTrailFinderDataSource(string? connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        
        dataSourceBuilder.MapEnum<DifficultyLevel>("difficulty_level", nameTranslator: new PostgresEnumNameTranslator());
        
        return dataSourceBuilder.Build();
    }
}