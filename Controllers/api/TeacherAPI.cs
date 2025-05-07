using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("teacherAPI")]
public class TeacherAPI : BaseController
{
    [HttpGet("getBriefTeacherCards")]
    public IActionResult getBriefTeacherCards(int currentPage)
    {
        List<BriefTeacherCard> cards = new();
        QDatabase.Exec(conn => cards = BriefTeacherPage.GetPage(conn, currentPage));
        return PartialView(PartialList.BriefTeacherCard, cards);
    }
}
