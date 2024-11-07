using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week2
{
    public enum Provider
    {
        File,
        ConfigurationManager
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationItemAttribute : Attribute
    {
        public string SettingName { get; set; }
        public Provider ProviderType { get; set; }

        public ConfigurationItemAttribute(string settingName, Provider providerType)
        {
            SettingName = settingName;
            ProviderType = providerType;
        }
    }
}
