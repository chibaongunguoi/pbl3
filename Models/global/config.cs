namespace module_config;

using module_io;

class Config
{
    private static Dictionary<string, Dictionary<string, string>> s_config =
        new Dictionary<string, Dictionary<string, string>>();

    public static void load()
    {
        s_config = IniReader.fetch("config.ini");
    }

    public static string get_config(string section, string key)
    {
        if (s_config.ContainsKey(section) && s_config[section].ContainsKey(key))
        {
            return s_config[section][key];
        }
        return "";
    }
}
