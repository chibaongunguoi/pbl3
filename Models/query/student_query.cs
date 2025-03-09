sealed class StudentQuery
{
    // ========================================================================
    public List<Student> get_student_by_id(int id)
    {
        return CommonQuery<Student>.get_record_by_id(id, Table.student);
    }

    // ========================================================================
}

/* EOF */
