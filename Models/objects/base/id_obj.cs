using Microsoft.Data.SqlClient;

class IdObj : DataObj
{
    // ========================================================================
    public int id { get; set; } = 0;

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        id = DataReader.get_int(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override Dictionary<string, string> to_dict()
    {
        Dictionary<string, string> dict = base.to_dict();
        dict["id"] = id.ToString();
        return dict;
    }

    // ========================================================================
}

/* EOF */
