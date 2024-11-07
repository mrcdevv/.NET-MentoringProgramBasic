using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileConfigurationProviderLibrary;

public class FileConfigurationProvider
{
    private readonly string _filePath;

    public FileConfigurationProvider(string filePath = "./settings.txt")
    {
        _filePath = filePath;
    }

    public async Task<object> GetSettingAsync(Type type, string settingName)
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"The configuration file '{_filePath}' was not found.");
        }

        using (var reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var keyValue = line.Split('=');
                if (keyValue.Length == 2 && keyValue[0].Trim() == settingName)
                {
                    return Convert.ChangeType(keyValue[1].Trim(), type);
                }
            }
        }

        throw new ArgumentException($"Setting '{settingName}' not found in the file.");
    }

    public async Task SetSettingAsync(string settingName, object value)
    {
        var lines = new List<string>();

        if (File.Exists(_filePath))
        {
            lines.AddRange(await File.ReadAllLinesAsync(_filePath));
        }

        bool settingFound = false;

        for (int i = 0; i < lines.Count; i++)
        {
            var keyValue = lines[i].Split('=');
            if (keyValue.Length == 2 && keyValue[0].Trim() == settingName)
            {
                lines[i] = $"{settingName}={value}";
                settingFound = true;
                break;
            }
        }

        if (!settingFound)
        {
            lines.Add($"{settingName}={value}");
        }

        await File.WriteAllLinesAsync(_filePath, lines);
    }
}
