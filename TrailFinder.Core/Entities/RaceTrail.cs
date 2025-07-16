using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class RaceTrail : BaseEntity
{
    private RaceTrail() { } // For EF Core

    public Guid RaceId { get; set; }
    public Guid TrailId { get; set; }
    public RaceStatus RaceStatus { get; set; }
    public string Comment { get; set; }
    public int DisplayOrder { get; set; }
}