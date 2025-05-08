using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

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

    [HttpGet("ManageSemester")]
    public IActionResult ManageSemester(int courseId)
    {
        List<ManageSemesterCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = 1;
                Query q = ManageSemesterCard.get_query_creator();
                q.Where(Field.semester__course_id, courseId);
                q.OrderBy(Field.semester__start_date, desc: true);
                q.Select(
                    conn,
                    reader => cards.Add(ManageSemesterCard.get_card(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_ManageSemesterCardList", cards);
    }


    [HttpGet("ManageRequest")]
    public IActionResult ManageRequest()
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        Query q = ManageRequestCard.GetQueryCreator();
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Where(Field.teacher__username, username);
        List<ManageRequestCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = 1;
                q.Select(
                    conn,
                    reader => cards.Add(ManageRequestCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_ManageRequestCardList", cards);
    }

    [HttpGet("AcceptRequest")]
    public IActionResult AcceptRequest(int stuId, int semesterId)
    {
        Query q = new(Tbl.request);
        q.Set(Field.request__status, RequestStatus.joined);
        q.Where(Field.request__semester_id, semesterId);
        q.Where(Field.request__stu_id, stuId);
        QDatabase.Exec(q.Update);
        return RedirectToAction(nameof(ManageRequest), nameof(TeacherManageAPI));
    }

    [HttpGet("RejectRequest")]
    public IActionResult RejectRequest(int stuId, int semesterId)
    {
        Query q = new(Tbl.request);
        q.Where(Field.request__semester_id, semesterId);
        q.Where(Field.request__stu_id, stuId);
        QDatabase.Exec(q.Delete);
        return RedirectToAction(nameof(ManageRequest), nameof(TeacherManageAPI));
    }

    [HttpGet("ManageStuSemCard")]
    public IActionResult ManageStuSemCard_(int semesterId)
    {
        Query q = ManageStuSemCard.GetQueryCreator();
        List<ManageStuSemCard> cards = [];
        int tableIdx = 1;
        QDatabase.Exec(
            conn =>
            {
                q.Where(Field.request__semester_id, semesterId);
                q.OrderBy(Field.request__timestamp, desc: true);
                q.Select(
                    conn,
                    reader => cards.Add(ManageStuSemCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_ManageStuSemCardList", cards);
    }

    private static Query GetManageRatingQueryCreator()
    {
        Query q = ManageRatingCard.GetQueryCreator();
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.OrderBy(Field.rating__timestamp, desc: true);
        return q;
    }

    [HttpGet("ManageRating")]
    public IActionResult ManageRating(PaginationInfo paginationInfo)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        List<ManageRatingCard> cards = [];
        int tableIdx = 1;

        Query q = GetManageRatingQueryCreator();
        q.Where(Field.teacher__username, username);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        QDatabase.Exec(
            conn =>
            q.Select(
                conn,
                reader => cards.Add(ManageRatingCard.GetCard(reader, ref tableIdx))
           )
        );
        return PartialView("List/_ManageRatingCardList", cards);
    }

    [HttpGet("ManageRating/Pagination")]
    public IActionResult ManageRatingPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        Query q = GetManageRatingQueryCreator();
        q.Where(Field.teacher__username, username);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
}