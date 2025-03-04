sealed class ContractQuery
{
    // ========================================================================
    public static DatabaseTableConfig get_table_config()
    {
        return DatabaseConfigManager.get_contract_table_config();
    }

    // ------------------------------------------------------------------------
    public static string get_table_name() => get_table_config().name;

    // ========================================================================
    public static List<Contract> get_all_contracts()
    {
        return RecordQueryFromTable<Contract>.get_all_records(get_table_name());
    }
    // ========================================================================
}

/* EOF */
