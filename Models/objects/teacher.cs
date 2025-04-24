using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public string Thumbnail = "";
    public string Description = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        Thumbnail = QDataReader.GetString(reader, ref pos);
        Description = QDataReader.GetString(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(Thumbnail));
        lst.Add(QPiece.toStr(Description));
        return lst;
    }

    // ========================================================================
}

/* EOF */
