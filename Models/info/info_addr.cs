namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoAddr : DataObj
{
    // ========================================================================
    string content;

    // ========================================================================
    public InfoAddr()
    {
        this.content = "";
    }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.content = reader.GetString(pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
