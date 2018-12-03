using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NetStash.Extensions.Logging
{
    public static class NetStashBuilderExtensions
    {
        public static ILoggingBuilder AddNetStash(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, NetStashLoggerProvider>();
            return builder;
        }

        public static ILoggingBuilder AddNetStash(this ILoggingBuilder builder, Action<NetStashOptions> configure)
        {
            builder.AddNetStash();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}