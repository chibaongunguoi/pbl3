using Microsoft.Data.SqlClient;

class Account : DataObj
{
    // ========================================================================
    public int id;
    public string username = "";
    public string password = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        id = DataReader.get_int(reader, pos++);
        username = DataReader.get_string(reader, pos++);
        password = DataReader.get_string(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return string.Join(",", new string[] { id.ToString(), username, password });
    }

    // ========================================================================
}

/* EOF */
