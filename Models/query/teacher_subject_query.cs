class TeacherSubjectQuery
{
    // ========================================================================
    public static DatabaseTableConfig get_table_config()
    {
        return DatabaseConfigManager.get_teacher_subject_table_config();
    }

    // ------------------------------------------------------------------------
    public static string get_table_name() => get_table_config().name;

    // ========================================================================
    public static string field(string f) => "[" + get_table_name() + "].[" + f + "]";

    // ========================================================================
}

/* EOF */
