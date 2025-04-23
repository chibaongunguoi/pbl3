using Microsoft.Data.SqlClient;

sealed class Course : DataObj
{
    // ========================================================================
    public int Id { get; set; }
    public int TchId { get; set; }
    public int SbjId { get; set; }
    public string Name { get; set; } = "";
    public string Status { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        Id = DataReader.getInt(reader, ref pos);
        TchId = DataReader.getInt(reader, ref pos);
        SbjId = DataReader.getInt(reader, ref pos);
        Name = DataReader.getStr(reader, ref pos);
        Status = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(Id));
        lst.Add(QPiece.toStr(TchId));
        lst.Add(QPiece.toStr(SbjId));
        lst.Add(QPiece.toNStr(Name));
        lst.Add(QPiece.toStr(Status));
        return lst;
    }

    // ========================================================================
}

/* EOF */
