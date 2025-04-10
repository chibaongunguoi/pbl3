using Microsoft.Data.SqlClient;

class IdObj : DataObj
{
    // ========================================================================
    public int id { get; set; } = 0;

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        id = DataReader.get_int(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{id}");
        return lst;
    }

    // ========================================================================
}

/* EOF */
