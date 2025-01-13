namespace module_object;

using Microsoft.Data.SqlClient;

sealed class Teacher : User
{
    // ========================================================================
    public Teacher() { }

    // ========================================================================
    public override int fetch(SqlDataReader reader, int pos)
    {
        pos = base.fetch(reader, pos);
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
