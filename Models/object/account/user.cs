namespace module_object;

using Microsoft.Data.SqlClient;
using module_info;

class User : Account
{
    // ========================================================================
    public string name = "";
    public InfoGender gender = new InfoGender();
    public string addr = "";
    public string tel = "";
    public InfoDate bday = new InfoDate();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.name = reader.GetString(pos++);
        pos = this.gender.fetch_data_by_reader(reader, pos);
        this.addr = reader.GetString(pos++);
        this.tel = reader.GetString(pos++);
        pos = this.bday.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
