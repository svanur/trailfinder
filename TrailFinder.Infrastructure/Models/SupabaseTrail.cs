
/*
[Table("trails")]
public class SupabaseTrail : BaseModel
{
    [Column("id")]
    public Guid Id { get; set; }
        
    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("slug")]
    public string Slug { get; set; } = string.Empty;
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("distance")]
    public decimal Distance { get; set; }
    
    [Column("elevation_gain_meters")]
    public double ElevationGainMeters { get; set; }
    
    //[Column("difficulty_level")]
    //public DifficultyLevel? DifficultyLevel { get; set; }
    
    [Column("route_geom")]
    public object? RouteGeom { get; set; } // LINESTRING geometry
    
    [Column("start_point")]
    public object? StartPoint { get; set; } // POINT geometry
    
    [Column("end_point")]
    public object? EndPoint { get; set; } // POINT geometry
    
    [Column("web_url")]
    public string? WebUrl { get; set; }
    
    [Column("has_gpx")]
    public bool? HasGpx { get; set; }
    
    [Column("user_id")]
    public Guid UserId { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
*/