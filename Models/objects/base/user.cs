using Microsoft.Data.SqlClient;

class User : Account
{
    // ========================================================================
    public string name = "";
    public InfoGender gender = new InfoGender();
    public InfoDate bday = new InfoDate();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        name = DataReader.get_string(reader, pos++);
        gender = DataReader.get_enum<InfoGender>(reader, pos++);
        pos = bday.fetch_data(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override List<string> ToListString()
    {
        var lst = base.ToListString();
        lst.Add(name);
        lst.Add($"{(int)gender}");
        lst.Add(bday.ToString());
        return lst;
    }

    // ========================================================================
}

/* EOF */
