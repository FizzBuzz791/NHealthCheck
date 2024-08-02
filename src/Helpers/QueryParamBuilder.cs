using System.Text;

namespace NHealthCheck.Helpers;

public class QueryParamBuilder(Dictionary<string, string>? queryParams = null)
{
    private Dictionary<string, string> QueryParams { get; set; } = queryParams ?? [];

    public void AddQueryParam(string key, string value)
    {
        QueryParams.Add(key, value);
    }

    public string ToQueryString()
    {
        var sb = new StringBuilder();

        if (QueryParams.Count > 0)
        {
            sb.Append('?');

            foreach (var queryParam in QueryParams)
            {
                if (sb.Length > 1)
                {
                    sb.Append('&');
                }

                sb.Append($"{queryParam.Key}={queryParam.Value}");
            }
        }

        return sb.ToString();
    }
}