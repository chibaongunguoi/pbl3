namespace module_io;

class IniReader
{
    public static Dictionary<string, Dictionary<string, string>> fetch(string file_path)
    {
        Dictionary<string, Dictionary<string, string>> m_content =
            new Dictionary<string, Dictionary<string, string>>();
        m_content.Clear();
        string current_section = "";

        foreach (var line in File.ReadAllLines(file_path))
        {
            var trimmed_line = line.Trim();

            if (
                string.IsNullOrEmpty(trimmed_line)
                || trimmed_line.StartsWith(";")
                || trimmed_line.StartsWith("#")
            )
            {
                continue;
            }

            if (trimmed_line.StartsWith("[") && trimmed_line.EndsWith("]"))
            {
                current_section = trimmed_line.Substring(1, trimmed_line.Length - 2);
                if (!m_content.ContainsKey(current_section))
                {
                    m_content[current_section] = new Dictionary<string, string>();
                }
            }
            else if (current_section != "")
            {
                var key_and_value = trimmed_line.Split(new[] { '=' }, 2);
                if (key_and_value.Length == 2)
                {
                    var key = key_and_value[0].Trim();
                    var value = key_and_value[1].Trim();
                    m_content[current_section][key] = value;
                }
            }
        }

        return m_content;
    }
}
