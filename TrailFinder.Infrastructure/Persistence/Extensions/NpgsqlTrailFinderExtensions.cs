using Npgsql;
using TrailFinder.Core.Enums;
using TrailFinder.Infrastructure.Persistence.PostgreSQL;

namespace TrailFinder.Infrastructure.Persistence.Extensions;

public static class NpgsqlTrailFinderExtensions
{
    public static NpgsqlDataSource CreateTrailFinderDataSource(string? connectionString)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    
        // Enable unmapped types support first
        dataSourceBuilder.EnableUnmappedTypes();
    
        // Map the enum with explicit name mapping
        dataSourceBuilder.MapEnum<DifficultyLevel>("difficulty_level", new NpgsqlNullNameTranslator());
        dataSourceBuilder.MapEnum<LocationType>("location_type", new NpgsqlNullNameTranslator());
        dataSourceBuilder.MapEnum<RaceStatus>("race_status", new NpgsqlNullNameTranslator()); // <-- ADDED THIS LINE
    
        // Configure NetTopologySuite for geometry support
        dataSourceBuilder.UseNetTopologySuite();
    
        return dataSourceBuilder.Build();
    }

 
    public class NpgsqlNullNameTranslator : INpgsqlNameTranslator
    {
        public string TranslateMemberName(string clrName) => clrName.ToLower();
        public string TranslateTypeName(string clrName) => clrName.ToLower();
    }

}