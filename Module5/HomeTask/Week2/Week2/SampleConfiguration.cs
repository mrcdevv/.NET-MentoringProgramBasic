
namespace Week2;

public class SampleConfiguration : ConfigurationComponentBase
{
    [ConfigurationItem("MaxItems", Provider.File)]
    public int MaxItems { get; set; }

    [ConfigurationItem("Timeout", Provider.ConfigurationManager)]
    public string Timeout { get; set; }

    [ConfigurationItem("AppName", Provider.File)]
    public string AppName { get; set; }
}
