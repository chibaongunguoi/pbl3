using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace REPO.Controllers;

[Route("teacherAPI")]
public class TeacherAPI : BaseController
{
    [HttpGet("getBriefTeacherCards")]
    public IActionResult getBriefTeacherCards(int currentPage)
    {
        List<BriefTeacherCard> cards = new();
        QDatabase.exec(conn => cards = BriefTeacherPage.get_page(conn, currentPage));
        return PartialView(PartialList.BriefTeacherCard, cards);
    }
}
