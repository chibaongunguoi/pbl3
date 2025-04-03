using Microsoft.Data.SqlClient;

sealed class Request : DataObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int semester_id { get; set; }
    public InfoDate date { get; set; } = new InfoDate();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        stu_id = DataReader.get_int(reader, pos++);
        semester_id = DataReader.get_int(reader, pos++);
        pos = date.fetch_data(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{stu_id}");
        lst.Add($"{semester_id}");
        lst.Add(date.ToString());
        return lst;
    }

    // ========================================================================
}

/* EOF */
