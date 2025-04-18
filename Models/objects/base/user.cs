using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string name = "";
    public string gender = "";
    public DateOnly bday = new();

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        name = DataReader.getStr(reader, ref pos);
        gender = DataReader.getStr(reader, ref pos);
        bday = DataReader.getDate(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toNStr(name));
        lst.Add(QPiece.toStr(gender));
        lst.Add(QPiece.toStr(bday));
        return lst;
    }

    // ========================================================================
}

/* EOF */
