using Microsoft.Data.SqlClient;

sealed class Rating : DataObj
{
    // ========================================================================
    public int StuId { get; set; }
    public int SemesterId { get; set; }
    public DateTime Timestamp { get; set; } = new();
    public int Stars { get; set; }
    public string Description { get; set; } = "";

    // ========================================================================
    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        StuId = QDataReader.GetInt(reader, ref pos);
        SemesterId = QDataReader.GetInt(reader, ref pos);
        Timestamp = QDataReader.GetDateTime(reader, ref pos);
        Stars = QDataReader.GetInt(reader, ref pos);
        Description = QDataReader.GetString(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(StuId));
        lst.Add(QPiece.toStr(SemesterId));
        lst.Add(QPiece.toStr(Timestamp));
        lst.Add(QPiece.toStr(Stars));
        lst.Add(QPiece.toNStr(Description));
        return lst;
    }

    // ========================================================================
}

/* EOF */
