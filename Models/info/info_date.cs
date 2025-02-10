namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoDate : DataObj
{
    // ========================================================================
    public InfoDate() { }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
