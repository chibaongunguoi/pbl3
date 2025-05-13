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
    }

    [HttpGet("SubmitAdminEditStuProfileForm")]
    public IActionResult SubmitAdminEditStuProfileForm(AdminEditStuProfileForm form, int stuId)
    {
        if (!ModelState.IsValid)
            return PartialView("Form/_AdminEditStuProfileForm", form);

        QDatabase.Exec(conn => form.Execute(conn, stuId));
        return PartialView("Form/_AdminEditStuProfileForm", form);
    }
}