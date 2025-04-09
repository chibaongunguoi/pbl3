using Microsoft.Data.SqlClient;

class Account : IdObj
{
    // ========================================================================
    public string username { get; set; } = "";
    public string password { get; set; } = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        pos = base.fetch_data(reader, ref pos);
        password = DataReader.get_string(reader, ref pos);
        username = DataReader.get_string(reader, ref pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(username);
        lst.Add(password);
        return lst;
    }

    // ========================================================================
}

/* EOF */
