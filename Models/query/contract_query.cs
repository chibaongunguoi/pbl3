using Microsoft.Data.SqlClient;

sealed class ContractQuery
{
    // ========================================================================
    public static void get_all_contracts(SqlConnection conn, Database.ReaderFunction f)
    {
        CommonQuery.get_all_records(conn, f, Table.contract);
    }

    // ------------------------------------------------------------------------
    public static void get_all_contracts_from_student(
        SqlConnection conn,
        Database.ReaderFunction f,
        int stu_id
    )
    {
        Query q = new(Table.contract);
        q.where_(Field.contract__stu_id, stu_id);
        q.select(conn, f);
    }

    // ------------------------------------------------------------------------
    public static void get_all_contracts_from_teacher(
        SqlConnection conn,
        Database.ReaderFunction f,
        int tch_id
    )
    {
        Query q = new(Table.contract);
        q.where_(Field.contract__tch_id, tch_id);
        q.select(conn, f);
    }

    // ========================================================================
}

/* EOF */
