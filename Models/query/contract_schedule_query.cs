using Microsoft.Data.SqlClient;

sealed class ContractScheduleQuery
{
    // ========================================================================
    public static void get_contract_schedule_from_contract(
        DatabaseConn.ReaderFunction f,
        int contract_id
    )
    {
        Query q = new(Table.contract_schedule);
        q.where_(Field.contract_schedule__ctrct_id, contract_id);
        q.select(f);
    }

    // ========================================================================
    public static List<int> get_ctrct_schedules_from_student(int stu_id)
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
        q.select(func);
        return result;
    }

    // ========================================================================
}

/* EOF */
