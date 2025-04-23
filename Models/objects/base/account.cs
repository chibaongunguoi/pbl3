using Microsoft.Data.SqlClient;

class Account : DataObj
{
    // ========================================================================
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        Id = DataReader.getInt(reader, ref pos);
        Password = DataReader.getStr(reader, ref pos);
        Username = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(Id));
        lst.Add(QPiece.toStr(Username));
        lst.Add(QPiece.toStr(Password));
        return lst;
    }

    // ========================================================================
}

/* EOF */
