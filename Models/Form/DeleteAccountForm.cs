using Microsoft.Data.SqlClient;

public class DeleteAccountForm
{
    public Dictionary<string, string> Messages { get; set; } = [];
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool ShowForm { get; set; } = true;

    public DeleteAccountForm() { }
    public DeleteAccountForm(int id, string role)
    {
        Id = id;
        Role = role;
        string table = role switch
        {
            UserRole.Student => Tbl.student,
            UserRole.Teacher => Tbl.teacher,
            UserRole.Admin => Tbl.admin,
            _ => string.Empty
        };

        int? tempId = null;
        QDatabase.Exec(
            conn =>
            {
                Query q = new(table);
                q.Where(Fld.id, id);
                q.Select(conn, reader => tempId = QDataReader.GetInt(reader));
            }
        );

        if (tempId is null)
        {
            Messages["Error"] = "Không tìm thấy tài khoản này";
            ShowForm = false;
        }
    }

    public void Execute(SqlConnection conn)
    {
        string table = Role switch
        {
            UserRole.Student => Tbl.student,
            UserRole.Teacher => Tbl.teacher,
            UserRole.Admin => Tbl.admin,
            _ => string.Empty
        };

        if (Role == UserRole.Student)
        {
            Query qq = new(Tbl.rating);
            qq.Where(Field.rating__stu_id, Id);
            qq.Delete(conn);

            qq = new(Tbl.request);
            qq.Where(Field.request__stu_id, Id);
            qq.Delete(conn);
        }
        else if (Role == UserRole.Teacher)
        {
            Query qq2 = new(Tbl.course);
            qq2.Join(Field.semester__course_id, Field.course__id);
            qq2.Where(Field.course__tch_id, Id);
            qq2.Output(Field.semester__id);

            Query qq = new(Tbl.rating);
            qq.WhereInQuery(Field.rating__semester_id, qq2.SelectQuery());
            qq.Delete(conn);

            qq = new(Tbl.request);
            qq.WhereInQuery(Field.request__semester_id, qq2.SelectQuery());
            qq.Delete(conn);

            qq = new(Tbl.semester);
            qq.WhereInQuery(Field.semester__id, qq2.SelectQuery());
            qq.Delete(conn);

            qq = new(Tbl.course);
            qq.Where(Field.course__tch_id, Id);
            qq.Delete(conn);
        }

        Query q = new(table);
        q.Where(Fld.id, Id);
        q.Delete(conn);

        Query q2 = new(table);
        q2.Where(Fld.id, Id);
        if (q2.Count(conn) == 0)
        {
            Messages["Success"] = "Xóa tài khoản thành công.";
            ShowForm = false;
        }
        else
        {
            Messages["Error"] = "Không thể xóa tài khoản.";
            ShowForm = true;
        }
    }
}