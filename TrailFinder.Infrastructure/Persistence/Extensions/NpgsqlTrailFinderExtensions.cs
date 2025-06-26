using Npgsql;
using TrailFinder.Core.Enums;

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
        dataSourceBuilder.MapEnum<RouteType>("route_type", new NpgsqlNullNameTranslator());
        dataSourceBuilder.MapEnum<TerrainType>("terrain_type", new NpgsqlNullNameTranslator());
    
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