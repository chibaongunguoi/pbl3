using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string fullname = "";
    public InfoGender gender = new InfoGender();
    public string addr = "";
    public string tel = "";
    public InfoDate bday = new InfoDate();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        fullname = DataReader.get_string(reader, pos++);
        gender = DataReader.get_enum<InfoGender>(reader, pos++);
        addr = DataReader.get_string(reader, pos++);
        tel = DataReader.get_string(reader, pos++);
        pos = bday.fetch_data(reader, pos);
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
                ((int)gender).ToString(),
                addr,
                tel,
                bday.ToString(),
            }
        );
    }

    // ------------------------------------------------------------------------
    public override Dictionary<string, string> to_dict()
    {
        Dictionary<string, string> dict = base.to_dict();
        dict["fullname"] = fullname;
        dict["gender"] = IoUtils.conv(gender);
        dict["addr"] = addr;
        dict["tel"] = tel;
        dict["bday"] = IoUtils.conv(bday);
        return dict;
    }

    // ========================================================================
}

/* EOF */
