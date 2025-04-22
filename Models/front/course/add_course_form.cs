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

    public required string course_name { get; set; }
    public required string subject { get; set; }
    public required string grade { get; set; }
    public DateOnly? start_date { get; set; }
    public DateOnly? finish_date { get; set; }
    public int capacity { get; set; }
    public int fee { get; set; }
    public required string description { get; set; }

    public Log execute(SqlConnection conn, int tch_id)
    {
        Log log = new();
        int i_grade;
        if (!int.TryParse(grade, out i_grade))
        {
            log.errors[ErrorKey.grade_invalid] = "Khối lớp không hợp lệ";
        }
        Checking.check_start_finish_dates(ref log.errors, start_date, finish_date);

        if (!log.success)
            return log;

        // Start querying

        Query q = new(Tbl.teacher);
        int c = q.count(conn);
        if (c == 0)
        {
            log.errors[ErrorKey.tch_id_not_exist] = "Gia sư không tồn tại";
            return log;
        }

        Query sbj_query = new(Tbl.subject);
        sbj_query.Where(Field.subject__name, subject);
        sbj_query.Where(Field.subject__grade, i_grade);
        List<Subject> subjects = new();
        sbj_query.select(conn, reader => subjects.Add(DataReader.getDataObj<Subject>(reader)));

        if (subjects.Count == 0)
        {
            log.errors[ErrorKey.subject_invalid] = "Không tồn tại môn học";
            return log;
        }

        int sbj_id = subjects[0].id;
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
            id = course_id,
            tch_id = tch_id,
            sbj_id = sbj_id,
            name = course_name,
            status = CourseStatus.waiting,
        };

        Semester semester = new()
        {
            id = semester_id,
            course_id = course_id,
            start_date = start_date ?? new(),
            finish_date = finish_date ?? new(),
            capacity = capacity,
            fee = fee,
            description = description,
            status = SemesterStatus.waiting,
        };

        Query q1 = new(Tbl.course);
        q1.insert(conn, string.Join(", ", course.ToList()));
        q1 = new(Tbl.semester);
        q1.insert(conn, string.Join(", ", semester.ToList()));

        log.course_id = course_id;
        log.semester_id = semester_id;
        return log;
    }

    public Log execute(int tch_id)
    {
        Log log = new();
        QDatabase.exec(conn => log = execute(conn, tch_id));
        return log;
    }

    public void print_log()
    {
        Console.WriteLine("[LOG]");
        Console.WriteLine($"course_name: {course_name}");
        Console.WriteLine($"subject: {subject}");
        Console.WriteLine($"grade: {grade}");
        Console.WriteLine($"start_date: {start_date}");
        Console.WriteLine($"finish_date: {finish_date}");
        Console.WriteLine($"capacity: {capacity}");
        Console.WriteLine($"fee: {fee}");
        Console.WriteLine($"description: {description}");
        Console.WriteLine("[/LOG]");
    }
}

/* EOF */
