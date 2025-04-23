using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string Name { get; set; } = "";
    public string Gender { get; set; } = "";
    public DateOnly Bday { get; set; } = new();

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        Name = DataReader.getStr(reader, ref pos);
        Gender = DataReader.getStr(reader, ref pos);
        Bday = DataReader.getDate(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toNStr(Name));
        lst.Add(QPiece.toStr(Gender));
        lst.Add(QPiece.toStr(Bday));
        return lst;
    }

    // ========================================================================
}

/* EOF */
