using Microsoft.Data.SqlClient;

// HACK: Đây là class dùng để thử nghiệm việc đọc các kiểu dữ liệu khác
// nhau từ cơ sở dữ liệu.
sealed class DemoUser : DataObj
{
    // ========================================================================
    public int id;
    public string username = "";
    public string password = "";
    public string name = "";
    public InfoGender gender;
    public string addr = "";
    public string tel = "";
    public InfoDate bday = new InfoDate();
    public InfoTime working_time = new InfoTime();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        id = DataReader.get_int(reader, pos++);
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
