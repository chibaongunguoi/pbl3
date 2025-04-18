using Microsoft.Data.SqlClient;

sealed class Course : IdObj
{
    // ========================================================================
    public int tch_id { get; set; }
    public int sbj_id { get; set; }
    public string name { get; set; } = "";
    public string status { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        tch_id = DataReader.getInt(reader, ref pos);
        sbj_id = DataReader.getInt(reader, ref pos);
        name = DataReader.getStr(reader, ref pos);
        status = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(tch_id));
        lst.Add(QPiece.toStr(sbj_id));
        lst.Add(QPiece.toNStr(name));
        lst.Add(QPiece.toStr(status));
        return lst;
    }

    // ========================================================================
}

/* EOF */
