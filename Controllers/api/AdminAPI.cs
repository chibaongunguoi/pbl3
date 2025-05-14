using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace REPO.Controllers;

[Route("AdminAPI")]
[Authorize(Roles = UserRole.Admin)]
public class AdminAPI : BaseController
{    
    [HttpGet("GetTeacherAddForm")]
    public IActionResult GetTeacherAddForm()
    {
        TeacherAddForm form = new();
        return PartialView("Form/_TeacherAddForm", form);
    }
    
    [HttpPost("AddTeacher")]
    public IActionResult AddTeacher(TeacherAddForm form)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("Form/_TeacherAddForm", form);
        }

        Teacher? teacher = null;
        QDatabase.Exec(conn => form.Execute(conn, ModelState, TempData, out teacher));
        return PartialView("Form/_TeacherAddForm", form);
    }
      [HttpGet("GetTeacherEditForm")]
    public IActionResult GetTeacherEditForm(int tchId)
    {
        TeacherEditForm form = new(tchId);
        Console.WriteLine($"GetTeacherEditForm: {form.Name}");
        return PartialView("Form/_TeacherEditForm", form);
    }
    
    [HttpPost("EditTeacher")]
    public IActionResult EditTeacher(TeacherEditForm form)
    {
        if (!ModelState.IsValid)
            return PartialView("Form/_TeacherEditForm", form);

        QDatabase.Exec(form.Execute);
        return PartialView("Form/_TeacherEditForm", form);
    }
    
    private Query GetTeacherCoursesQuery(int teacherId)
    {
        Query q = AdminTeacherCourseCard.GetQuery(teacherId);
        q.OrderBy(Field.course__id, desc: false);
        return q;
    }
    
    [HttpGet("GetTeacherCourses")]
    public IActionResult GetTeacherCourses(int tchId, PaginationInfo paginationInfo)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 5 };
        }

        Query q = GetTeacherCoursesQuery(tchId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        
        List<AdminTeacherCourseCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminTeacherCourseCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_AdminTeacherCoursesCardList", cards);
    }
    
    [HttpGet("GetTeacherCourses/Pagination")]
    public IActionResult GetTeacherCoursesPagination(int tchId, PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetTeacherCoursesQuery(tchId).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
    
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
    
    [HttpPost("SubmitAdminEditStuProfileForm")]
    public IActionResult SubmitAdminEditStuProfileForm(AdminEditStuProfileForm form, int stuId)
    {
        if (!ModelState.IsValid)
            return PartialView("Form/_AdminEditStuProfileForm", form);

        QDatabase.Exec(conn => form.Execute(conn, stuId));
        return PartialView("Form/_AdminEditStuProfileForm", form);
    }
    
    private Query GetStudentCoursesQuery(int stuId)
    {
        Query q = AdminStuCorCard.GetQuery(stuId);
        q.OrderBy(Field.semester__status, [SemesterStatus.started, SemesterStatus.waiting, SemesterStatus.finished]);
        return q;
    }
    
    [HttpGet("GetStudentCourses")]
    public IActionResult GetStudentCourses(int stuId, PaginationInfo paginationInfo)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 5 };
        }

        Query q = GetStudentCoursesQuery(stuId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        
        List<AdminStuCorCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
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
    
    private Query GetStudentRatingsQuery(int stuId)
    {
        Query q = AdminStuRatingCard.GetQuery(stuId);
        q.OrderBy(Field.rating__timestamp, desc: true); // Most recent ratings first
        return q;
    }
    
    [HttpGet("GetStudentRatings")]
    public IActionResult GetStudentRatings(int stuId, PaginationInfo paginationInfo)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 5 };
        }

        Query q = GetStudentRatingsQuery(stuId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        
        List<AdminStuRatingCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminStuRatingCard.GetCard(reader, ref tableIdx))
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
        return PartialView("List/_AdminStuRatingCardList", cards);
    }
    
    [HttpGet("GetStudentRatings/Pagination")]
    public IActionResult GetStudentRatingsPagination(int stuId, PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetStudentRatingsQuery(stuId).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    private Query GetTeachersQuery()
    {
        Query q = AdminMngTeacherCard.GetQuery();
        q.OrderBy(Field.teacher__id);
        return q;
    }

    [HttpGet("GetTeachers")]
    public IActionResult GetTeachers(PaginationInfo paginationInfo)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 20 };
        }

        Query q = GetTeachersQuery();
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        
        List<AdminMngTeacherCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminMngTeacherCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_AdminMngTeacherCardList", cards);
    }

    [HttpGet("GetTeachers/Pagination")]
    public IActionResult GetTeachersPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetTeachersQuery().Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
}