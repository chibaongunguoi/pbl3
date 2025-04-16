using Microsoft.Data.SqlClient;

sealed class Request : DataObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int semester_id { get; set; }
    public DateOnly date { get; set; } = new();
    public string state { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        stu_id = DataReader.getInt(reader, ref pos);
        semester_id = DataReader.getInt(reader, ref pos);
        date = DataReader.getDate(reader, ref pos);
        state = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.toStr(stu_id));
        lst.Add(QPiece.toStr(semester_id));
        lst.Add(QPiece.toStr(date));
        lst.Add(QPiece.toStr(state));
        return lst;
    }

    // ========================================================================
}

/* EOF */
