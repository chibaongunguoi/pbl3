using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Route("TeacherManageAPI")]
[Authorize(Roles = UserRole.Teacher)]
public class TeacherManageAPI : BaseController
{
    [HttpGet("ManageCourse")]
    public IActionResult ManageCourse()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        List<ManageCourseCard> cards = [];
        Query q = ManageCourseCard.GetQueryCreator();
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Where(Field.teacher__username, username);
        int pos = 0;
        int current_table_index = 1;
        QDatabase.Exec(conn => q.Select(conn, reader => cards.Add(ManageCourseCard.GetCard(reader, ref pos, ref current_table_index))));
        return PartialView("List/_ManageCourseCardList", cards);
    }
}