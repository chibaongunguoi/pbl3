using Microsoft.Data.SqlClient;

class Account : IdObj
{
    // ========================================================================
    public string password { get; set; } = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        password = DataReader.get_string(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return string.Join(",", new string[] { id.ToString(), password });
    }

    // ------------------------------------------------------------------------
    public override Dictionary<string, string> to_dict()
    {
        Dictionary<string, string> dict = base.to_dict();
        dict["password"] = password;
        return dict;
    }

    // ========================================================================
}

/* EOF */
