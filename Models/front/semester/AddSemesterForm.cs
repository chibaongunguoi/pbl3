using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

public class AddSemesterForm
{
    public List<CourseOption> Courses { get; set; } = [];

    [Required(ErrorMessage = "Khóa học không được để trống")]
    public int? CourseId { get; set; } = null;

    [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? StartDate { get; set; } = null;

    [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? FinishDate { get; set; } = null;
    [Required(ErrorMessage = "Số lượng học viên không được để trống")]
    public int? Capacity { get; set; } = null;

    [Required(ErrorMessage = "Học phí không được để trống")]
    public int? Fee { get; set; } = null;
    public string? Description { get; set; } = null;

    public AddSemesterForm()
    {
    }

    public AddSemesterForm(string username)
    {
        QDatabase.Exec(
            delegate (SqlConnection conn)
            {
                Query q = CourseOption.get_query_creator();
                q.Join(Field.teacher__id, Field.course__tch_id);
                q.Join(Field.semester__course_id, Field.course__id);
                q.WhereQuery(Field.semester__id, SemesterQuery.GetLatestSemesterIdQuery("s"));
                q.Where(Field.semester__status, SemesterStatus.finished);
                q.Where(Field.teacher__username, username);
                q.Select(
                    conn,
                    reader => Courses.Add(QDataReader.GetDataObj<CourseOption>(reader))
                );
            }
        );
    }

    public void Execute(SqlConnection conn, ModelStateDictionary dict, out Semester? semester)
    {
        semester = null;

        if (!IdCounterQuery.get_count(conn, Tbl.semester, out int _))
        {
            dict.AddModelError(nameof(CourseId), "Đã đạt giới hạn số lượng kì học");
            return;
        }

        Query q = new(Tbl.course);
        q.Where(Field.course__id, CourseId);
        List<Course> courses = q.Select<Course>(conn);
        if (courses.Count == 0)
        {
            dict.AddModelError(nameof(CourseId), "Khóa học không tồn tại");
            return;
        }
        Course course = courses[0];
        IdCounterQuery.increment(conn, Tbl.semester, out int semester_id);

        semester = new()
        {
            Id = semester_id,
            CourseId = CourseId ?? 0,
            StartDate = StartDate ?? new(),
            FinishDate = FinishDate ?? new(),
            Capacity = Capacity ?? 0,
            Fee = Fee ?? 0,
            Description = Description ?? "",
            Status = SemesterStatus.waiting,
        };

        Query q_ins_semester = new(Tbl.semester);
        q_ins_semester.Insert(conn, string.Join(", ", semester.ToList()));

        Query q_update_course = new(Tbl.course);
        q_update_course.Set(Fld.status, CourseStatus.waiting);
        q_update_course.Where(Fld.id, CourseId ?? 0);
        q_update_course.Update(conn);
        course.Status = CourseStatus.waiting;
    }
}
