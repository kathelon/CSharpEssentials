namespace CSharpEssentials.RequestResponseLogging.LogWriters;

#if NET8_0_OR_GREATER
internal sealed class LoggerFactoryLogWriter(ILoggerFactory loggerFactory,
                              LoggingOptions options) : ILogWriter
{
    private readonly ILogger _logger = loggerFactory.CreateLogger(options.LoggerCategoryName);
#else
internal sealed class LoggerFactoryLogWriter : ILogWriter
{
    private readonly ILogger _logger;
    private readonly LogLevel _loggingLevel;
    private readonly List<LogFields> _loggingFields;

    public LoggerFactoryLogWriter(ILoggerFactory loggerFactory, LoggingOptions options)
    {
        _logger = loggerFactory.CreateLogger(options.LoggerCategoryName);
        _loggingLevel = options.LoggingLevel;
        _loggingFields = options.LoggingFields;
        MessageCreator = options.UseSeparateContext
                            ? new LogMessageWithContextCreator(options)
                            : new LogMessageCreator(options);
    }
#endif

#if NET8_0_OR_GREATER
    public IMessageCreator MessageCreator { get; } = options.UseSeparateContext
                            ? new LogMessageWithContextCreator(options)
                            : new LogMessageCreator(options);
#else
    public IMessageCreator MessageCreator { get; }
#endif

    public Task Write(RequestResponseContext requestResponseContext)
    {
#if NET8_0_OR_GREATER
        LogLevel logLevel = options.LoggingLevel;
        List<LogFields> loggingFields = options.LoggingFields;
#else
        LogLevel logLevel = _loggingLevel;
        List<LogFields> loggingFields = _loggingFields;
#endif
        if (!_logger.IsEnabled(logLevel))
            return Task.CompletedTask;

        (string logString, List<string?>? values) = MessageCreator.Create(requestResponseContext);
        RequestResponseLogState state = values is null
            ? RequestResponseLogState.FromMessage(logString)
            : RequestResponseLogState.FromFields(logString, loggingFields, values);

        _logger.Log(logLevel, default, state, null, RequestResponseLogState.Format);

        return Task.CompletedTask;
    }
}
