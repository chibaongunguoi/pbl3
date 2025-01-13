namespace module_database;

using Microsoft.Data.SqlClient;

abstract class DatabaseInfo
{
    public virtual int fetch(SqlDataReader reader)
    {
        return 0;
    }

    public virtual void print() { }
}
