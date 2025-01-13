namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoGender : DataObj
{
    // ========================================================================
    enum Option
    {
        FEMALE,
        MALE,
    }

    // ========================================================================
    private Option option;

    // ========================================================================
    public InfoGender() { }

    // ========================================================================
    public override int fetch(SqlDataReader reader, int pos)
    {
        this.option = (Option)reader.GetInt32(pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
