using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NetStash.Extensions.Logging
{
    public class NetStashLogger : ILogger
    {
        private readonly NetStashLoggerProvider _provider;
        private readonly string _categoryName;

        public NetStashLogger(NetStashLoggerProvider provider, string categoryName)
        {
            _provider = provider;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var builder = new StringBuilder();

            var additionalValues = new Dictionary<string, string>();

            if (eventId.Id > 0)
                additionalValues.Add("event.id", eventId.Id.ToString());
            
            if (!string.IsNullOrEmpty(eventId.Name))
                additionalValues.Add("event.name", eventId.Name);

            if (state != null)
            {
                // TODO: implementar fluxo com escopo
            }

            builder.Append(formatter(state, exception));

            
            _provider.AddMessage(logLevel, builder.ToString(), exception, additionalValues);
        }
    }
}