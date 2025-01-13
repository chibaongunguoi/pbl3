namespace module_object;

using Microsoft.Data.SqlClient;

class Admin : Account
{
    // ========================================================================
    public Admin() { }

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
