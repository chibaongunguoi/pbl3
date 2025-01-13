namespace module_info;

using Microsoft.Data.SqlClient;
using module_data;

class Account : DataObj
{
    protected int id;
    protected string username = "";
    protected string password = "";

    public Account() { }

    public Account(int id, string username, string password)
    {
        this.id = id;
        this.username = username;
        this.password = password;
    }

    public override int fetch(SqlDataReader reader)
    {
        int pos = base.fetch(reader);
        id = reader.GetInt32(pos++);
        username = reader.GetString(pos++);
        password = reader.GetString(pos++);
        return pos;
    }

    public override void print()
    {
        Console.WriteLine($"ID: {id}, Username: {username}, Password: {password}");
    }
}
