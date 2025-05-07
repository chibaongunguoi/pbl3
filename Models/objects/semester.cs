using Microsoft.Data.SqlClient;

public sealed class Semester : DataObj
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
    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        base.Fetch(reader, ref pos);
        Id = QDataReader.GetInt(reader, ref pos);
        CourseId = QDataReader.GetInt(reader, ref pos);
        StartDate = QDataReader.GetDateOnly(reader, ref pos);
        FinishDate = QDataReader.GetDateOnly(reader, ref pos);
        Capacity = QDataReader.GetInt(reader, ref pos);
        Fee = QDataReader.GetInt(reader, ref pos);
        Description = QDataReader.GetString(reader, ref pos);
        Status = QDataReader.GetString(reader, ref pos);
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
