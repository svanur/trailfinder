// TrailFinder.UnitTests\DifficultyAnalyzerFactoryTests.cs
using Xunit;
using FluentAssertions;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Services.TrailAnalysis; // Your factory and analyzers
using System;
using Microsoft.Extensions.DependencyInjection;
using TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis; // For IServiceProvider

public class DifficultyAnalyzerFactoryTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DifficultyAnalyzerFactory _factory;

    public DifficultyAnalyzerFactoryTests()
    {
        // Setup a simple DI container for testing
        var services = new ServiceCollection();
        
        // Register all your specific analyzer implementations
        services.AddTransient<PavedRouteDifficultyAnalyzer>();
        services.AddTransient<TrailRouteDifficultyAnalyzer>();
        // Add other specific analyzers if you create them (e.g., MixedRouteDifficultyAnalyzer)
        services.AddTransient<DifficultyAnalyzer>(); // Your fallback/original if it exists

        _serviceProvider = services.BuildServiceProvider();
        _factory = new DifficultyAnalyzerFactory(_serviceProvider);
    }

    [Fact]
    public void GetAnalyzer_ReturnsPavedRouteDifficultyAnalyzer_ForPavedSurfaceType()
    {
        // Act
        var analyzer = _factory.GetAnalyzer(SurfaceType.Paved);

        // Assert
        analyzer.Should().BeOfType<PavedRouteDifficultyAnalyzer>();
    }

    [Fact]
    public void GetAnalyzer_ReturnsTrailRouteDifficultyAnalyzer_ForTrailSurfaceType()
    {
        // Act
        var analyzer = _factory.GetAnalyzer(SurfaceType.Trail);

        // Assert
        analyzer.Should().BeOfType<TrailRouteDifficultyAnalyzer>();
    }

    [Fact]
    public void GetAnalyzer_ReturnsDefaultDifficultyAnalyzer_ForUnknownSurfaceType()
    {
        // Act
        var analyzer = _factory.GetAnalyzer(SurfaceType.Unknown);

        // Assert
        analyzer.Should().BeOfType<DifficultyAnalyzer>();
    }

    // Add tests for other SurfaceTypes (e.g., Mixed) as you implement them
}