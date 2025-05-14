using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class TeacherEditForm
{
    public Dictionary<string, string> Message { get; } = new();

    public int TchId { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string? Username { get; set; } = null;

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public string? Name { get; set; } = null; [Required(ErrorMessage = "Giới tính không được để trống")]
    public string? MGender { get; set; } = Gender.male;

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? Bday { get; set; } = null;

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string? Tel { get; set; } = null;

    public string? Description { get; set; } = "";

    public string? NewPassword { get; set; } = null;

    public TeacherEditForm() { }
    public TeacherEditForm(int tchId)
    {
        TchId = tchId;

        Query q = new(Tbl.teacher);
        q.Output(Field.teacher__username);
        q.Output(Field.teacher__name);
        q.Output(Field.teacher__gender);
        q.Output(Field.teacher__bday);
        q.Output(Field.teacher__tel);
        q.Output(Field.teacher__thumbnail);
        q.Output(Field.teacher__description);
        q.Where(Field.teacher__id, tchId);

        QDatabase.Exec(conn => q.Select(conn, reader =>
        {
            int pos = 0;
            Username = QDataReader.GetString(reader, ref pos);
            Name = QDataReader.GetString(reader, ref pos);
            MGender = QDataReader.GetString(reader, ref pos);
            Bday = QDataReader.GetDateOnly(reader, ref pos);
            Tel = QDataReader.GetString(reader, ref pos);
            string thumbnail = QDataReader.GetString(reader, ref pos); // Skip thumbnail
            Description = QDataReader.GetString(reader, ref pos);
        }));
    }
    public void Execute(SqlConnection conn)
    {
        if (Username is null || Name is null || MGender is null || Bday is null || Tel is null)
        {
            Message.Add("Error", "Thông tin không được để trống");
            return;
        }

        string oldUsername = string.Empty;
        Query q = new(Tbl.teacher);
        q.Output(Field.teacher__username);
        q.Where(Field.teacher__id, TchId);
        q.Select(conn, reader =>
        {
            oldUsername = QDataReader.GetString(reader);
        });

        // Only check for username conflicts if the username has changed
        if (oldUsername != Username)
        {
            foreach (string table in new List<string> { Tbl.student, Tbl.teacher, Tbl.admin })
            {
                q = new(table);
                q.Where(Fld.username, Username);
                if (q.Count(conn) > 0)
                {
                    Message.Add("Error", "Tên đăng nhập đã tồn tại");
                    return;
                }
            }
        }        // Update teacher information using Query directly rather than creating a Teacher object
        Query query = new(Tbl.teacher);
        query.Set(Field.teacher__username, Username);
        query.SetNString(Field.teacher__name, Name);
        query.Set(Field.teacher__gender, MGender);
        query.Set(Field.teacher__bday, Bday);
        query.Set(Field.teacher__tel, Tel);
        query.SetNString(Field.teacher__description, Description ?? "");
        query.Where(Field.teacher__id, TchId);
        query.Update(conn);
        // Update password if provided
        if (!string.IsNullOrEmpty(NewPassword))
        {
            query = new(Tbl.teacher);
            query.Set(Field.teacher__password, NewPassword);
            query.Where(Field.teacher__id, TchId);
            query.Update(conn);
        }

        Message.Add("Success", "Cập nhật thông tin giảng viên thành công");
    }
}
