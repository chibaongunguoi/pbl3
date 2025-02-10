namespace module_object;

using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public Teacher() { }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override void print()
    {
        base.print();
    }

    // ========================================================================
}

/* EOF */
