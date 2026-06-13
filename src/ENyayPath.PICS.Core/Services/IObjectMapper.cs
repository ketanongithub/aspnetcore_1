using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Services
{
    /// <summary>
    /// Contract for object-to-object mapping (DTO ↔ entity).
    /// Abstracts AutoMapper or any other mapping library.
    /// </summary>
    public interface IObjectMapper : ITransientDependency
    {
        TDestination Map<TDestination>(object source);
        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
    }   
}
