using Microsoft.Data.SqlClient;

class TeacherScheduleQuery
{
    // ========================================================================
    public static void get_all_teacher_schedules(SqlConnection conn, Database.ReaderFunction f)
    {
        CommonQuery.get_all_records(conn, f, Table.teacher_schedule);
    }

    // ------------------------------------------------------------------------
    public static List<int> get_avai_schedule(SqlConnection conn, int tch_id)
    {
        List<int> result = new();
        void func(SqlDataReader reader)
        {
            var obj = DataReader.get_data_obj<TeacherSchedule>(reader);
            result.Add(obj.sch_id);
        }
        Query q = new(Table.teacher_schedule);
        q.where_(Field.teacher_schedule__tch_id, tch_id);
        q.select(conn, func);
        return result;
    }

    // ========================================================================
}

/* EOF */
