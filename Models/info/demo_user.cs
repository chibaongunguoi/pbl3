namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

class DemoUser : DataObj
{
    protected int m_id;
    protected string m_name = "";

    public DemoUser() { }

    public DemoUser(int id, string name)
    {
        m_id = id;
        m_name = name;
    }

    public override int fetch(SqlDataReader reader)
    {
        int pos = base.fetch(reader);
        m_id = reader.GetInt32(++pos);
        m_name = reader.GetString(++pos);
        return pos;
    }

    public override void print()
    {
        Console.WriteLine($"ID: {m_id}, Name: {m_name}");
    }
}
