namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;

abstract class Account : DataObj
{
    // ========================================================================
    public int id;
    public string username = "";
    public string password = "";

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.username = reader.GetString(pos++);
        this.password = reader.GetString(pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return string.Join(",", new string[] { id.ToString(), username, password });
    }

    // ========================================================================
}

/* EOF */
