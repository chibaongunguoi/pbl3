using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string name = "";
    public string gender = "";
    public DateOnly bday = new();

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        name = DataReader.get_string(reader, ref pos);
        gender = DataReader.get_string(reader, ref pos);
        bday = DataReader.get_date(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.ToNStr(name));
        lst.Add(QPiece.ToStr(gender));
        lst.Add(QPiece.ToStr(bday));
        return lst;
    }

    // ========================================================================
}

/* EOF */
