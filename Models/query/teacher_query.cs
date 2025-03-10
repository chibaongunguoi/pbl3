using Microsoft.Data.SqlClient;

sealed class TeacherQuery
{
    // ========================================================================
    public List<Teacher> get_teacher_by_id(SqlConnection conn, int id)
    {
        return CommonQuery<Teacher>.get_record_by_id(conn, id, Table.teacher);
    }

    // ========================================================================
}

/* EOF */
