using Microsoft.Data.SqlClient;

static class SemesterQuery
{
    public static int get_num_of_joined_requests(SqlConnection conn, int semester_id)
    {
        Query q_2 = new(Table.request);
        q_2.where_(Field.request__semester_id, semester_id);
        q_2.where_(Field.request__state, (int)InfoRequestState.JOINED);
        List<Request> requests = q_2.select<Request>(conn);
        return requests.Count;
    }
}

/* EOF */
