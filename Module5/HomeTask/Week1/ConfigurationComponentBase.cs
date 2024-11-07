using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Week1;

public abstract class ConfigurationComponentBase
{
    private readonly FileConfigurationProvider fileProvider = new("settings.txt");
    private readonly ConfigurationManagerConfigurationProvider configManagerProvider = new();

    public async Task SaveSettingsAsync()
    {
        foreach (var property in GetType().GetProperties())
        {
            var attribute = property.GetCustomAttribute<ConfigurationItemAttribute>();
            if (attribute == null) continue;

            var value = property.GetValue(this);
            switch (attribute.ProviderType)
            {
                case Provider.File:
                    await fileProvider.SetSettingAsync(attribute.SettingName, value);
                    break;
                case Provider.ConfigurationManager:
                    await configManagerProvider.SetSettingAsync(attribute.SettingName, value);
                    break;
            }
        }
    }

    public async Task LoadSettingsAsync()
    {
        foreach (var property in GetType().GetProperties())
        {
            var attribute = property.GetCustomAttribute<ConfigurationItemAttribute>();
            if (attribute == null) continue;

            object value = attribute.ProviderType switch
            {
                Provider.File => await fileProvider.GetSettingAsync(property.PropertyType, attribute.SettingName),
                Provider.ConfigurationManager => await configManagerProvider.GetSettingAsync(property.PropertyType, attribute.SettingName),
                _ => null
            };

            if (value != null)
            {
                property.SetValue(this, value);
            }
        }
    }
}
