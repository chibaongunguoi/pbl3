using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public string thumbnail = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        thumbnail = DataReader.get_string(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return string.Join(",", new string[] { base.ToString(), thumbnail });
    }

    // ------------------------------------------------------------------------
    public override Dictionary<string, string> to_dict()
    {
        Dictionary<string, string> dict = base.to_dict();
        dict["thumbnail"] = thumbnail;
        return dict;
    }

    // ========================================================================
}

/* EOF */
