using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string Name { get; set; } = "";
    public string Gender { get; set; } = "";
    public DateOnly Bday { get; set; } = new();

    public string Tel {get;set; } = "";

    // ========================================================================
    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        Name = QDataReader.GetString(reader, ref pos);
        Gender = QDataReader.GetString(reader, ref pos);
        Bday = QDataReader.GetDateOnly(reader, ref pos);
        Tel = QDataReader.GetString(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toNStr(Name));
        lst.Add(QPiece.toStr(Gender));
        lst.Add(QPiece.toStr(Bday));
        lst.Add(QPiece.toStr(Tel));
        return lst;
    }

    // ========================================================================
}

/* EOF */
