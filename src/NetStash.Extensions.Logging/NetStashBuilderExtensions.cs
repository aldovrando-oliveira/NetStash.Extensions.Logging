using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetStash.Extensions.Logging
{
    public static class NetStashBuilderExtensions
    {
        public static ILoggingBuilder AddLogstash(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, NetStashLoggerProvider>();
            return builder;
        }

        public static ILoggingBuilder AddLogstash(this ILoggingBuilder builder, Action<NetStashOptions> configure)
        {
            builder.AddLogstash();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}