using Microsoft.Data.SqlClient;

class CommonQuery
{
    // ========================================================================
}

class CommonQuery<T>
    where T : DataObj, new()
{
    // ------------------------------------------------------------------------
    public static T? GetRecordById(SqlConnection conn, string table, int id)
    {
        Query q = new(table);
        q.Where($"[{table}].[{Fld.id}]", id);
        var temp = q.Select<T>(conn);
        if (temp.Count > 0)
        {
            return temp[0];
        }

        return null;
    }

    // ========================================================================
}


/* EOF */
