using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ConfigurationManagerConfigurationProviderLibrary;

public class ConfigurationManagerConfigurationProvider
{
    public async Task<object> GetSettingAsync(Type type, string settingName)
    {
        return await Task.Run(() =>
        {
            string value = ConfigurationManager.AppSettings[settingName];
            if (value == null)
                throw new InvalidOperationException($"Setting '{settingName}' not found in app.config.");

            return type == typeof(TimeSpan) ? TimeSpan.Parse(value) : Convert.ChangeType(value, type);
        });
    }

    public async Task SetSettingAsync(string settingName, object value)
    {
        await Task.Run(() =>
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[settingName] == null)
                {
                    settings.Add(settingName, value.ToString());
                }
                else
                {
                    settings[settingName].Value = value.ToString();
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        });
    }
}
