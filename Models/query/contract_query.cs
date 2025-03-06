sealed class ContractQuery
{
    // ========================================================================
    public static void get_all_contracts(DatabaseConn.ReaderFunction f, string? conn_string = null)
    {
        Query q = new(Table.contract);
        q.select(f, conn_string);
    }

    // ------------------------------------------------------------------------
    public static void get_all_contracts_from_student(
        DatabaseConn.ReaderFunction f,
        int stu_id,
        string? conn_string = null
    )
    {
        Query q = new(Table.contract);
        q.where_(Field.contract__stu_id, stu_id);
        q.select(f, conn_string);
    }

    // ------------------------------------------------------------------------
    public static void get_all_contracts_from_teacher(
        DatabaseConn.ReaderFunction f,
        int tch_id,
        string? conn_string = null
    )
    {
        Query q = new(Table.contract);
        q.where_(Field.contract__tch_id, tch_id);
        q.select(f, conn_string);
    }

    // ========================================================================
}

/* EOF */
