using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public string thumbnail = "";
    public string description = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        thumbnail = DataReader.getStr(reader, ref pos);
        description = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(thumbnail));
        lst.Add(QPiece.toStr(description));
        return lst;
    }

    // ========================================================================
}

/* EOF */
