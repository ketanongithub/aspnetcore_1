using ENyayPath.PICS.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Localization
{
    public class LanguageManager : ILanguageManager
    {
        private readonly IRepository<ApplicationLanguageText, long> _languageTextRepository;

        public LanguageManager(IRepository<ApplicationLanguageText, long> languageTextRepository)
        {
            _languageTextRepository = languageTextRepository;
        }

        public async Task<string> GetTextAsync(string key, string source, string languageName, long? userId = null, int? tenantId = null)
        {
            // 1. User-level override
            if (userId.HasValue)
            {
                var userText = await _languageTextRepository.FirstOrDefaultAsync(
                    t => t.LanguageName == languageName && t.Source == source && t.Key == key && t.LastModifierUserId == userId);
                if (userText != null) return userText.Value;
            }

            // 2. Tenant-level override
            if (tenantId.HasValue)
            {
                var tenantText = await _languageTextRepository.FirstOrDefaultAsync(
                    t => t.LanguageName == languageName && t.Source == source && t.Key == key && t.TenantId == tenantId && t.LastModifierUserId == null);
                if (tenantText != null) return tenantText.Value;
            }

            // 3. System-level fallback
            var systemText = await _languageTextRepository.FirstOrDefaultAsync(
                t => t.LanguageName == languageName && t.Source == source && t.Key == key);
            if (systemText != null) return systemText.Value;

            // 4. Default to key if not found
            return key;
        }
    }
}
