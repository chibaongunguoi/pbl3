using Microsoft.Data.SqlClient;

class Subject : DataObj
{
    // ========================================================================
    public int Id { get; set; } 
    public string Name = "";
    public int Grade;

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        Id = QDataReader.GetInt(reader, ref pos);
        Name = QDataReader.GetString(reader, ref pos);
        Grade = QDataReader.GetInt(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(Id));
        lst.Add(QPiece.toNStr(Name));
        lst.Add(QPiece.toStr(Grade));
        return lst;
    }

    // ========================================================================
}

/* EOF */
