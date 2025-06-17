using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TrailFinder.Infrastructure.Models;

[Table("trails")]  // Specify the actual table name from your Supabase database
public class SupabaseTrail : BaseModel
{
    [Column("id")]
    public string Id { get; set; } = string.Empty;

    [Column("parent_id")]
    public string ParentId { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("slug")]
    public string Slug { get; set; } = string.Empty;
}