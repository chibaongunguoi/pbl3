using Microsoft.Data.SqlClient;

sealed class Semester : IdObj
{
    // ========================================================================
    public int course_id { get; set; }
    public InfoDate start_date { get; set; } = new InfoDate();
    public InfoDate finish_date { get; set; } = new InfoDate();
    public int capacity { get; set; }
    public int fee { get; set; }
    public string description { get; set; } = "";
    public InfoSemesterState state { get; set; } = new();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        course_id = DataReader.get_int(reader, pos++);
        pos = start_date.fetch_data(reader, pos);
        pos = finish_date.fetch_data(reader, pos);
        capacity = DataReader.get_int(reader, pos++);
        fee = DataReader.get_int(reader, pos++);
        description = DataReader.get_string(reader, pos++);
        state = DataReader.get_enum<InfoSemesterState>(reader, pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add($"{course_id}");
        lst.Add(start_date.ToString());
        lst.Add(finish_date.ToString());
        lst.Add($"{capacity}");
        lst.Add($"{fee}");
        lst.Add(description);
        lst.Add($"{(int)state}");
        return lst;
    }

    // ========================================================================
}

/* EOF */
