using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

public class EditCourseForm
{
    public int CourseId { get; set; }
    public int SemesterId { get; set; }

    [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? StartDate { get; set; }

    [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
    [DataType(DataType.Date)]
    public DateOnly? FinishDate { get; set; }

    [Required(ErrorMessage = "Số học viên tối đa không được để trống")]
    public int? Capacity { get; set; }

    [Required(ErrorMessage = "Học phí không được để trống")]
    public int? Fee { get; set; }

    [Required(ErrorMessage = "Mô tả khóa học không được để trống")]
    public string? Description { get; set; }

    public Dictionary<string, string> Messages { get; set; } = [];

    public EditCourseForm() { }

    public EditCourseForm(int courseId)
    {
        CourseId = courseId;
        
        // Get the latest semester for this course
        Query q = new(Tbl.semester);
        q.Where(Field.semester__course_id, courseId);
        q.OrderBy(Field.semester__start_date, desc: true);
        q.OutputTop(1);
        
        QDatabase.Exec(conn =>
        {
            q.Select(conn, reader =>
            {
                int pos = 0;
                SemesterId = QDataReader.GetInt(reader, ref pos);
                pos++; // Skip course_id
                StartDate = QDataReader.GetDateOnly(reader, ref pos);
                FinishDate = QDataReader.GetDateOnly(reader, ref pos);
                Capacity = QDataReader.GetInt(reader, ref pos);
                Fee = QDataReader.GetInt(reader, ref pos);
                Description = QDataReader.GetString(reader, ref pos);
                // Skip Status
            });
        });
    }

    public void Execute(SqlConnection conn, out Semester? semester)
    {
        semester = null;
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);


        if (FinishDate < StartDate)
        {
            Messages["Error"] = "Ngày kết thúc không được trước ngày bắt đầu";
            return;
        }

        if (Capacity <= 0)
        {
            Messages["Error"] = "Số học viên tối đa không hợp lệ";
            return;
        }

        if (Fee <= 0)
        {
            Messages["Error"] = "Học phí không hợp lệ";
            return;
        }

        Query q = new(Tbl.semester);
        q.Where(Field.semester__id, SemesterId);
        q.Where(Field.semester__course_id, CourseId);
        List<Semester> semesters = q.Select<Semester>(conn);
        
        if (semesters.Count == 0)
        {
            Messages["Error"] = "Học kỳ không tồn tại";
            return;
        }

        DateOnly startDate = semesters[0].StartDate;
        DateOnly finishDate = semesters[0].FinishDate;

        if (startDate != StartDate && StartDate < today)
        {
            Messages["Error"] = "Ngày bắt đầu không được trước ngày hiện tại";
            return;
        }

        if (finishDate != FinishDate && FinishDate < today)
        {
            Messages["Error"] = "Ngày kết thúc không được trước ngày hiện tại";
            return;
        }

        // Check that we're not reducing capacity below current registrations
        Query countQuery = new(Tbl.request);
        countQuery.Where(Field.request__semester_id, SemesterId);
        int currentParticipants = countQuery.Count(conn);

        if (currentParticipants > Capacity)
        {
            Messages["Error"] = $"Số học viên tối đa không thể ít hơn số học viên đã đăng ký ({currentParticipants})";
            return;
        }

        // Update semester information
        Query updateQuery = new(Tbl.semester);
        updateQuery.Set(Field.semester__start_date, StartDate);
        updateQuery.Set(Field.semester__finish_date, FinishDate);
        updateQuery.Set(Field.semester__capacity, Capacity);
        updateQuery.Set(Field.semester__fee, Fee);
        updateQuery.SetNString(Field.semester__description, Description ?? "");
        updateQuery.Where(Field.semester__id, SemesterId);
        updateQuery.Update(conn);

        // Get updated semester
        q = new(Tbl.semester);
        q.Where(Field.semester__id, SemesterId);
        semesters = q.Select<Semester>(conn);
        
        if (semesters.Count > 0)
        {
            semester = semesters[0];
            Messages["Success"] = "Thông tin khóa học đã được cập nhật thành công";
        }
    }
}
