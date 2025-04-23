using Microsoft.Data.SqlClient;

public class AddSemesterFormLog
{
    public Dictionary<string, string> errors = new();
    public bool Success => errors.Count == 0;
    public int semester_id;
}

public class AddSemesterForm
{
    public required string CourseId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? FinishDate { get; set; }
    public int Capacity { get; set; }
    public int Fee { get; set; }
    public required string Description { get; set; }

    public AddSemesterFormLog execute(SqlConnection conn)
    {
        AddSemesterFormLog log = new();
        int i_course_id;
        if (!int.TryParse(CourseId, out i_course_id))
        {
            log.errors[ErrorKey.course_invalid] = "Khóa học không hợp lệ";
        }

        Checking.check_start_finish_dates(ref log.errors, StartDate, FinishDate);

        if (!log.Success)
        {
            return log;
        }

        // Start querying

        Query q = new(Tbl.teacher);
        int c = q.Count(conn);
        if (c == 0)
        {
            log.errors[ErrorKey.tch_id_not_exist] = "Gia sư không tồn tại";
            return log;
        }

        int semester_id;

        if (!IdCounterQuery.get_count(conn, Tbl.semester, out semester_id))
        {
            log.errors[ErrorKey.run_out_of_id] = "Đã đạt giới hạn số lượng kì học";
            return log;
        }
        q = new(Tbl.course);
        q.Where(Field.course__id, i_course_id);
        List<Course> courses = q.Select<Course>(conn);
        if (courses.Count == 0)
        {
            log.errors[ErrorKey.course_invalid] = "Khóa học không tồn tại";
            return log;
        }
        Course course = courses[0];

        // success
        IdCounterQuery.increment(conn, Tbl.semester, out semester_id);

        Semester semester = new()
        {
            Id = semester_id,
            CourseId = i_course_id,
            StartDate = StartDate ?? new(),
            FinishDate = FinishDate ?? new(),
            Capacity = Capacity,
            Fee = Fee,
            Description = Description,
            Status = SemesterStatus.waiting,
        };

        Query q_ins_semester = new(Tbl.semester);
        q_ins_semester.Insert(conn, string.Join(", ", semester.ToList()));

        Query q_update_course = new(Tbl.course);
        q_update_course.Set(Fld.status, CourseStatus.waiting);
        q_update_course.Where(Fld.id, i_course_id);
        q_update_course.Update(conn);
        course.Status = CourseStatus.waiting;

        log.semester_id = semester_id;
        return log;
    }

    public AddSemesterFormLog execute()
    {
        AddSemesterFormLog log = new();
        QDatabase.exec(conn => log = execute(conn));
        return log;
    }

    public void print_log()
    {
        Console.WriteLine("[LOG] AddSemesterForm");
        Console.WriteLine($"course_id: {CourseId}");
        Console.WriteLine($"start_date: {StartDate}");
        Console.WriteLine($"finish_date: {FinishDate}");
        Console.WriteLine($"capacity: {Capacity}");
        Console.WriteLine($"fee: {Fee}");
        Console.WriteLine($"description: {Description}");
        Console.WriteLine("[/LOG]");
    }
}
