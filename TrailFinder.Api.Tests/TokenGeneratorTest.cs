using JetBrains.Annotations;
using TrailFinder.Api;
using Xunit;

namespace TrailFinder.Api.Tests;

[TestSubject(typeof(TokenGenerator))]
public class TokenGeneratorTest
{

    [Fact]
    public void GenerateToken()
    {
        var generateJwtToken = TokenGenerator.GenerateJwtToken();
        Console.WriteLine(generateJwtToken);
    }
}