using System.Text.Json;


public class ConfigOption
{
    public bool data_generator { get; set; } = false;
}

public class ConfigOptionManager
{
    private static ConfigOption s_config_option = new();
    private static string s_server_name = "";

    public static void load()
    {
        string config_option_json = File.ReadAllText("config.json");
        s_server_name = File.ReadAllText("server.txt").Trim();
        s_config_option = JsonSerializer.Deserialize<ConfigOption>(config_option_json) ?? new();
    }

    public static string get_server_name() => s_server_name;    
    public static bool get_data_generator() => s_config_option.data_generator;
}

/* EOF */
