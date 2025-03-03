namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoInterval : DataObj
{
    // ========================================================================
    public InfoTime start_time = new InfoTime();
    public InfoTime end_time = new InfoTime();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        pos = this.start_time.fetch_data_by_reader(reader, pos);
        pos = this.end_time.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ========================================================================
    public override string ToString()
    {
        return $"{start_time.ToString()} - {end_time.ToString()}";
    }

    // ========================================================================
}

/* EOF */
