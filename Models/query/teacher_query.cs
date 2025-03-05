sealed class TeacherQuery
{
    // ========================================================================
    public List<Teacher> get_teacher_by_id(int id)
    {
        Query q = new(Table.teacher);
        q.where_(Field.teacher__id, id);
        return q.select<Teacher>();
    }

    // ========================================================================
}

/* EOF */
