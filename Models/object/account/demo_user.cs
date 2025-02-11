namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;
using module_info;

sealed class DemoUser : DataObj
{
    // ========================================================================
    public int id;
    public string name = "";
    public InfoTime working_time = new InfoTime();

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        this.name = reader.GetString(pos++);
        pos = this.working_time.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override string get_repr()
    {
        return $"ID: {this.id}, Name: {this.name}, Working_time: {this.working_time.get_repr()}";
    }

    // ========================================================================
}

/* EOF */
