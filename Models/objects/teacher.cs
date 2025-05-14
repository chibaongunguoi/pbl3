using Microsoft.Data.SqlClient;

public sealed class Teacher : User
{
    // ========================================================================
    public string Thumbnail = "";
    public string Description = "";

    // ========================================================================
    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
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
