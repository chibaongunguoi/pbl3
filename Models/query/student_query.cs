sealed class StudentQuery
{
    // ------------------------------------------------------------------------
    public List<Student> get_student_by_id(int id)
    {
        Query q = new(Table.student);
        q.where_(Field.student__id, id);
        return q.select<Student>();
    }

    // ========================================================================
}

/* EOF */
