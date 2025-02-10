namespace module_object;

using Microsoft.Data.SqlClient;
using module_info;

class User : Account
{
    // ========================================================================
    protected string name;
    protected InfoEmail email;
    protected InfoTel tel;
    protected InfoAddr addr;

    // ========================================================================
    public User()
    {
        this.name = "";
        this.email = new InfoEmail();
        this.tel = new InfoTel();
        this.addr = new InfoAddr();
    }

    // ========================================================================
    public override int fetch_data_by_reader(SqlDataReader reader, int pos)
    {
        pos = base.fetch_data_by_reader(reader, pos);
        name = reader.GetString(pos++);
        pos = email.fetch_data_by_reader(reader, pos);
        pos = tel.fetch_data_by_reader(reader, pos);
        pos = addr.fetch_data_by_reader(reader, pos);
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
