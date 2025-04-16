using Microsoft.Data.SqlClient;

class Account : IdObj
{
    // ========================================================================
    public string username { get; set; } = "";
    public string password { get; set; } = "";

    // ========================================================================
    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        base.fetch_data(reader, ref pos);
        password = DataReader.get_string(reader, ref pos);
        username = DataReader.get_string(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(QPiece.ToStr(username));
        lst.Add(QPiece.ToStr(password));
        return lst;
    }

    // ========================================================================
}

/* EOF */
