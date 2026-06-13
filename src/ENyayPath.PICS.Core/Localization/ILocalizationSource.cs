using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ENyayPath.PICS.Core.Localization
{
    public interface ILocalizationSource : ITransientDependency
    {
        //
        // Summary:
        //     Unique Name of the source.
        string Name { get; }

        //
        // Summary:
        //     This method is called before first usage.
        //void Initialize(ILocalizationConfiguration configuration, IIocResolver iocResolver);

        //
        // Summary:
        //     Gets key for given value.
        //
        // Parameters:
        //   value:
        //     Value
        //
        //   culture:
        //     culture information
        //
        //   tryDefaults:
        //     True: Fallbacks to default language if not found in current culture.
        //
        // Returns:
        //     Key
        string FindKeyOrNull(string value, CultureInfo culture, bool tryDefaults = true);

        //
        // Summary:
        //     Gets localized string for given name in current language. Fallbacks to default
        //     language if not found in current culture.
        //
        // Parameters:
        //   name:
        //     Key name
        //
        // Returns:
        //     Localized string
        string GetString(string name);

        //
        // Summary:
        //     Gets localized string for given name and specified culture. Fallbacks to default
        //     language if not found in given culture.
        //
        // Parameters:
        //   name:
        //     Key name
        //
        //   culture:
        //     culture information
        //
        // Returns:
        //     Localized string
        string GetString(string name, CultureInfo culture);

        //
        // Summary:
        //     Gets localized string for given name in current language. Returns null if not
        //     found.
        //
        // Parameters:
        //   name:
        //     Key name
        //
        //   tryDefaults:
        //     True: Fallbacks to default language if not found in current culture.
        //
        // Returns:
        //     Localized string
        string GetStringOrNull(string name, bool tryDefaults = true);

        //
        // Summary:
        //     Gets localized string for given name and specified culture. Returns null if not
        //     found.
        //
        // Parameters:
        //   name:
        //     Key name
        //
        //   culture:
        //     culture information
        //
        //   tryDefaults:
        //     True: Fallbacks to default language if not found in current culture.
        //
        // Returns:
        //     Localized string
        string GetStringOrNull(string name, CultureInfo culture, bool tryDefaults = true);

        //
        // Summary:
        //     Gets list of localized strings for given names in current language. Fallbacks
        //     to default language if not found in current culture.
        //
        // Parameters:
        //   names:
        //     Key names
        //
        // Returns:
        //     Localized string
        List<string> GetStrings(List<string> names);

        //
        // Summary:
        //     Gets list of localized strings for given names and specified culture. Fallbacks
        //     to default language if not found in given culture.
        //
        // Parameters:
        //   names:
        //     Key names
        //
        //   culture:
        //     culture information
        //
        // Returns:
        //     Localized string
        List<string> GetStrings(List<string> names, CultureInfo culture);

        //
        // Summary:
        //     Gets list of localized strings for given names in current language. Returns null
        //     if not found.
        //
        // Parameters:
        //   names:
        //     Key name
        //
        //   tryDefaults:
        //     True: Fallbacks to default language if not found in current culture.
        //
        // Returns:
        //     Localized string
        List<string> GetStringsOrNull(List<string> names, bool tryDefaults = true);

        //
        // Summary:
        //     Gets list of localized strings for given names and specified culture. Returns
        //     null if not found.
        //
        // Parameters:
        //   names:
        //     Key name
        //
        //   culture:
        //     culture information
        //
        //   tryDefaults:
        //     True: Fallbacks to default language if not found in current culture.
        //
        // Returns:
        //     Localized string
        List<string> GetStringsOrNull(List<string> names, CultureInfo culture, bool tryDefaults = true);

        //
        // Summary:
        //     Gets all strings in current language.
        //
        // Parameters:
        //   includeDefaults:
        //     True: Fallbacks to default language texts if not found in current culture.
        IReadOnlyList<LocalizedString> GetAllStrings(bool includeDefaults = true);

        //
        // Summary:
        //     Gets all strings in specified culture.
        //
        // Parameters:
        //   culture:
        //     culture information
        //
        //   includeDefaults:
        //     True: Fallbacks to default language texts if not found in current culture.
        IReadOnlyList<LocalizedString> GetAllStrings(CultureInfo culture, bool includeDefaults = true);
    }
}
