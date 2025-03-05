sealed class ContractQuery
{
    // ========================================================================
    public static List<Contract> get_all_contracts()
    {
        Query q = new(Table.contract);
        return q.select<Contract>();
    }
    // ========================================================================
}

/* EOF */
