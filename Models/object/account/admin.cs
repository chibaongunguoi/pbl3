namespace module_object;

using Microsoft.Data.SqlClient;

class Admin : Account
{
    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
