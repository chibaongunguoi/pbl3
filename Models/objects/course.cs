using Microsoft.Data.SqlClient;

public sealed class Course : DataObj
{
    // ========================================================================
    public int Id { get; set; }
    public int TchId { get; set; }
    public int SbjId { get; set; }
    public string Name { get; set; } = "";
    public string Status { get; set; } = "";

    // ========================================================================
    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        Id = QDataReader.GetInt(reader, ref pos);
        TchId = QDataReader.GetInt(reader, ref pos);
        SbjId = QDataReader.GetInt(reader, ref pos);
        Name = QDataReader.GetString(reader, ref pos);
        Status = QDataReader.GetString(reader, ref pos);
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
