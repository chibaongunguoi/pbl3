using Microsoft.Data.SqlClient;

sealed class Semester : DataObj
{
    // ========================================================================
    public int Id { get; set; }
    public int CourseId { get; set; }
    public DateOnly StartDate { get; set; } = new();
    public DateOnly FinishDate { get; set; } = new();
    public int Capacity { get; set; }
    public int Fee { get; set; }
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";

    // ========================================================================
    public override void fetch(SqlDataReader reader, ref int pos)
    {
        base.fetch(reader, ref pos);
        Id = DataReader.getInt(reader, ref pos);
        CourseId = DataReader.getInt(reader, ref pos);
        StartDate = DataReader.getDate(reader, ref pos);
        FinishDate = DataReader.getDate(reader, ref pos);
        Capacity = DataReader.getInt(reader, ref pos);
        Fee = DataReader.getInt(reader, ref pos);
        Description = DataReader.getStr(reader, ref pos);
        Status = DataReader.getStr(reader, ref pos);
    }

    // ------------------------------------------------------------------------
    public override List<string> ToList()
    {
        var lst = base.ToList();
        lst.Add(QPiece.toStr(Id));
        lst.Add(QPiece.toStr(CourseId));
        lst.Add(QPiece.toStr(StartDate));
        lst.Add(QPiece.toStr(FinishDate));
        lst.Add(QPiece.toStr(Capacity));
        lst.Add(QPiece.toStr(Fee));
        lst.Add(QPiece.toStr(Description));
        lst.Add(QPiece.toStr(Status));
        return lst;
    }

    // ========================================================================
}

/* EOF */
