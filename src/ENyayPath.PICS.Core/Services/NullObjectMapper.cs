using ENyayPath.PICS.Core.Dependency;
using ENyayPath.PICS.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Services
{
    /// <summary>
    /// Null implementation of IObjectMapper.
    /// Throws exceptions to indicate mapping is not configured.
    /// </summary>
    public class NullObjectMapper : IObjectMapper, ITransientDependency
    {
        public static NullObjectMapper Instance { get; } = new NullObjectMapper();

        // Public parameterless constructor so DI can instantiate it
        public NullObjectMapper() { }

        public TDestination Map<TDestination>(object source)
        {
            throw new AppException("ObjectMapper is not configured. Please register a real implementation.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new AppException("ObjectMapper is not configured. Please register a real implementation.");
        }
    }
}
