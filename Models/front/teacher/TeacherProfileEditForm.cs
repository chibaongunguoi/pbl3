using System.ComponentModel.DataAnnotations;

class TeacherProfileEditForm
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

    public TeacherProfileEditForm(string username)
    {
        Role = UserRole.Teacher;
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
        }
    }
}