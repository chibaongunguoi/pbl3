namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;

class Subject : DataObj
{
    // ========================================================================
    protected int id;
    protected string name;

    // ========================================================================
    public Subject()
    {
        this.name = "";
    }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.name = reader.GetString(pos++);
        return pos;
    }

    // ========================================================================
}

/* EOF */
