sealed class TeacherQuery
{
    // ========================================================================
    public List<Teacher> get_teacher_by_id(int id)
    {
        return CommonQuery<Teacher>.get_record_by_id(id, Table.teacher);
    }

    // ========================================================================
}

/* EOF */
