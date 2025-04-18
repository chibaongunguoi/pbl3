using Microsoft.Data.SqlClient;

class IdObj : DataObj
{
    // ========================================================================
    public int id { get; set; } = 0;

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        id = DataReader.getInt(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(id));
        return lst;
    }

    // ========================================================================
}

/* EOF */
