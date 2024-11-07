using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Week2;

public abstract class ConfigurationComponentBase
{
    private object fileProvider;
    private object configManagerProvider;

    public ConfigurationComponentBase()
    {
        LoadProviders();
    }

    private void LoadProviders()
    {
        string fileProviderPath = Path.Combine("../../../Plugins", "FileConfigurationProviderLibrary.dll");
        var fileProviderAssembly = Assembly.LoadFrom(fileProviderPath);
        var fileProviderType = fileProviderAssembly.GetType("FileConfigurationProviderLibrary.FileConfigurationProvider");
        fileProvider = Activator.CreateInstance(fileProviderType, "./settings.txt");

        string configProviderPath = Path.Combine("../../../Plugins", "ConfigurationManagerConfigurationProviderLibrary.dll");
        var configProviderAssembly = Assembly.LoadFrom(configProviderPath);
        var configProviderType = configProviderAssembly.GetType("ConfigurationManagerConfigurationProviderLibrary.ConfigurationManagerConfigurationProvider");
        configManagerProvider = Activator.CreateInstance(configProviderType);
    }

    public async Task SaveSettingsAsync()
    {
        foreach (var property in GetType().GetProperties())
        {
            var attribute = property.GetCustomAttribute<ConfigurationItemAttribute>();
            if (attribute == null) continue;

            var value = property.GetValue(this);
            if (attribute.ProviderType == Provider.File)
            {
                var setMethod = fileProvider.GetType().GetMethod("SetSettingAsync");
                await (Task)setMethod.Invoke(fileProvider, new object[] { attribute.SettingName, value });
            }
            else if (attribute.ProviderType == Provider.ConfigurationManager)
            {
                var setMethod = configManagerProvider.GetType().GetMethod("SetSettingAsync");
                await (Task)setMethod.Invoke(configManagerProvider, new object[] { attribute.SettingName, value });
            }
        }
    }

    public async Task LoadSettingsAsync()
    {
        foreach (var property in GetType().GetProperties())
        {
            var attribute = property.GetCustomAttribute<ConfigurationItemAttribute>();
            if (attribute == null) continue;

            object value = null;
            if (attribute.ProviderType == Provider.File)
            {
                var getMethod = fileProvider.GetType().GetMethod("GetSettingAsync");
                value = await (Task<object>)getMethod.Invoke(fileProvider, new object[] { property.PropertyType, attribute.SettingName });
            }
            else if (attribute.ProviderType == Provider.ConfigurationManager)
            {
                var getMethod = configManagerProvider.GetType().GetMethod("GetSettingAsync");
                value = await (Task<object>)getMethod.Invoke(configManagerProvider, new object[] { property.PropertyType, attribute.SettingName });
            }

            if (value != null)
            {
                property.SetValue(this, value);
            }
        }
    }
}
