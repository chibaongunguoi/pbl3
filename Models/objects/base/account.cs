using Microsoft.Data.SqlClient;

class Account : DataObj
{
    // ========================================================================
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    // ========================================================================
    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        Id = QDataReader.GetInt(reader, ref pos);
        Password = QDataReader.GetString(reader, ref pos);
        Username = QDataReader.GetString(reader, ref pos);
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
