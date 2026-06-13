using ENyayPath.PICS.Core.Exceptions;
using ENyayPath.PICS.Core.Localization;
using ENyayPath.PICS.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ENyayPath.PICS.Core.Services
{
    /// <summary>
    /// Base class for all application services.
    /// Provides access to settings, unit of work, localization, logging, and object mapping.
    /// </summary>
    public abstract class AppServiceBase
    {
        /// <summary>
        /// Reference to the setting manager.
        /// </summary>
        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// Ensures transactional consistency across service calls.
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get => _unitOfWorkManager ?? throw new AppException("Must set UnitOfWorkManager before use.");
            set => _unitOfWorkManager = value;
        }
        private IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Gets current active unit of work.
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;

        /// <summary>
        /// Reference to the localization manager.
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// Gets/sets name of the localization source used in this service.
        /// </summary>
        protected string LocalizationSourceName { get; set; }

        /// <summary>
        /// Gets localization source.
        /// </summary>
        protected ILocalizationSource LocalizationSource
        {
            get
            {
                if (LocalizationSourceName == null)
                    throw new AppException("Must set LocalizationSourceName before using LocalizationSource.");

                if (_localizationSource == null || _localizationSource.Name != LocalizationSourceName)
                    _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);

                return _localizationSource;
            }
        }
        private ILocalizationSource _localizationSource;

        /// <summary>
        /// Logger for writing logs.
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Object mapper for DTO ↔ entity conversion.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        /// <summary>
        /// Constructor.
        /// Initializes default null implementations.
        /// </summary>
        protected AppServiceBase()
        {
            Logger = NullLogger.Instance;
            ObjectMapper = NullObjectMapper.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        protected virtual string L(string name) => LocalizationSource.GetString(name);

        /// <summary>
        /// Gets localized string for given key name and specified culture.
        /// </summary>
        protected virtual string L(string name, CultureInfo culture) => LocalizationSource.GetString(name, culture);
    }
}
