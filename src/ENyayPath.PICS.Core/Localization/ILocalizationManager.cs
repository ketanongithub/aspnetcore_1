using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Localization
{
    public interface ILocalizationManager : ITransientDependency
    {
        //
        // Summary:
        //     Gets a localization source with name.
        //
        // Parameters:
        //   name:
        //     Unique name of the localization source
        //
        // Returns:
        //     The localization source
        ILocalizationSource GetSource(string name);

        //
        // Summary:
        //     Gets all registered localization sources.
        //
        // Returns:
        //     List of sources
        IReadOnlyList<ILocalizationSource> GetAllSources();
    }
}
