using Microsoft.Data.SqlClient;

public class AddSemesterFormLog
{
    public bool success = true;
    public int semester_id;
}

public class AddSemesterForm
{
    public required string course_id { get; set; }
    public DateOnly? start_date { get; set; }
    public DateOnly? finish_date { get; set; }
    public int capacity { get; set; }
    public int fee { get; set; }
    public required string description { get; set; }

    public AddSemesterFormLog execute(SqlConnection conn)
    {
        AddSemesterFormLog log = new();
        int i_course_id;
        bool valid = true;
        if (!int.TryParse(course_id, out i_course_id))
        {
            valid = false;
        }

        if (start_date is null)
        {
            valid = false;
        }

        if (finish_date is null)
        {
            valid = false;
        }

        if (!valid)
        {
            log.success = false;
            return log;
        }

        // Start querying

        Query q = new(Table.teacher);
        int c = q.count(conn);
        if (c == 0)
        {
            log.success = false;
            return log;
        }

        int semester_id;

        if (!IdCounterQuery.get_count(conn, Table.semester, out semester_id))
        {
            log.success = false;
            return log;
        }
        q = new(Table.course);
        q.where_(Field.course__id, i_course_id);
        List<Course> courses = q.select<Course>(conn);
        if (courses.Count == 0)
        {
            log.success = false;
            return log;
        }
        Course course = courses[0];

        // success
        IdCounterQuery.increment(conn, Table.semester, out semester_id);

        Semester semester = new()
        {
            id = semester_id,
            course_id = i_course_id,
            start_date = start_date ?? new(),
            finish_date = finish_date ?? new(),
            capacity = capacity,
            fee = fee,
            description = description,
            state = SemesterState.waiting,
        };

        Query q_ins_semester = new(Table.semester);
        q_ins_semester.insert<Semester>(conn, semester);

        Query q_update_course = new(Table.course);
        q_update_course.set_(Field.course__state, CourseState.waiting);
        q_update_course.where_(Field.course__id, i_course_id);
        q_update_course.update(conn);
        course.state = CourseState.waiting;

        log.semester_id = semester_id;
        return log;
    }

    public AddSemesterFormLog execute()
    {
        AddSemesterFormLog log = new();
        Database.exec(conn => log = execute(conn));
        return log;
    }

    public void print_log()
    {
        Console.WriteLine("[LOG] AddSemesterForm");
        Console.WriteLine($"course_id: {course_id}");
        Console.WriteLine($"start_date: {start_date}");
        Console.WriteLine($"finish_date: {finish_date}");
        Console.WriteLine($"capacity: {capacity}");
        Console.WriteLine($"fee: {fee}");
        Console.WriteLine($"description: {description}");
        Console.WriteLine("[/LOG]");
    }
}
