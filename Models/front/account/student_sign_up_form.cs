using Microsoft.Data.SqlClient;

public class StudentSignUpForm
{
    public required string name { get; set; }
    public required string gender { get; set; }
    public DateOnly? bday { get; set; }
    public required string username { get; set; }
    public required string password { get; set; }
    public required string password_confirm { get; set; }

    public class Log
    {
        public int? stu_id;
        public List<string> errors = new();
        public bool success => errors.Count == 0;
    }

    public Log execute(SqlConnection conn)
    {
        Log log = new Log();

        bool success = true;

        if (bday == null)
        {
            log.errors.Add(ErrorStr.bday_empty);
        }

        if (string.IsNullOrEmpty(name))
        {
            log.errors.Add(ErrorStr.name_empty);
        }

        if (string.IsNullOrEmpty(username))
        {
            log.errors.Add(ErrorStr.username_empty);
        }

        if (password != password_confirm)
        {
            log.errors.Add(ErrorStr.password_mismatch);
        }

        if (!success)
        {
            return log;
        }

        Query q = new(Table.student);
        q.where_(Field.student__username, username);
        int count = q.count(conn);

        if (count > 0)
        {
            log.errors.Add(ErrorStr.username_taken);
            return log;
        }

        int id = 0;
        if (!IdCounterQuery.increment(conn, Table.student, out id))
        {
            log.errors.Add(ErrorStr.run_out_of_id);
            return log;
        }

        Student student = new()
        {
            id = id,
            username = username,
            password = password,
            name = name,
            gender = gender,
            bday = bday ?? new(),
        };
        q = new Query(Table.student);
        q.insert<Student>(conn, student);

        log.stu_id = id;
        return log;
    }

    public Log execute()
    {
        Log log = new();
        Database.exec(conn => log = execute(conn));
        return log;
    }
}
