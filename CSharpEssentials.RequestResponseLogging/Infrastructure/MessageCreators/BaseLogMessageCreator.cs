using System.Globalization;

namespace CSharpEssentials.RequestResponseLogging.Infrastructure.MessageCreators;

internal abstract class BaseLogMessageCreator
{
    protected static string? GenerateLogStringByField(RequestResponseContext requestResponseContext, LoggingOptions loggingOptions, LogFields field) =>
        EscapeLineBreaks(GetFieldValue(requestResponseContext, loggingOptions, field));

    private static string? GetFieldValue(RequestResponseContext requestResponseContext, LoggingOptions loggingOptions, LogFields field)
    {
        return field switch
        {
            LogFields.Request => requestResponseContext.RequestBody,
            LogFields.Response => requestResponseContext.ResponseBody,
            LogFields.QueryString => requestResponseContext.Context.Request.QueryString.Value,
            LogFields.Path => requestResponseContext.Context.Request.Path.Value,
            LogFields.HostName => requestResponseContext.Context.Request.Host.Value,
            LogFields.ResponseTiming => requestResponseContext.ResponseTime,
            LogFields.RequestLength => requestResponseContext.RequestLength?.ToString(CultureInfo.InvariantCulture),
            LogFields.ResponseLength => requestResponseContext.ResponseLength?.ToString(CultureInfo.InvariantCulture),
            LogFields.Method => requestResponseContext.Context.Request.Method,
            LogFields.Headers => GetHeaders(requestResponseContext, loggingOptions.HeaderKeys),
            _ => string.Empty
        };
    }

    private static string GetHeaders(RequestResponseContext requestResponseContext, HashSet<string> headerKeys)
    {
        if (headerKeys.Count == 0)
            return string.Empty;

        IEnumerable<string> filteredHeaders = requestResponseContext.Context.Request.Headers
            .Where(header => headerKeys.Contains(header.Key))
            .Select(header => $"{{\"{header.Key}\":\"{header.Value}\"}}");

        return $"[{string.Join(",", filteredHeaders)}]";
    }

    private static string? EscapeLineBreaks(string? value) =>
        value?
            .Replace("\r", @"\r", StringComparison.Ordinal)
            .Replace("\n", @"\n", StringComparison.Ordinal)
            .Replace("\v", @"\v", StringComparison.Ordinal)
            .Replace("\f", @"\f", StringComparison.Ordinal)
            .Replace("\u0085", @"\u0085", StringComparison.Ordinal)
            .Replace("\u2028", @"\u2028", StringComparison.Ordinal)
            .Replace("\u2029", @"\u2029", StringComparison.Ordinal);

}
