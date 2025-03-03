sealed class StudentQuery
{
    // ========================================================================
    public static List<Student> get_all_students()
    {
        return MultiAccountQueryFromTable<Student>.get_all_accounts(
            ConfigUtils.get_table_name(InfoAccountType.STUDENT)
        );
    }

    // ========================================================================
}

/* EOF */
