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
    public override int fetch(SqlDataReader reader, int pos)
    {
        pos = base.fetch(reader, pos);
        name = reader.GetString(pos++);
        pos = email.fetch(reader, pos);
        pos = tel.fetch(reader, pos);
        pos = addr.fetch(reader, pos);
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
