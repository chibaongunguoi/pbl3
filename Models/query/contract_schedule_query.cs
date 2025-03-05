sealed class ContractScheduleQuery
{
    // ========================================================================
    public static List<ContractSchedule> get_all_contracts()
    {
        Query q = new(Table.contract_schedule);
        return q.select<ContractSchedule>();
    }
    // ========================================================================
}

/* EOF */
