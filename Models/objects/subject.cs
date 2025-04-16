using Microsoft.Data.SqlClient;

class Subject : IdObj
{
    // ========================================================================
    public string name = "";
    public int grade;

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        name = DataReader.getStr(reader, ref pos);
        grade = DataReader.getInt(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.toNStr(name));
        lst.Add(QPiece.toStr(grade));
        return lst;
    }

    // ========================================================================
}

/* EOF */
