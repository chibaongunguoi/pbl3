using Microsoft.Data.SqlClient;

sealed class Rating : DataObj
{
    // ========================================================================
    public int stu_id { get; set; }
    public int course_id { get; set; }
    public InfoDate date { get; set; } = new InfoDate();
    public int stars { get; set; }
    public string description { get; set; } = "";

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        stu_id = DataReader.get_int(reader, pos++);
        course_id = DataReader.get_int(reader, pos++);
        pos = date.fetch_data(reader, pos);
        stars = DataReader.get_int(reader, pos++);
        description = DataReader.get_string(reader, pos++);
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
