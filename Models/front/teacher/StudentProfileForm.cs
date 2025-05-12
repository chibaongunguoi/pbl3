using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class StudentProfileForm
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


    public StudentProfileForm()
    {
    }

    public StudentProfileForm(string username)
    {
        Student? account = null;
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.student);
            q.Where(Fld.username, username);

            List<Student> teachers = q.Select<Student>(conn);
            account = teachers.Count > 0 ? teachers[0] : null;
        });

        if (account != null)
        {
            Id = account.Id;
            Name = account.Name;
            Gender = account.Gender;
            Bday = account.Bday;
            Tel = account.Tel;
            Role = UserRole.Student;
        }
    }

    public void Reset(SqlConnection conn, string username)
    {
        Query q = new(Tbl.student);
        q.Output(Fld.id);    
        q.Where(Fld.username, username);
        q.Select(conn, reader => Id = QDataReader.GetInt(reader));
        Role = UserRole.Student;
    }

    public void Execute(SqlConnection conn, string username, ITempDataDictionary tempData, out Account? account)
    {
        string table = Tbl.student;
        Query q = new(table);
        q.Where(Fld.username, username);
        if (Bday is not null)
        {
            q.Set(Fld.bday, Bday);
        }
        if (Name is not null)
        {
            q.SetNString(Fld.name, Name);
        }

        if (Gender is not null)
        {
            q.Set(Fld.gender, Gender);
        }
        if (Tel is not null)
        {
            q.Set(Fld.tel, Tel);
        }
        q.Update(conn);

        q = new(table);
        q.Where(Fld.username, username);
        List<Account> accounts = q.Select<Account>(conn);
        account = accounts.Count > 0 ? accounts[0] : null;

        if (account is not null)
        {
            tempData["SuccessMessage"] = "Cập nhật thông tin thành công";
        }
    }
}