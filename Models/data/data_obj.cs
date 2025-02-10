namespace module_data;

using Microsoft.Data.SqlClient;

abstract class DataObj
{
    // ========================================================================
    public virtual int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        return pos;
    }

    // ------------------------------------------------------------------------
    public virtual void print() { }

    // ------------------------------------------------------------------------
    public virtual bool is_valid()
    {
        return true;
    }

    // ========================================================================
}

/* EOF */
