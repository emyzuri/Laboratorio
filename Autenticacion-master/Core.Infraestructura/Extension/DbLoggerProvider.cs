using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Infraestructura.Extension
{
    [ProviderAlias("Database")]
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;
        public DbLoggerProvider(IOptions<DbLoggerOptions> _options)
        {
            Options = _options.Value;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {
        }
    }
}