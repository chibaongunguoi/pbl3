using Microsoft.Data.SqlClient;

// HACK: Đây là class dùng để thử nghiệm việc đọc các kiểu dữ liệu khác
// nhau từ cơ sở dữ liệu.
sealed class DemoUser : IdObj
{
    // ========================================================================
    public string username { get; set; } = "";
    public string password { get; set; } = "";
    public string name { get; set; } = "";
    public InfoGender gender { get; set; } = new();
    public string addr { get; set; } = "";
    public string tel { get; set; } = "";
    public InfoDate bday { get; set; } = new InfoDate();
    public InfoTime working_time = new InfoTime();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        username = DataReader.get_string(reader, pos++);
        password = DataReader.get_string(reader, pos++);
        name = DataReader.get_string(reader, pos++);
        gender = DataReader.get_enum<InfoGender>(reader, pos++);
        addr = DataReader.get_string(reader, pos++);
        tel = DataReader.get_string(reader, pos++);
        pos = bday.fetch_data(reader, pos);
        pos = working_time.fetch_data(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        string str = string.Join(
            ",",
            new string[]
            {
                id.ToString(),
                username,
                password,
                name,
                ((int)gender).ToString(),
                addr,
                tel,
                bday.ToString(),
                working_time.ToString(),
            }
        );

        return str;
    }

    // ========================================================================
}

/* EOF */
