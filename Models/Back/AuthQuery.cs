using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;

static class AuthQuery
{
    public static void Login(SqlConnection conn, LoginForm form, ref Account? account, ref string table, ref string role)
    {

        Account? accountResult = null;
        string tableResult = "";
        string roleResult = "";

        foreach (string tableName in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
        {
            Query q = new(tableName);
            q.Where(Fld.username, form.Username);
            q.Where(Fld.password, form.Password);
            var queryResult = q.Select<Account>(conn);
            if (queryResult.Count > 0)
            {
                accountResult = queryResult[0];
                tableResult = tableName;
                break;
            }
        }

        switch (tableResult)
        {
            case Tbl.student:
                roleResult = UserRole.Student;
                break;
            case Tbl.teacher:
                roleResult = UserRole.Teacher;
                break;
            case Tbl.admin:
                roleResult = UserRole.Admin;
                break;
        }

        account = accountResult;
        table = tableResult;
        role = roleResult;
    }

    public static void SignUp(SqlConnection conn, ref ModelStateDictionary modelState, StudentSignUpForm form, ref Account? account)
    {
        if (string.IsNullOrEmpty(form.Name))
        {
            modelState.AddModelError(nameof(form.Name), "Họ và tên không được để trống");
        }

        if (string.IsNullOrEmpty(form.Username))
        {
            modelState.AddModelError(nameof(form.Username), "Tên đăng nhập không được để trống");
        }


        Query q = new(Tbl.student);
        q.Where(Fld.username, form.Username);
        int count = q.Count(conn);

        if (count > 0)
        {
            modelState.AddModelError(nameof(form.Username), "Tên đăng nhập đã tồn tại");
            return;
        }
    }
}