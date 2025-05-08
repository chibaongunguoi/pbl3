using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;

static class AuthQuery
{
    public static void Login(SqlConnection conn, LoginForm form, ref Account? account, ref string table, ref string role)
    {
        foreach (string tableName in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
        {
            Query q = new(tableName);
            q.Where(Fld.username, form.Username);
            q.Where(Fld.password, form.Password);
            var queryResult = q.Select<Account>(conn);
            if (queryResult.Count > 0)
            {
                account = queryResult[0];
                table = tableName;
                role = tableName switch
                {
                    Tbl.student => UserRole.Student,
                    Tbl.teacher => UserRole.Teacher,
                    Tbl.admin => UserRole.Admin,
                    _ => string.Empty,
                };
                break;
            }
        }
    }

    public static void SignUp(SqlConnection conn, ModelStateDictionary modelState, StudentSignUpForm form, ref Account? account)
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

        if (!IdCounterQuery.increment(conn, Tbl.student, out int id))
        {
            modelState.AddModelError(string.Empty, "Đã đạt giới hạn số lượng học viên");
            return;
        }

        Student student = new()
        {
            Id = id,
            Username = form.Username,
            Password = form.Password,
            Name = form.Name,
            Gender = form.Gender,
            Bday = form.Bday ?? new(),
            Tel = form.Tel,
        };
        Query q1 = new(Tbl.student);
        q1.Insert(conn, string.Join(", ", student.ToList()));

        account = student;
    }
}