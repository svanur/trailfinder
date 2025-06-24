using Npgsql;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure.Persistence.PostgreSQL;

namespace TrailFinder.Infrastructure.Persistence.Extensions;

public static class NpgsqlTrailFinderExtensions
{
    public static NpgsqlDataSource CreateTrailFinderDataSource(string? connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    
        // Configure type mapping at the data source level
        dataSourceBuilder.EnableUnmappedTypes();
    
        // Map the enum with the fully qualified name (including schema)
        dataSourceBuilder.MapEnum<DifficultyLevel>("public.difficulty_level", new PostgresEnumNameTranslator());
    
        return dataSourceBuilder.Build();
    }

}