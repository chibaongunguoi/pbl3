namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoDate : DataObj
{
    // ========================================================================
    public InfoDate() { }

    // ========================================================================
    public override int fetch(SqlDataReader reader, int pos)
    {
        pos = base.fetch(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
