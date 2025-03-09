sealed class UserQuery<T>
    where T : Account, new()
{
    // ========================================================================
    public List<T> get_account_by_id(int id, Table? table = null)
    {
        return (table.HasValue)
            ? CommonQuery<T>.get_record_by_id(table.Value, id)
            : SingleAccountQuery<T>.get_account_by_id(id);
    }

    // ========================================================================
}

/* EOF */
