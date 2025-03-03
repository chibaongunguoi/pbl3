sealed class UserQuery
{
    // ========================================================================
    public string get_user_fullname(int id)
    {
        SingleAccountQueryResult<User> query_handler = SingleAccountQuery<User>.get_account_by_id(
            id
        );
        if (query_handler.result.Count == 0)
        {
            return string.Empty;
        }

        return query_handler.result[0].fullname;
    }
    // ========================================================================
}

/* EOF */
