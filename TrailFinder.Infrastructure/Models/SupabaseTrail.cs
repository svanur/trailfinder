// TrailFinder.Infrastructure/Models/SupabaseTrail.cs
using Supabase.Postgrest.Models;

namespace TrailFinder.Infrastructure.Models;

public class SupabaseTrail : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    
    // Add other properties that match your Supabase table structure
}