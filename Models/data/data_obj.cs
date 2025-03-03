using Microsoft.Data.SqlClient;

abstract class DataObj
{
    // ========================================================================
    public virtual int fetch_data(SqlDataReader reader, int pos)
    {
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string ToString()
    {
        return "";
    }

    // ========================================================================
}

/* EOF */
