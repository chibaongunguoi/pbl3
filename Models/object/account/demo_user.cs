namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;

sealed class DemoUser : DataObj
{
    // ========================================================================
    private int id;
    private string name = "";

    // ========================================================================
    public DemoUser() { }

    // ------------------------------------------------------------------------
    public DemoUser(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.name = reader.GetString(pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override void print()
    {
        Console.WriteLine($"ID: {this.id}, Name: {this.name}");
    }

    // ========================================================================
}

/* EOF */
