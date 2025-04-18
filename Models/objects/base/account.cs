using Microsoft.Data.SqlClient;

class Account : IdObj
{
    // ========================================================================
    public string username { get; set; } = "";
    public string password { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        password = DataReader.getStr(reader, ref pos);
        username = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(username));
        lst.Add(QPiece.toStr(password));
        return lst;
    }

    // ========================================================================
}

/* EOF */
