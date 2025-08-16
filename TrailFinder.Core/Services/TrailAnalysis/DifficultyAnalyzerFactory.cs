// TrailFinder.Core.Services.TrailAnalysis\DifficultyAnalyzerFactory.cs (New file)

using Microsoft.Extensions.DependencyInjection;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Services.TrailAnalysis;

public class DifficultyAnalyzerFactory(IServiceProvider serviceProvider)
{
   
    public IAnalyzer<DifficultyAnalysisInput, DifficultyLevel> GetAnalyzer(SurfaceType surfaceType)
    {
        return surfaceType switch
        {
            SurfaceType.Paved => serviceProvider.GetRequiredService<PavedRouteDifficultyAnalyzer>(),
            SurfaceType.Trail => serviceProvider.GetRequiredService<TrailRouteDifficultyAnalyzer>(),
            //SurfaceType.Mixed => _serviceProvider.GetRequiredService<MixedRouteDifficultyAnalyzer>(),
            _ => serviceProvider.GetRequiredService<DifficultyAnalyzer>() // A fallback Difficulty analyzer
        };
    }
}