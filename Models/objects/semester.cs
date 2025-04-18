using Microsoft.Data.SqlClient;

sealed class Semester : IdObj
{
    // ========================================================================
    public int course_id { get; set; }
    public DateOnly start_date { get; set; } = new();
    public DateOnly finish_date { get; set; } = new();
    public int capacity { get; set; }
    public int fee { get; set; }
    public string description { get; set; } = "";
    public string status { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        course_id = DataReader.getInt(reader, ref pos);
        start_date = DataReader.getDate(reader, ref pos);
        finish_date = DataReader.getDate(reader, ref pos);
        capacity = DataReader.getInt(reader, ref pos);
        fee = DataReader.getInt(reader, ref pos);
        description = DataReader.getStr(reader, ref pos);
        status = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(course_id));
        lst.Add(QPiece.toStr(start_date));
        lst.Add(QPiece.toStr(finish_date));
        lst.Add(QPiece.toStr(capacity));
        lst.Add(QPiece.toStr(fee));
        lst.Add(QPiece.toStr(description));
        lst.Add(QPiece.toStr(status));
        return lst;
    }

    // ========================================================================
}

/* EOF */
