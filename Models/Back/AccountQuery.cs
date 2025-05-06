using Microsoft.Data.SqlClient;

class AccountQuery {
    public static User? GetUser(SqlConnection conn, string username)
    {
        User? result = null;
        foreach (string tableName in new List<string> { Tbl.student, Tbl.teacher }) {
            Query q = new(tableName);
            q.Where(Fld.username, username);
            var queryResult = q.Select<User>(conn);
            if (queryResult.Count > 0) {
                result = queryResult[0];
                break;
            }
        }
        return result;
    }
}