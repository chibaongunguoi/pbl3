using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class StuTopRightMenuViewComponent : ViewComponent
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
        }

        int unreadCount = 0;
        QDatabase.Exec(conn =>
        {
            Query countQuery = new(Tbl.notification);
            countQuery.Where(Field.notification__stu_id, studentId);
            countQuery.Where(Field.notification__is_read, 0);
            unreadCount = countQuery.Count(conn);
        });

        ViewBag.UnreadCount = unreadCount;
        return View("~/Views/Shared/Components/_StuTopRightMenu.cshtml");
    }
}
