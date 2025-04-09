using Microsoft.Data.SqlClient;

sealed class Rating : DataObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int course_id { get; set; }
    public DateOnly date { get; set; } = new DateOnly();
    public int stars { get; set; }
    public string description { get; set; } = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, ref int pos)
    {
        stu_id = DataReader.get_int(reader, ref pos);
        course_id = DataReader.get_int(reader, ref pos);
        date = DataReader.get_date(reader, ref pos);
        stars = DataReader.get_int(reader, ref pos);
        description = DataReader.get_string(reader, ref pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{stu_id}");
        lst.Add($"{course_id}");
        lst.Add(date.ToString());
        lst.Add($"{stars}");
        lst.Add(description);
        return lst;
    }

    // ========================================================================
}

/* EOF */
