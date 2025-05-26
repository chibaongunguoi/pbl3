using System;
using Microsoft.Data.SqlClient;

public class Notification
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int IsRead { get; set; } = 0;
    public static void Add(SqlConnection conn, int studentId, string message)
    {
        if (!IdCounterQuery.increment(conn, Tbl.notification, out int id))
            return;
        Query q = new(Tbl.notification);
        q.Set(Field.notification__id, id);
        q.Set(Field.notification__stu_id, studentId);
        q.SetNString(Field.notification__message, message);
        q.Set(Field.notification__timestamp, DateTime.Now);
        q.Set(Field.notification__is_read, 0);
        q.Insert(conn);
    }
}
