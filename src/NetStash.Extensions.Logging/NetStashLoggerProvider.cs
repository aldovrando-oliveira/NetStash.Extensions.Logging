using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetStash.Core.Log;
using NetStash.Extensions.Logging.Infra;

namespace NetStash.Extensions.Logging
{
    [ProviderAlias("NetStash")]
    public class NetStashLoggerProvider : ILoggerProvider
    {
        private bool _disposed;
        private readonly NetStashLog _netStash;
        private readonly Dictionary<string, string> _extraValues;

        public NetStashLoggerProvider(IOptions<NetStashOptions> options) : this(options.Value)
        {
        }

        public NetStashLoggerProvider(NetStashOptions options)
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;

            _disposed = false;
            _netStash = new NetStashLog(options.Host, options.Port, appName, options.AppName ?? appName);
            _extraValues = options.ExtraValues ?? new Dictionary<string, string>();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NetStashLogger(this, categoryName);
        }

        public void AddMessage(LogLevel level, string message, Exception exception, Dictionary<string, string> additionalValues)
        {
            var extra = _extraValues.CloneDictionary();

            if (additionalValues != null)
                foreach (var item in additionalValues)
                    extra.Add(item.Key, item.Value);

            _netStash.Log(GetNetStashLogLevel(level), message, exception, extra);            
        }

        private NetStashLogLevel GetNetStashLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return NetStashLogLevel.Verbose;
                case LogLevel.Debug:
                    return NetStashLogLevel.Debug;
                case LogLevel.Warning:
                    return NetStashLogLevel.Warning;
                case LogLevel.Error:
                    return NetStashLogLevel.Error;
                case LogLevel.Critical:
                    return NetStashLogLevel.Fatal;
                case LogLevel.Information:
                default:
                    return NetStashLogLevel.Information;
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _netStash.Stop();
            _extraValues.Clear();

            _disposed = true;
        }
    }
}