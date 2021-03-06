using Microsoft.Extensions.Logging;

namespace NetStash.Extensions.Logging
{
    public static class NetStashFactoryExtensions
    {
        public static ILoggerFactory AddNetStash(this ILoggerFactory loggerFactory, NetStashOptions options)
        {
            loggerFactory.AddProvider(new NetStashLoggerProvider(options));
            return loggerFactory;
        }
    }
}