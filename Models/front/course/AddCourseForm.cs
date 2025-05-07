using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.ComponentModel.DataAnnotations;

public class AddCourseForm
{
    [Required(ErrorMessage = "Tên khóa học không được để trống")]
    public string? CourseName { get; set; } = null;

    [Required(ErrorMessage = "Tên môn học không được để trống")]
    public string? Subject { get; set; } = null;

    [Required(ErrorMessage = "Khối lớp không được để trống")]
    public int? Grade { get; set; } = null;

    [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? StartDate { get; set; }

    [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? FinishDate { get; set; }

    [Required(ErrorMessage = "Số lượng học viên không được để trống")]
    public int? Capacity { get; set; }

    [Required(ErrorMessage = "Học phí không được để trống")]
    public int? Fee { get; set; }
    public string? Description { get; set; }

    public void Execute(SqlConnection conn, ModelStateDictionary dict, string username, out Course? course, out Semester? semester)
    {
        course = null;
        semester = null;
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        Query sbj_query = new(Tbl.subject);
        sbj_query.Where(Field.subject__name, Subject ?? "");
        sbj_query.Where(Field.subject__grade, Grade);

        List<Subject> subjects = [];
        sbj_query.Select(conn, reader => subjects.Add(QDataReader.GetDataObj<Subject>(reader)));
        if (subjects.Count == 0)
        {
            dict.AddModelError(nameof(Subject), "Môn học không tồn tại");
            return;
        }

        List<Teacher> teachers = [];
        Query tch_query = new(Tbl.teacher);
        tch_query.Where(Field.teacher__username, username);
        tch_query.Select(conn, reader => teachers.Add(QDataReader.GetDataObj<Teacher>(reader)));
        if (teachers.Count == 0)
            return;

        int tch_id = teachers[0].Id;
        int sbj_id = subjects[0].Id;

        if (
            !IdCounterQuery.get_count(conn, Tbl.course, out int course_id)
            || !IdCounterQuery.get_count(conn, Tbl.semester, out int semester_id)
        )
        {
            dict.AddModelError(nameof(CourseName), "Không thể tạo khóa học mới");
            return;
        }

        IdCounterQuery.increment(conn, Tbl.course, out course_id);
        IdCounterQuery.increment(conn, Tbl.semester, out semester_id);
        course = new()
        {
            Id = course_id,
            TchId = tch_id,
            SbjId = sbj_id,
            Name = CourseName ?? "",
            Status = CourseStatus.waiting,
        };

        semester = new()
        {
            Id = semester_id,
            CourseId = course_id,
            StartDate = StartDate ?? new(),
            FinishDate = FinishDate ?? new(),
            Capacity = Capacity ?? 0,
            Fee = Fee ?? 0,
            Description = Description ?? "",
            Status = SemesterStatus.waiting,
        };

        Query q1 = new(Tbl.course);
        q1.Insert(conn, course);
        q1 = new(Tbl.semester);
        q1.Insert(conn, semester);
    }


    public void print_log()
    {
        Console.WriteLine("[LOG]");
        Console.WriteLine($"course_name: {CourseName}");
        Console.WriteLine($"subject: {Subject}");
        Console.WriteLine($"grade: {Grade}");
        Console.WriteLine($"start_date: {StartDate}");
        Console.WriteLine($"finish_date: {FinishDate}");
        Console.WriteLine($"capacity: {Capacity}");
        Console.WriteLine($"fee: {Fee}");
        Console.WriteLine($"description: {Description}");
        Console.WriteLine("[/LOG]");
    }
}

/* EOF */
