sealed class UserQuery<T>
    where T : Account, new()
{
    // ========================================================================
    public List<T> get_account_by_id(int id, InfoAccountType? account_type = null) =>
        (account_type.HasValue)
            ? RecordQueryFromTable<T>.get_record_by_id(
                DatabaseConfigManager.get_account_table_config(account_type.Value).name,
                id
            )
            : SingleAccountQuery<T>.get_account_by_id(id);

    // ========================================================================
}

/* EOF */
