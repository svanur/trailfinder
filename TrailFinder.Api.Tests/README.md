# TrailFinder.Api.Tests

This project contains tests for the TrailFinder API using xUnit testing framework.

## Table of Contents

- [Setup](#setup)
- [xUnit Basics](#xunit-basics)
- [Assertions Reference](#assertions-reference)
- [Test Organization](#test-organization)
- [Best Practices](#best-practices)

## Setup

Required NuGet packages:

```xml
<PackageReference Include="coverlet.collector" Version="6.0.2"/>
<PackageReference Include="MediatR" Version="12.5.0" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="xunit" Version="2.9.3" />
```
 
## xUnit Basics

### Common Attributes

```csharp
- [Fact] - Basic test method
- [Theory] - Test with multiple data sets
- [InlineData(...)] - Data for theory
- [Trait("Category", "Integration")] - Test categorization
```

### Test Lifecycle

```csharp

public class TestClass : IDisposable 
{ 
    public TestClass() // Runs before each test 
    { 
        // Setup code 
    } 
    
    public void Dispose() // Runs after each test
    {
        // Cleanup code
    }
}
```

## Assertions Reference

### Basic Assertions

```csharp
// Equality 
Assert.Equal(expected, actual); Assert.NotEqual(expected, actual);

//Boolean 
Assert.True(condition); Assert.False(condition);

//Null checks 
Assert.Null(object); Assert.NotNull(object);
```

### Collection Assertions

```csharp

// Basic collection checks 
Assert.Empty(collection); 
Assert.NotEmpty(collection); 
Assert.Contains(expectedItem, collection);

// Complex collection checks 
Assert.Collection(collection, 
    item1 => Assert.Equal(expected1, item1), 
    item2 => Assert.Equal(expected2, item2) 
);
```

### Type & Exception Assertions

```csharp
// Type checks 
Assert.IsType (object); Assert. IsAssignableFrom  (object);  

// Exception testing 
await Assert.ThrowsAsync ( async () => await methodThatThrows());

// Ranges 
Assert.InRange(actual, min, max);
```

## Test Organization

### Naming Guidelines

- Test Classes: `[TestedClass]Tests` 
- Example: `TrailsControllerTests`

### Example Structure

```csharp

public class CalculatorTests { 
    [Fact] 
    public void Add_PositiveNumbers_ReturnsSum() 
    { 
        // Arrange 
        var calculator = new Calculator();
        
        // Act
        var result = calculator.Add(2, 3);
        
        // Assert
        Assert.Equal(5, result);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(0, 0, 0)]
    public void Add_MultipleInputs_ReturnsExpectedSum(int a, int b, int expected)
    {
        var calculator = new Calculator();
        var result = calculator.Add(a, b);
        Assert.Equal(expected, result);
    }
}
```

## Parameterized Tests

### Theory with InlineData

```csharp 
[Theory] 
[InlineData(2, 2, 4)] 
[InlineData(0, 5, 5)] 
[InlineData(-2, 2, 0)] 
public void Add_WhenCalled_ReturnsExpectedSum(int a, int b, int expected) 
{ 
   var calculator = new Calculator(); 
   var result = calculator.Add(a, b); Assert.Equal(expected, result); 
}
```

### MemberData for Complex Test Cases


### Benefits of Parameterized Tests
- Reduces code duplication
- Makes test cases more maintainable
- Clearly shows the relationship between inputs and expected outputs
- Easier to add new test cases
- Better test documentation

### Best Practices for Parameterized Tests
1. **Data Organization**
   - Keep test data organized and easy to maintain
   - Use meaningful test cases that cover edge cases
   - Consider using external data sources for large datasets

2. **Naming and Documentation**
   - Use descriptive names for test data classes
   - Document complex test cases
   - Include comments explaining special test cases

3. **Data Management**
   - Keep data sets focused and relevant
   - Avoid excessive test combinations
   - Consider using helper methods for complex data setup

4. **Error Cases**
   - Include both valid and invalid test cases
   - Test boundary conditions
   - Include null/empty values where appropriate


## Best Practices for Unit tests

1. **Test Structure**
    - Use AAA pattern (Arrange - Act - Assert)
    - Keep tests independent
    - One logical assertion per test
    - Use meaningful test data

1. **Naming and Organization**
    - Use clear, descriptive test names
    - Group related tests in test classes
    - Use appropriate attributes

1. **Test Maintainability**
    - Keep tests simple and readable
    - Avoid test interdependence
    - Use setup methods for common initialization
    - Clean up resources properly

1. **Coverage**
    - Test positive and negative scenarios
    - Include edge cases
    - Test async methods properly
