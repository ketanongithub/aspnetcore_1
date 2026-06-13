using ENyayPath.PICS.Core.Exceptions;
using ENyayPath.PICS.Core.Localization;
using ENyayPath.PICS.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace ENyayPath.PICS.Core.Services
{
    /// <summary>
    /// Base class for domain services.
    /// Encapsulates business logic, isolated from persistence.
    /// Provides settings, unit of work, localization, logging, and mapping.
    /// </summary>
    public abstract class DomainServiceBase : IDomainService
    {
        public ISettingManager SettingManager { get; set; }
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }
        protected IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current ?? throw new AppException("UnitOfWorkManager must be set before use.");

        public ILocalizationManager LocalizationManager { get; set; }
        protected string LocalizationSourceName { get; set; }
        private ILocalizationSource _localizationSource;

        protected ILocalizationSource LocalizationSource
            => _localizationSource ??= LocalizationManager.GetSource(LocalizationSourceName);

        public ILogger Logger { protected get; set; }
        public IObjectMapper ObjectMapper { get; set; }

        protected DomainServiceBase()
        {
            Logger = NullLogger.Instance;
            ObjectMapper = NullObjectMapper.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        protected virtual string L(string name) => LocalizationSource.GetString(name);
        protected virtual string L(string name, CultureInfo culture) => LocalizationSource.GetString(name, culture);
    }
}
