// TrailFinder.Infrastructure/Services/SupabaseService.cs
using AutoMapper;
using Supabase;
using Microsoft.Extensions.Options;
using Supabase.Postgrest;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Infrastructure.Configuration;
using TrailFinder.Infrastructure.Models;
using Client = Supabase.Client;

namespace TrailFinder.Infrastructure.Services;

public class SupabaseService : ISupabaseService
{
    private readonly Client _supabaseClient;
    private readonly IMapper _mapper;

    public SupabaseService(
        IOptions<SupabaseSettings> settings,
        IMapper mapper)
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };

        _supabaseClient = new Client(
            settings.Value.Url,
            settings.Value.Key,
            options);
            
        _mapper = mapper;
    }

    public async Task<TrailDto?> GetTrailBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var trail = await _supabaseClient
            .From<SupabaseTrail>()
            .Select("*")
            .Filter("slug", Constants.Operator.Equals, slug)
            .Single(cancellationToken);

        return trail != null ? _mapper.Map<TrailDto>(trail) : null;
    }
}