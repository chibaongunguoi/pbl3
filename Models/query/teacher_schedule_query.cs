using Microsoft.Data.SqlClient;

class TeacherScheduleQuery
{
    // ========================================================================
    public static void get_all_teacher_schedules(
        DatabaseConn.ReaderFunction f,
        string? conn_string = null
    )
    {
        Query q = new(Table.teacher_schedule);
        q.select(f, conn_string);
    }

    // ------------------------------------------------------------------------
    public static List<int> get_avai_schedule(int tch_id, string? conn_string = null)
    {
        List<int> result = new();
        void func(SqlDataReader reader)
        {
            var obj = DataReader.get_data_obj<TeacherSchedule>(reader);
            result.Add(obj.sch_id);
        }
        Query q = new(Table.teacher_schedule);
        q.where_(Field.teacher_schedule__tch_id, tch_id);
        q.select(func, conn_string);
        return result;
    }

    // ========================================================================
}

/* EOF */
