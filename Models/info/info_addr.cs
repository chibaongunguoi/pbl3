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
    public override int fetch(SqlDataReader reader, int pos)
    {
        pos = base.fetch(reader, pos);
        this.content = reader.GetString(pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
