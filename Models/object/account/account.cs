namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;

class Account : DataObj
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
    public override string get_repr()
    {
        return $"ID: {this.id}, Username: {this.username}, Password: {this.password}";
    }

    // ========================================================================
}

/* EOF */
