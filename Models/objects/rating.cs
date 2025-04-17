using Microsoft.Data.SqlClient;

sealed class Rating : DataObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int semester_id { get; set; }
    public DateTime timestamp { get; set; } = new();
    public int stars { get; set; }
    public string description { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        stu_id = DataReader.getInt(reader, ref pos);
        semester_id = DataReader.getInt(reader, ref pos);
        timestamp = DataReader.getDateTime(reader, ref pos);
        stars = DataReader.getInt(reader, ref pos);
        description = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.toStr(stu_id));
        lst.Add(QPiece.toStr(semester_id));
        lst.Add(QPiece.toStr(timestamp));
        lst.Add(QPiece.toStr(stars));
        lst.Add(QPiece.toNStr(description));
        return lst;
    }

    // ========================================================================
}

/* EOF */
