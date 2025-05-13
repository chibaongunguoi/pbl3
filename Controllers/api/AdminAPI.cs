using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace REPO.Controllers;

[Route("AdminAPI")]
[Authorize(Roles = UserRole.Admin)]
public class AdminAPI : BaseController
{
    private Query GetStudentsQuery()
    {
        Query q = AdminMngStuCard.GetQuery();
        q.OrderBy(Field.student__id, desc: false);
        return q;
    }
    [HttpGet("GetStudents")]
    public IActionResult GetStudents(PaginationInfo paginationInfo)
    {
        Query q = GetStudentsQuery();
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        List<AdminMngStuCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminMngStuCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_AdminMngStuCardList", cards);
    }

    [HttpGet("GetStudents/Pagination")]
    public IActionResult GetStudentsPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetStudentsQuery().Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    [HttpGet("GetAdminEditStuProfileForm")]
    public IActionResult GetAdminEditStuProfileForm(int stuId)
    {
        AdminEditStuProfileForm form = new(stuId);
        Console.WriteLine($"GetAdminEditStuProfileForm: {form.Name}");
        return PartialView("Form/_AdminEditStuProfileForm", form);
    }    [HttpPost("SubmitAdminEditStuProfileForm")]
    public IActionResult SubmitAdminEditStuProfileForm(AdminEditStuProfileForm form, int stuId)
    {
        if (!ModelState.IsValid)
            return PartialView("Form/_AdminEditStuProfileForm", form);

        QDatabase.Exec(conn => form.Execute(conn, stuId));
        return PartialView("Form/_AdminEditStuProfileForm", form);
    }    private Query GetStudentCoursesQuery(int stuId)
    {
        Query q = AdminStuCorCard.GetQuery(stuId);
        q.OrderBy(Field.semester__status, [SemesterStatus.started, SemesterStatus.waiting, SemesterStatus.finished]);
        return q;
    }    [HttpGet("GetStudentCourses")]
    public IActionResult GetStudentCourses(int stuId)
    {
        Query q = GetStudentCoursesQuery(stuId);
        
        List<AdminStuCorCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = 1;
                q.Select(
                    conn,
                    reader => cards.Add(AdminStuCorCard.GetCard(reader, ref tableIdx))
                );

                // Get student name for popup title
                if (cards.Count > 0)
                {
                    ViewBag.StudentName = cards[0].StudentName;
                    ViewBag.StuId = cards[0].StuId;
                }
                else
                {
                    Query stuQuery = new(Tbl.student);
                    stuQuery.Where(Field.student__id, stuId);
                    stuQuery.Output(Field.student__name);
                    stuQuery.Output(Field.student__id);
                    stuQuery.Select(
                        conn,
                        reader => 
                        {
                            int pos = 0;
                            ViewBag.StudentName = QDataReader.GetString(reader, ref pos);
                            ViewBag.StuId = QDataReader.GetInt(reader, ref pos);
                        }
                    );
                }
            }
        );
        return PartialView("List/_AdminStuCorCardList", cards);
    }
    
    [HttpGet("GetStudentCourses/Pagination")]
    public IActionResult GetStudentCoursesPagination(int stuId, PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetStudentCoursesQuery(stuId).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
}