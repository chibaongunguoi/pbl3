namespace module_object;

using Microsoft.Data.SqlClient;
using module_data;

class Account : DataObj
{
    // ========================================================================
    protected int id;
    protected InfoUsername username;
    protected InfoPassword password;

    // ========================================================================
    public Account()
    {
        this.username = new InfoUsername();
        this.password = new InfoPassword();
    }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        this.id = reader.GetInt32(pos++);
        pos = this.username.fetch_data_by_reader(reader, pos);
        pos = this.password.fetch_data_by_reader(reader, pos);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override void print()
    {
        Console.WriteLine(
            $"ID: {this.id}, Username: {this.username.get_content()}, Password: {this.password.get_content()}"
        );
    }

    // ========================================================================
}

/* EOF */
