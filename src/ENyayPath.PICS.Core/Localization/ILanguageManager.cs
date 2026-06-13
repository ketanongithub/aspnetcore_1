using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Localization
{
    public interface ILanguageManager : ITransientDependency
    {
        Task<string> GetTextAsync(string key, string source, string languageName, long? userId = null, int? tenantId = null);
    }
}
