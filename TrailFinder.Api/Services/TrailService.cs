using Npgsql;
using Dapper;
using TrailFinder.Api.Models;
using TrailFinder.Api.Services.Interfaces;

namespace TrailFinder.Api.Services;

public class TrailService : ITrailService
{
    private readonly string _connectionString;

    public TrailService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Trail>> GetTrailsAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Trail>(@"
            SELECT *,
                   ST_Y(start_point::geometry) as start_point_latitude,
                   ST_X(start_point::geometry) as start_point_longitude
            FROM trails");
    }

    public async Task<Trail?> GetTrailBySlugAsync(string slug)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Trail>(@"
            SELECT *,
                   ST_Y(start_point::geometry) as start_point_latitude,
                   ST_X(start_point::geometry) as start_point_longitude
            FROM trails 
            WHERE slug = @Slug", new { Slug = slug });
    }

    public Task<Trail> CreateTrailAsync(Trail trail)
    {
        throw new NotImplementedException();
    }

    public Task<Trail?> UpdateTrailAsync(string id, Trail trail)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTrailAsync(string id)
    {
        throw new NotImplementedException();
    }
}