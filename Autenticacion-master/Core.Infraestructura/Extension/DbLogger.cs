using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Infraestructura.Extension
{
    public class DbLogger : ILogger
    {
        private readonly DbLoggerProvider _dbLoggerProvider;

        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
        {
            _dbLoggerProvider = dbLoggerProvider;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var values = new JObject();
            if (_dbLoggerProvider?.Options?.LogFiles?.Any() ?? false)
            {
                foreach (var item in _dbLoggerProvider.Options.LogFiles)
                {
                    switch (item)
                    {
                        case "LogLevel":
                            if (!string.IsNullOrWhiteSpace(logLevel.ToString()))
                            {
                                values["LogLevel"] = logLevel.ToString();
                            }
                            break;
                        case "ThreadId":
                            values["ThreadId"] = threadId;
                            break;
                        case "EventId":
                            values["EventId"] = eventId.Id;
                            break;
                        case "EventName":
                            if (!string.IsNullOrWhiteSpace(eventId.Name))
                            {
                                values["EventName"] = eventId.Name;
                            }
                            break;
                        case "Message":
                            if (!string.IsNullOrWhiteSpace(formatter(state, exception)))
                            {
                                values["Message"] = formatter(state, exception);
                            }
                            break;
                        case "ExceptionMessage":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                            {
                                values["ExceptionMessage"] = exception?.Message;
                            }
                            break;
                        case "ExceptionStackTrace":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                            {
                                values["ExceptionStackTrace"] = exception?.StackTrace;
                            }
                            break;
                        case "ExceptionSource":
                            if (exception != null && !string.IsNullOrWhiteSpace(exception.Source))
                            {
                                values["ExceptionSource"] = exception?.Source;
                            }
                            break;
                    }
                }
            }

            //using (var command = new SqlCommand())
            //{
            //    command.Connection = connection;
            //    command.CommandType = System.Data.CommandType.Text;
            //    command.CommandText = string.Format("INSERT INTO {0} ([Valores], [Created])" +
            //        "VALUES (@Values, @Created)", _dbLoggerProvider.Options.LogTable);
            //    command.Parameters.Add(new SqlParameter("@Values", JsonConvert.SerializeObject(values, new JsonSerializerSettings
            //    {
            //        NullValueHandling = NullValueHandling.Ignore,
            //        DefaultValueHandling = DefaultValueHandling.Ignore,
            //        Formatting = Formatting.None
            //    }).ToString()));
            //    command.Parameters.Add(new SqlParameter("@Created", DateTimeOffset.Now));
            //    command.ExecuteNonQuery();
            //}
            //connection.Close();

        }
    }
}