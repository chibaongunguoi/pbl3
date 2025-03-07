using Microsoft.Data.SqlClient;

class Schedule : DataObj
{
    // ========================================================================
    public int id;
    public InfoInterval interval = new InfoInterval();

    // ========================================================================
    public override int fetch_data(SqlDataReader reader, int pos = 0)
    {
        pos = base.fetch_data(reader, pos);
        id = DataReader.get_int(reader, pos++);
        pos = interval.fetch_data(reader, pos);
        return pos;
    }

    // ========================================================================
}

/* EOF */
