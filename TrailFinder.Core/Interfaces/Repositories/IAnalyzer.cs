namespace TrailFinder.Core.Interfaces.Repositories;

public interface IAnalyzer<in TInput, out TOutput>
{
     /// <summary>
     /// Analyze the TInput and return the analysis as a TOutput
     /// </summary>
     /// <param name="item">An item of type TInput</param>
     /// <returns>An item of type TOutput</returns>
     TOutput Analyze(TInput item); 
}