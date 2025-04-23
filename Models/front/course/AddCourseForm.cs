using Microsoft.Data.SqlClient;

public class AddCourseForm
{
    public class Log
    {
        public Dictionary<string, string> errors = new();
        public int? course_id = null;
        public int? semester_id = null;

        public bool success => errors.Count == 0;
    }

    public required string CourseName { get; set; }
    public required string Subject { get; set; }
    public required string Grade { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? FinishDate { get; set; }
    public int Capacity { get; set; }
    public int Fee { get; set; }
    public required string Description { get; set; }

    public Log execute(SqlConnection conn, int tch_id)
    {
        Log log = new();
        int i_grade;
        if (!int.TryParse(Grade, out i_grade))
        {
            log.errors[ErrorKey.grade_invalid] = "Khối lớp không hợp lệ";
        }
        Checking.check_start_finish_dates(ref log.errors, StartDate, FinishDate);

        if (!log.success)
            return log;

        // Start querying

        Query q = new(Tbl.teacher);
        int c = q.Count(conn);
        if (c == 0)
        {
            log.errors[ErrorKey.tch_id_not_exist] = "Gia sư không tồn tại";
            return log;
        }

        Query sbj_query = new(Tbl.subject);
        sbj_query.Where(Field.subject__name, Subject);
        sbj_query.Where(Field.subject__grade, i_grade);
        List<Subject> subjects = new();
        sbj_query.Select(conn, reader => subjects.Add(DataReader.getDataObj<Subject>(reader)));

        if (subjects.Count == 0)
        {
            log.errors[ErrorKey.subject_invalid] = "Không tồn tại môn học";
            return log;
        }

        int sbj_id = subjects[0].Id;
        int course_id,
            semester_id;

        if (
            !IdCounterQuery.get_count(conn, Tbl.course, out course_id)
            || !IdCounterQuery.get_count(conn, Tbl.semester, out semester_id)
        )
        {
            log.errors[ErrorKey.run_out_of_id] =
                "Đã đạt đến giới hạn số lượng khóa học hoặc kì học";
            return log;
        }
        IdCounterQuery.increment(conn, Tbl.course, out course_id);
        IdCounterQuery.increment(conn, Tbl.semester, out semester_id);
        Course course = new()
        {
            Id = course_id,
            TchId = tch_id,
            SbjId = sbj_id,
            Name = CourseName,
            Status = CourseStatus.waiting,
        };

        Semester semester = new()
        {
            Id = semester_id,
            CourseId = course_id,
            StartDate = StartDate ?? new(),
            FinishDate = FinishDate ?? new(),
            Capacity = Capacity,
            Fee = Fee,
            Description = Description,
            Status = SemesterStatus.waiting,
        };

        Query q1 = new(Tbl.course);
        q1.Insert(conn, string.Join(", ", course.ToList()));
        q1 = new(Tbl.semester);
        q1.Insert(conn, string.Join(", ", semester.ToList()));

        log.course_id = course_id;
        log.semester_id = semester_id;
        return log;
    }

    public Log execute(int tch_id)
    {
        Log log = new();
        QDatabase.Exec(conn => log = execute(conn, tch_id));
        return log;
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
