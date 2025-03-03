sealed class TeacherQuery
{
    // ========================================================================
    public static List<Teacher> get_all_teachers()
    {
        return MultiAccountQueryFromTable<Teacher>.get_all_accounts(
            ConfigUtils.get_table_name(InfoAccountType.TEACHER)
        );
    }

    // ========================================================================
}

/* EOF */
