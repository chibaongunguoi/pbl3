using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

public class StuNotificationViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        int studentId = 0;
        var user = HttpContext.User;
        string username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        if (!string.IsNullOrEmpty(username))
        {
            QDatabase.Exec(conn =>
            {
                Query q = new(Tbl.student);
                q.Where(Field.student__username, username);
                q.Select(conn, reader =>
                {
                    int pos = 0;
                    studentId = QDataReader.GetInt(reader, ref pos);
                });
            });
        }        List<Notification> notifications = [];
        QDatabase.Exec(conn =>
        {
            // Get the latest 10 notifications for display
            Query q = new(Tbl.notification);
            q.Where(Field.notification__stu_id, studentId);
            q.OrderBy(Field.notification__timestamp, desc: true);
            q.OutputTop(10);
            q.Select(conn, reader =>
            {
                int pos = 0;
                Notification n = new()
                {
                    Id = QDataReader.GetInt(reader, ref pos),
                    StudentId = QDataReader.GetInt(reader, ref pos),
                    Message = QDataReader.GetString(reader, ref pos),
                    CreatedAt = QDataReader.GetDateTime(reader, ref pos),
                    IsRead = QDataReader.GetInt(reader, ref pos)
                };
                notifications.Add(n);
            });
        });

        return View("~/Views/Shared/Components/_StuNotificationDropdown.cshtml", notifications);
    }
}
