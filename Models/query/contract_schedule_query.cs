using Microsoft.Data.SqlClient;

sealed class ContractScheduleQuery
{
    // ========================================================================
    public static List<string> get_contract_schedule_from_contract(
        SqlConnection conn,
        int contract_id
    )
    {
        Query q = new(Table.contract_schedule);
        q.where_(Field.contract_schedule__ctrct_id, contract_id);
        return q.select(conn);
    }

    // ========================================================================
    public static List<int> get_ctrct_schedules_from_student(SqlConnection conn, int stu_id)
    {
        List<int> result = new();
        void func(SqlDataReader reader)
        {
            var obj = DataReader.get_data_obj<ContractSchedule>(reader);
            result.Add(obj.sch_id);
        }
        Query q = new(Table.contract);
        q.join(Field.contract__id, Field.contract_schedule__ctrct_id);
        q.where_(Field.contract__stu_id, stu_id);
        q.select(conn, func);
        return result;
    }

    // ========================================================================
    public static List<int> get_ctrct_schedules_from_teacher(SqlConnection conn, int tch_id)
    {
        List<int> result = new();
        void func(SqlDataReader reader)
        {
            var obj = DataReader.get_data_obj<ContractSchedule>(reader);
            result.Add(obj.sch_id);
        }
        Query q = new(Table.contract_schedule);
        q.join(Field.contract__id, Field.contract_schedule__ctrct_id);
        q.where_(Field.contract__tch_id, tch_id);
        q.select(conn, func);
        return result;
    }
    // ========================================================================
}

/* EOF */
