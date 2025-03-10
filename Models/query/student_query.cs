using Microsoft.Data.SqlClient;

sealed class StudentQuery
{
    // ========================================================================
    public List<Student> get_student_by_id(SqlConnection conn, int id)
    {
        return CommonQuery<Student>.get_record_by_id(conn, id, Table.student);
    }

    // ========================================================================
}

/* EOF */
