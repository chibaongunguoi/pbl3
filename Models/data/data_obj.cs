namespace module_data;

using Microsoft.Data.SqlClient;

abstract class DataObj
{
    public virtual int fetch(SqlDataReader reader)
    {
        return 0;
    }

    public virtual void print() { }
}
