namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

class InfoString : DataObj
{
    // ========================================================================
    public string content = "";

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        this.content = reader.GetString(pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public string get_content()
    {
        return this.content;
    }

    // ========================================================================
}

/* EOF */
