using Microsoft.Data.SqlClient;

sealed class Request : DataObj
{
    // ========================================================================
    public int StuId { get; set; }
    public int SemesterId { get; set; }
    public DateTime Timestamp { get; set; } = new();
    public string Status { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        StuId = DataReader.getInt(reader, ref pos);
        SemesterId = DataReader.getInt(reader, ref pos);
        Timestamp = DataReader.getDateTime(reader, ref pos);
        Status = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(StuId));
        lst.Add(QPiece.toStr(SemesterId));
        lst.Add(QPiece.toStr(Timestamp));
        lst.Add(QPiece.toStr(Status));
        return lst;
    }

    // ========================================================================
}

/* EOF */
