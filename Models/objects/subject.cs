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
        Id = DataReader.getInt(reader, ref pos);
        Name = DataReader.getStr(reader, ref pos);
        Grade = DataReader.getInt(reader, ref pos);
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
