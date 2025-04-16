using Microsoft.Data.SqlClient;

class Subject : IdObj
{
    // ========================================================================
    public string name = "";
    public int grade;

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        name = DataReader.get_string(reader, ref pos);
        grade = DataReader.get_int(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.ToStr(name));
        lst.Add(QPiece.ToStr(grade));
        return lst;
    }

    // ========================================================================
}

/* EOF */
