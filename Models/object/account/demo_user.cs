namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;
using module_info;
using DemoUserId = int;

// HACK: Đây là class dùng để thử nghiệm việc đọc các kiểu dữ liệu khác
// nhau từ cơ sở dữ liệu.
sealed class DemoUser : DataObj
{
    // ========================================================================
    public DemoUserId id;
    public string username = "";
    public string password = "";
    public string name = "";
    public InfoGender gender = new InfoGender();
    public string addr = "";
    public string tel = "";
    public InfoDate bday = new InfoDate();
    public InfoTime working_time = new InfoTime();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.username = reader.GetString(pos++);
        this.password = reader.GetString(pos++);
        this.name = reader.GetString(pos++);
        pos = this.gender.fetch_data_by_reader(reader, pos);
        this.addr = reader.GetString(pos++);
        this.tel = reader.GetString(pos++);
        pos = this.bday.fetch_data_by_reader(reader, pos);
        pos = this.working_time.fetch_data_by_reader(reader, pos);
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
                gender.ToString(),
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
