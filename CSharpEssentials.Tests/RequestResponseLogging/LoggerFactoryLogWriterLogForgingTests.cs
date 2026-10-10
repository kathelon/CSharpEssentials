using CSharpEssentials.RequestResponseLogging;
using CSharpEssentials.RequestResponseLogging.LogWriters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CSharpEssentials.Tests.RequestResponseLogging;

public sealed class LoggerFactoryLogWriterLogForgingTests
{
    private const string OriginalFormatKey = "{OriginalFormat}";

    private static RequestResponseContext CreateContext(
        string? requestBody = null,
        string path = "/test",
        string method = "GET",
        (string Key, string Value)? header = null)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = method;
        httpContext.Request.Path = path;
        if (header is { } h)
            httpContext.Request.Headers[h.Key] = h.Value;

        return new RequestResponseContext(httpContext) { RequestBody = requestBody };
    }

    private static async Task<CapturedEntry> WriteAndCapture(LoggingOptions options, RequestResponseContext context)
    {
        using var provider = new CapturingLoggerProvider();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(provider));
        var writer = new LoggerFactoryLogWriter(loggerFactory, options);

        await writer.Write(context);

        return provider.Entries.Should().ContainSingle().Subject;
    }

    private static string[] Lines(string message) =>
        message.Split(["\r\n", "\n", "\r", "\v", "\f", "\u0085", "\u2028", "\u2029"], StringSplitOptions.None);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Write_Should_KeepMessageTemplateConstant_When_RequestDataContainsPlaceholders(bool useSeparateContext)
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = useSeparateContext,
            LoggingFields = [LogFields.Request, LogFields.Path]
        };

        CapturedEntry benign = await WriteAndCapture(options, CreateContext("plain body"));
        CapturedEntry hostile = await WriteAndCapture(options, CreateContext("{Path} {0} {{x}}", path: "/{Request}"));

        hostile.OriginalFormat.Should().Be(benign.OriginalFormat);
        hostile.OriginalFormat.Should().NotContain("{0}").And.NotContain("/{Request}");
        hostile.Message.Should().Contain("{Path} {0} {{x}}").And.Contain("/{Request}");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Write_Should_NotLetRequestBodyForgeLogLines_When_BodyContainsLineBreaks(bool useSeparateContext)
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = useSeparateContext,
            LoggingFields = [LogFields.Request, LogFields.Path]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext("ok\r\nPath: /forged\nMethod: DELETE\u2028Path: /x"));

        Lines(entry.Message).Should().NotContain(["Path: /forged", "Method: DELETE", "Path: /x"]);
        entry.Message.Should().Contain(@"ok\r\nPath: /forged\nMethod: DELETE\u2028Path: /x");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Write_Should_NotLetPathForgeLogLines_When_PathContainsLineBreaks(bool useSeparateContext)
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = useSeparateContext,
            LoggingFields = [LogFields.Path, LogFields.Method]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext(path: "/a\nMethod: DELETE"));

        Lines(entry.Message).Should().NotContain("Method: DELETE");
        Lines(entry.Message).Should().Contain(["Path: /a\\nMethod: DELETE", "Method: GET"]);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Write_Should_NotLetHeaderValueForgeLogLines_When_HeaderContainsLineBreaks(bool useSeparateContext)
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = useSeparateContext,
            LoggingFields = [LogFields.Headers, LogFields.Method],
            HeaderKeys = ["X-Request-Id"]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext(header: ("X-Request-Id", "abc\r\nMethod: DELETE")));

        Lines(entry.Message).Should().NotContain("Method: DELETE");
        entry.Message.Should().Contain(@"abc\r\nMethod: DELETE");
    }

    [Theory]
    [InlineData("\r", @"\r")]
    [InlineData("\n", @"\n")]
    [InlineData("\v", @"\v")]
    [InlineData("\f", @"\f")]
    [InlineData("\u0085", @"\u0085")]
    [InlineData("\u2028", @"\u2028")]
    [InlineData("\u2029", @"\u2029")]
    public async Task Write_Should_EscapeLineBreak_When_RequestBodyContainsIt(string lineBreak, string escaped)
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = true,
            LoggingFields = [LogFields.Request]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext($"a{lineBreak}Path: /forged"));

        entry.Properties.Should().Contain(new KeyValuePair<string, object?>("Request", $"a{escaped}Path: /forged"));
        Lines(entry.Message).Should().NotContain("Path: /forged");
    }

    [Fact]
    public async Task Write_Should_ExposeFormattedTextAsSingleProperty_When_UseSeparateContextIsFalse()
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = false,
            LoggingFields = [LogFields.Path, LogFields.Method]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext(path: "/users"));

        entry.OriginalFormat.Should().Be("{RequestResponseLog:l}");
        entry.Properties.Should().Contain(new KeyValuePair<string, object?>("RequestResponseLog", entry.Message));
    }

    [Fact]
    public async Task Write_Should_ExposeEachFieldAsProperty_When_UseSeparateContextIsTrue()
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = true,
            LoggingFields = [LogFields.Request, LogFields.Path, LogFields.Method]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext("a\nb", path: "/users"));

        entry.Properties.Should().Contain(new KeyValuePair<string, object?>("Request", @"a\nb"));
        entry.Properties.Should().Contain(new KeyValuePair<string, object?>("Path", "/users"));
        entry.Properties.Should().Contain(new KeyValuePair<string, object?>("Method", "GET"));
        entry.OriginalFormat.Should().Be(
            $"Request: {{Request}}{Environment.NewLine}Path: {{Path}}{Environment.NewLine}Method: {{Method}}{Environment.NewLine}");
    }

    [Theory]
    [InlineData(false, "Request: ")]
    [InlineData(true, "Request: (null)")]
    public async Task Write_Should_RenderTheSameMessageAsBefore_When_InputIsBenign(bool useSeparateContext, string nullRequestLine)
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = useSeparateContext,
            LoggingFields = [LogFields.Request, LogFields.Path, LogFields.Method]
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext(path: "/users"));

        entry.Message.Should().Be(
            $"{nullRequestLine}{Environment.NewLine}Path: /users{Environment.NewLine}Method: GET{Environment.NewLine}");
    }

    [Fact]
    public async Task Write_Should_LogAtConfiguredLevel_When_LevelIsWarning()
    {
        var options = new LoggingOptions
        {
            UseSeparateContext = true,
            LoggingFields = [LogFields.Path],
            LoggingLevel = LogLevel.Warning
        };

        CapturedEntry entry = await WriteAndCapture(options, CreateContext());

        entry.Level.Should().Be(LogLevel.Warning);
    }

    private sealed record CapturedEntry(LogLevel Level, string Message, IReadOnlyList<KeyValuePair<string, object?>> Properties)
    {
        public string? OriginalFormat =>
            Properties.FirstOrDefault(p => p.Key == OriginalFormatKey).Value as string;
    }

    private sealed class CapturingLoggerProvider : ILoggerProvider
    {
        public List<CapturedEntry> Entries { get; } = [];

        public ILogger CreateLogger(string categoryName) => new CapturingLogger(Entries);

        public void Dispose()
        {
        }
    }

    private sealed class CapturingLogger(List<CapturedEntry> entries) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            IReadOnlyList<KeyValuePair<string, object?>> properties =
                state as IReadOnlyList<KeyValuePair<string, object?>> ?? [];
            entries.Add(new CapturedEntry(logLevel, formatter(state, exception), [.. properties]));
        }
    }
}
