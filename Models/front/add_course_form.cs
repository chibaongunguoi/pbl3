using Microsoft.Data.SqlClient;

public enum AddCourseFormError
{
    NONE,
    TCH_ID_NOT_EXIST,
    GRADE_MUST_BE_INT,
    START_DATE_MISSING,
    FINISH_DATE_MISSING,
    RUN_OUT_OF_ID,
}

public class AddCourseFormLog
{
    public List<AddCourseFormError> errors = new();
    public int? course_id = null;
    public int? semester_id = null;

    public void Add(AddCourseFormError error) => errors.Add(error);

    public bool success => errors.Count == 0;
}

public class AddCourseForm
{
    public required string course_name { get; set; }
    public required string subject { get; set; }
    public required string grade { get; set; }
    public DateOnly? start_date { get; set; }
    public DateOnly? finish_date { get; set; }
    public int capacity { get; set; }
    public int fee { get; set; }
    public required string description { get; set; }

    public AddCourseFormLog execute(SqlConnection conn, int tch_id)
    {
        AddCourseFormLog log = new();
        int i_grade;
        bool valid = true;
        if (!int.TryParse(grade, out i_grade))
        {
            log.Add(AddCourseFormError.GRADE_MUST_BE_INT);
            valid = false;
        }

        if (start_date is null)
        {
            log.Add(AddCourseFormError.START_DATE_MISSING);
            valid = false;
        }

        if (finish_date is null)
        {
            log.Add(AddCourseFormError.FINISH_DATE_MISSING);
            valid = false;
        }

        if (!valid)
            return log;

        // Start querying

        Query q = new(Table.teacher);
        int c = q.count(conn);
        if (c == 0)
        {
            log.Add(AddCourseFormError.TCH_ID_NOT_EXIST);
            return log;
        }

        Query sbj_query = new(Table.subject);
        sbj_query.where_(Field.subject__name, subject);
        sbj_query.where_(Field.subject__grade, i_grade);
        List<Subject> subjects = sbj_query.select<Subject>(conn);

        if (subjects.Count == 0)
        {
            log.Add(AddCourseFormError.RUN_OUT_OF_ID);
            return log;
        }

        int sbj_id = subjects[0].id;
        int course_id,
            semester_id;

        if (
            !IdCounterQuery.get_count(conn, Table.course, out course_id)
            || !IdCounterQuery.get_count(conn, Table.semester, out semester_id)
        )
        {
            log.Add(AddCourseFormError.RUN_OUT_OF_ID);
            return log;
        }
        IdCounterQuery.increment(conn, Table.course, out course_id);
        IdCounterQuery.increment(conn, Table.semester, out semester_id);
        Course course = new()
        {
            id = course_id,
            tch_id = tch_id,
            sbj_id = sbj_id,
            name = course_name,
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
            state = InfoSemesterState.WAITING,
        };

        q = new(Table.course);
        q.insert<Course>(conn, course);
        q = new(Table.semester);
        q.insert<Semester>(conn, semester);

        log.course_id = course_id;
        log.semester_id = semester_id;
        return log;
    }

    public AddCourseFormLog execute(int tch_id)
    {
        AddCourseFormLog log = new();
        Database.exec(conn => log = execute(conn, tch_id));
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
