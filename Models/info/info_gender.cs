namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

sealed class InfoGender : DataObj
{
    // ========================================================================
    public enum Option
    {
        FEMALE,
        MALE,
    }

    // ========================================================================
    public Option option;

    // ========================================================================
    public InfoGender(Option option = Option.FEMALE)
    {
        this.option = option;
    }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        this.option = (Option)reader.GetInt32(pos++);
        return pos;
    }

    // ========================================================================
    public override string ToString()
    {
        return ((int)this.option).ToString();
    }

    // ========================================================================
}

/* EOF */
