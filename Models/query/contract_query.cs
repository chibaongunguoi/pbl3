sealed class ContractQuery
{
    // ========================================================================
    public static void get_all_contracts(DatabaseConn.ReaderFunction f)
    {
        Query q = new(Table.contract);
        q.select(f);
    }

    // ------------------------------------------------------------------------
    public static void get_all_contracts_from_student(DatabaseConn.ReaderFunction f, int stu_id)
    {
        Query q = new(Table.contract);
        q.where_(Field.contract__stu_id, stu_id);
        q.select(f);
    }

    // ------------------------------------------------------------------------
    public static void get_all_contracts_from_teacher(DatabaseConn.ReaderFunction f, int tch_id)
    {
        Query q = new(Table.contract);
        q.where_(Field.contract__tch_id, tch_id);
        q.select(f);
    }

    // ========================================================================
}

/* EOF */
