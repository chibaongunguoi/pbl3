using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public string thumbnail = "";
    public string description = "";

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        thumbnail = DataReader.get_string(reader, ref pos);
        description = DataReader.get_string(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.ToStr(thumbnail));
        lst.Add(QPiece.ToStr(description));
        return lst;
    }

    // ========================================================================
}

/* EOF */
