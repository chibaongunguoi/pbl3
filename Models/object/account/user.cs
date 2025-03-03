using Microsoft.Data.SqlClient;

abstract class User : Account
{
    // ========================================================================
    public string fullname = "";
    public InfoGender gender = new InfoGender();
    public string addr = "";
    public string tel = "";
    public InfoDate bday = new InfoDate();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.fullname = reader.GetString(pos++);
        pos = this.gender.fetch_data_by_reader(reader, pos);
        this.addr = reader.GetString(pos++);
        this.tel = reader.GetString(pos++);
        pos = this.bday.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return string.Join(
            ",",
            new string[]
            {
                base.ToString(),
                fullname,
                gender.ToString(),
                addr,
                tel,
                bday.ToString(),
            }
        );
    }

    // ========================================================================
}

/* EOF */
