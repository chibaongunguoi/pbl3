using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class TeacherProfileEditForm
{
    public int Id { get; set; } = 0;

    public string? Role { get; set; } = null; 

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public string? Name { get; set; } = null;

    [Required]
    public string? Gender { get; set; } = null;

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? Bday { get; set; } = null;

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string? Tel { get; set; } = null;

    public string? Description { get; set; } = null;

    public TeacherProfileEditForm()
    {
    }

    public TeacherProfileEditForm(string username)
    {
        Teacher? teacher = null;
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.teacher);
            q.Where(Field.teacher__username, username);

            List<Teacher> teachers = q.Select<Teacher>(conn);
            teacher = teachers.Count > 0 ? teachers[0] : null;
        });

        if (teacher != null)
        {
            Id = teacher.Id;
            Name = teacher.Name;
            Gender = teacher.Gender;
            Bday = teacher.Bday;
            Tel = teacher.Tel;
            Description = teacher.Description;
            Role = UserRole.Teacher;
        }
    }

    public void Reset(SqlConnection conn, string username)
    {
        Query q = new(Tbl.teacher);
        q.Output(Field.teacher__id);    
        q.Where(Field.teacher__username, username);
        q.Select(conn, reader => Id = QDataReader.GetInt(reader));
        Role = UserRole.Teacher;
    }

    public void Execute(SqlConnection conn, string username, ITempDataDictionary tempData, out Account? account)
    {
        Query q = new(Tbl.teacher);
        q.Where(Field.teacher__username, username);
        if (Bday is not null)
        {
            q.Set(Field.teacher__bday, Bday);
        }
        if (Name is not null)
        {
            q.SetNString(Field.teacher__name, Name);
        }

        if (Gender is not null)
        {
            q.Set(Field.teacher__gender, Gender);
        }
        if (Tel is not null)
        {
            q.Set(Field.teacher__tel, Tel);
        }
        q.SetNString(Field.teacher__description, Description ?? "");
        q.Update(conn);

        q = new(Tbl.teacher);
        q.Where(Field.teacher__username, username);
        List<Teacher> teachers = q.Select<Teacher>(conn);
        account = teachers.Count > 0 ? teachers[0] : null;

        if (account is not null)
        {
            tempData["SuccessMessage"] = "Cập nhật thông tin thành công";
        }
    }
}