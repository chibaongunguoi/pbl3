using Microsoft.Data.SqlClient;

class TeacherScheduleQuery
{
    // ========================================================================
    public enum State
    {
        none,
        exist_in_avai,
        exist_in_busy,
    }

    // ========================================================================
    public static List<string> get_all_teacher_schedules(SqlConnection conn)
    {
        return CommonQuery.get_all_records(conn, Table.teacher_schedule);
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
    public static State add_schedule(SqlConnection conn, TeacherSchedule schedule)
    {
        int tch_id = schedule.tch_id;
        int sch_id = schedule.sch_id;
        List<int> avai_schedule = new();
        List<int> busy_schedule = new();
        avai_schedule = TeacherScheduleQuery.get_avai_schedule(conn, tch_id);
        busy_schedule = ContractScheduleQuery.get_ctrct_schedules_from_teacher(conn, tch_id);

        if (avai_schedule.Contains(sch_id))
        {
            return State.exist_in_avai;
        }
        else if (busy_schedule.Contains(sch_id))
        {
            return State.exist_in_busy;
        }

        CommonQuery<TeacherSchedule>.insert_record(conn, schedule, Table.teacher_schedule);
        return State.none;
    }

    // ========================================================================
    public static State remove_schedule(SqlConnection conn, TeacherSchedule schedule)
    {
        Query q = new(Table.teacher_schedule);
        q.where_(Field.teacher_schedule__tch_id, schedule.tch_id);
        q.where_(Field.teacher_schedule__sch_id, schedule.sch_id);
        q.delete(conn);
        return State.none;
    }

    // ========================================================================
}

/* EOF */
