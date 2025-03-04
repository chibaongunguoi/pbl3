class SubjectQuery
{
    // ========================================================================
    public static DatabaseTableConfig get_table_config()
    {
        return DatabaseConfigManager.get_subject_table_config();
    }

    // ------------------------------------------------------------------------
    public static string get_table_name() => get_table_config().name;

    // ========================================================================
    public static string field(string f) => "[" + get_table_name() + "].[" + f + "]";

    // ========================================================================
    public List<Subject> get_all_subjects()
    {
        return RecordQueryFromTable<Subject>.get_all_records(get_table_name());
    }

    // ========================================================================
}

/* EOF */
