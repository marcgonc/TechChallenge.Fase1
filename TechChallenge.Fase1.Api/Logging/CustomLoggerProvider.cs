using System.Collections.Concurrent;

namespace TechChallenge.Fase1.Api.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration logerConfig;
    private readonly ConcurrentDictionary<string, CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration _loggerConfig)
    {
        logerConfig = _loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, logerConfig));
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
