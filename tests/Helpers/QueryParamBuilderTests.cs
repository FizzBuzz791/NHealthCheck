using System.Collections;
using NHealthCheck.Helpers;
using Shouldly;

namespace NHealthCheck.Tests.Helpers;

public class QueryParamBuilderTests
{

    [TestCaseSource(typeof(QueryParamBuilderTestData), nameof(QueryParamBuilderTestData.TestCases))]
    public void GivenQueryParamsInConstructorThenItReturnsWellFormedString(Dictionary<string, string> queryParams, string expectedOutput)
    {
        // Arrange
        var qbp = new QueryParamBuilder(queryParams);

        // Act
        var result = qbp.ToQueryString();

        // Assert
        result.ShouldBe(expectedOutput);
    }

    [TestCaseSource(typeof(QueryParamBuilderTestData), nameof(QueryParamBuilderTestData.TestCases))]
    public void GivenQueryParamsViaMethodThenReturnsWellFormedString(Dictionary<string, string> queryParams, string expectedOutput)
    {
        // Arrange
        var qpb = new QueryParamBuilder();
        foreach (var queryParam in queryParams)
        {
            qpb.AddQueryParam(queryParam.Key, queryParam.Value);
        }

        // Act
        var result = qpb.ToQueryString();

        // Assert
        result.ShouldBe(expectedOutput);
    }
}

public class QueryParamBuilderTestData
{
    public static IEnumerable TestCases
    {
        get
        {

            yield return new TestCaseData(new Dictionary<string, string>(), string.Empty);
            yield return new TestCaseData(new Dictionary<string, string> { { "key", "value" } }, "?key=value");
            yield return new TestCaseData(new Dictionary<string, string> { { "key", "value" }, { "key2", "value2" } }, "?key=value&key2=value2");
        }
    }
}