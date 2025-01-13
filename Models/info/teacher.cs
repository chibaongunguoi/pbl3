namespace module_info;

using Microsoft.Data.SqlClient;

class Teacher : Account
{
    public Teacher() { }

    public override int fetch(SqlDataReader reader)
    {
        int pos = base.fetch(reader);
        return pos;
    }

    public override void print()
    {
        base.print();
    }
}
