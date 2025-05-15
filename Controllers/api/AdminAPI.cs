using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using REPO.Models;

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
        QDatabase.Exec(conn => form.Execute(conn, ModelState, out teacher));
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

    private Query GetStudentsQuery(string? searchQuery = null)
    {
        Query q = AdminMngStuCard.GetQuery();
        q.OrderBy(Field.student__id, desc: false);
        if (searchQuery is not null)
        {
            string searchPattern = $"N'%{searchQuery}%'";
            string s = $"{Field.student__name} LIKE {searchPattern} OR {Field.student__tel} LIKE {searchPattern}";
            if (int.TryParse(searchQuery, out int id))
            {
                s += $" OR {Field.student__id} = {id}";
            }
            q.WhereClause($"({s})");
        }
        return q;
    }

    [HttpGet("GetStudents")]
    public IActionResult GetStudents(PaginationInfo paginationInfo, string? searchQuery = null)
    {
        Query q = GetStudentsQuery(searchQuery);
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
    public IActionResult GetStudentsPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent, string? searchQuery = null)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetStudentsQuery(searchQuery).Count(conn));
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

    private Query GetTeachersQuery(string? searchQuery = null)
    {
        Query q = AdminMngTeacherCard.GetQuery();
        q.OrderBy(Field.teacher__id);
        if (searchQuery is not null)
        {
            string searchPattern = $"N'%{searchQuery}%'";
            string s = $"{Fld.name} LIKE {searchPattern} OR {Fld.tel} LIKE {searchPattern}";
            if (int.TryParse(searchQuery, out int id))
            {
                s += $" OR {Fld.id} = {id}";
            }
            q.WhereClause($"({s})");
        }
        return q;
    }

    [HttpGet("GetTeachers")]
    public IActionResult GetTeachers(PaginationInfo paginationInfo, string? searchQuery = null)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 20 };
        }

        Query q = GetTeachersQuery(searchQuery);
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
    public IActionResult GetTeachersPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent, string? searchQuery = null)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetTeachersQuery(searchQuery).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    private Query GetCoursesQuery(string? searchQuery = null, string? status = null)
    {
        Query q = AdminMngCorCard.GetQuery();
        q.OrderBy(Field.course__id);
        if (searchQuery is not null)
        {
            string searchPattern = $"N'%{searchQuery}%'";
            string s = $"{Field.course__name} LIKE {searchPattern} OR {Field.teacher__name} LIKE {searchPattern}";
            s += $" OR {Field.subject__name} LIKE {searchPattern}";
            if (int.TryParse(searchQuery, out int id))
            {
                s += $" OR {Field.course__id} = {id}";
            }
            q.WhereClause($"({s})");
        }
        if (status is not null)
        {
            q.WhereClause($"status = '{status}'");
        }
        return q;
    }

    [HttpGet("GetCourses")]
    public IActionResult GetCourses(PaginationInfo paginationInfo, string? searchQuery = null, string? status = null)
    {
        Query q = GetCoursesQuery(searchQuery, status);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);

        List<AdminMngCorCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminMngCorCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_AdminMngCorCardList", cards);
    }

    [HttpGet("GetCourses/Pagination")]
    public IActionResult GetCoursesPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent, string? searchQuery = null, string? status = null)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetCoursesQuery(searchQuery, status).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    private Query GetCourseSemestersQuery(int courseId)
    {
        Query q = AdminMngCorSemCard.GetQuery(courseId);
        return q;
    }

    [HttpGet("GetCourseSemesters")]
    public IActionResult GetCourseSemesters(int courseId, PaginationInfo paginationInfo)
    {
        Query q = GetCourseSemestersQuery(courseId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);

        List<AdminMngCorSemCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminMngCorSemCard.GetCard(reader, ref tableIdx))
                );

                // Get course name for popup title
                if (cards.Count > 0)
                {
                    ViewBag.CourseName = cards[0].CourseName;
                    ViewBag.CourseId = courseId;
                }
                else
                {
                    Query courseQuery = new(Tbl.course);
                    courseQuery.Where(Field.course__id, courseId);
                    courseQuery.Output(Field.course__name);
                    courseQuery.Select(
                        conn,
                        reader =>
                        {
                            int pos = 0;
                            ViewBag.CourseName = QDataReader.GetString(reader, ref pos);
                        }
                    );
                    ViewBag.CourseId = courseId;
                }
            }
        );
        return PartialView("List/_AdminMngCorSemCardList", cards);
    }

    [HttpGet("GetCourseSemesters/Pagination")]
    public IActionResult GetCourseSemestersPagination(int courseId, PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetCourseSemestersQuery(courseId).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    private Query GetSemesterStudentsQuery(int semesterId)
    {
        Query q = AdminSemesterStudentCard.GetQuery(semesterId);
        return q;
    }

    [HttpGet("GetSemesterStudents")]
    public IActionResult GetSemesterStudents(int semesterId, PaginationInfo paginationInfo)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 10 };
        }

        Query q = GetSemesterStudentsQuery(semesterId);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);

        List<AdminSemesterStudentCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminSemesterStudentCard.GetCard(reader, ref tableIdx))
                );

                // Get semester info for the modal title
                if (cards.Count == 0)
                {
                    Query semQuery = new(Tbl.semester);
                    semQuery.Where(Field.semester__id, semesterId);
                    semQuery.Join(Field.course__id, Field.semester__course_id);
                    semQuery.Output(Field.course__name);
                    semQuery.Output(Field.semester__id);
                    semQuery.Select(
                        conn,
                        reader =>
                        {
                            int pos = 0;
                            ViewBag.CourseName = QDataReader.GetString(reader, ref pos);
                            ViewBag.SemesterId = QDataReader.GetInt(reader, ref pos);
                        }
                    );
                }
            }
        );
        return PartialView("List/_AdminSemesterStudentCardList", cards);
    }

    [HttpGet("GetSemesterStudents/Pagination")]
    public IActionResult GetSemesterStudentsPagination(int semesterId, PaginationInfo paginationInfo, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetSemesterStudentsQuery(semesterId).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
    private Query GetRatingsQuery(string? searchQuery = null, int? stars = null)
    {
        Query q = AdminMngRatingCard.GetQuery();
        if (searchQuery is not null)
        {
            string searchPattern = $"N'%{searchQuery}%'";
            q.WhereClause($"({Field.student__name} LIKE {searchPattern} OR {Field.course__name} LIKE {searchPattern} OR {Field.rating__description} LIKE {searchPattern})");
        }

        if (stars is not null)
        {
            q.Where(Field.rating__stars, stars);
        }
        return q;
    }

    [HttpGet("GetRatings")]
    public IActionResult GetRatings(PaginationInfo paginationInfo, string? searchQuery = null, int? stars = null)
    {
        if (paginationInfo == null)
        {
            paginationInfo = new() { CurrentPage = 1, ItemsPerPage = 20 };
        }

        Query q = GetRatingsQuery(searchQuery, stars);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);

        List<AdminMngRatingCard> cards = [];
        QDatabase.Exec(
            conn =>
            {
                int tableIdx = paginationInfo.FirstIndex;
                q.Select(
                    conn,
                    reader => cards.Add(AdminMngRatingCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_AdminMngRatingCardList", cards);
    }

    [HttpGet("GetRatings/Pagination")]
    public IActionResult GetRatingsPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent, string? searchQuery = null, int? stars = null)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = GetRatingsQuery(searchQuery, stars).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
    [HttpPost("DeleteRating")]
    public IActionResult DeleteRating(int studentId, int semesterId)
    {
        bool success = false;
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.rating);
            q.Where(Field.rating__stu_id, studentId);
            q.Where(Field.rating__semester_id, semesterId);
            q.Delete(conn);
            success = true;
        });

        return Json(new { success });
    }
    [HttpGet("GetAllDashboardStatistics")]
    public IActionResult GetAllDashboardStatistics()
    {
        // Use the cached instance to get all statistics in a single database fetch
        var statistics = AdminStatistics.GetInstance();
        return Json(new
        {
            basicStats = new
            {
                totalStudents = statistics.TotalStudents,
                totalTeachers = statistics.TotalTeachers,
                totalCourses = statistics.TotalCourses,
                totalRevenue = statistics.TotalRevenue,
                totalSemesters = statistics.TotalSemesters,
                totalRatings = statistics.TotalRatings
            },
            monthlyRegistrations = statistics.MonthlyRegistrations,
            semestersByStatus = statistics.SemestersByStatus,
            ratingDistribution = statistics.RatingDistribution,
            coursesByStatus = statistics.CoursesByStatus,
            topUpcomingCourses = statistics.TopUpcomingCourses,
            topRatedCourses = statistics.TopRatedCourses
        });
    }

    [HttpGet("GetDeleteAccountForm")]
    public IActionResult GetDeleteAccountForm(int id, string role)
    {
        DeleteAccountForm form = new(id, role);
        return PartialView("Form/_DeleteAccountForm", form);
    }

    [HttpPost("SubmitDeleteAccountForm")]
    public IActionResult SubmitDeleteAccountForm(DeleteAccountForm form)
    {
        if (!ModelState.IsValid)
            return PartialView("Form/_DeleteAccountForm", form);

        QDatabase.Exec(form.Execute);
        return PartialView("Form/_DeleteAccountForm", form);
    }

    [HttpGet("GetDeleteCourseForm")]
    public IActionResult GetDeleteCourseForm(int id, string submitUrl)
    {
        DeleteCourseForm form = new(id, submitUrl);
        return PartialView("Form/_DeleteCourseForm", form);
    }

    [HttpPost("SubmitDeleteCourseForm")]
    public IActionResult SubmitDeleteCourseForm(DeleteCourseForm form)
    {
        if (!ModelState.IsValid)
            return PartialView("Form/_DeleteCourseForm", form);

        QDatabase.Exec(form.Execute);
        return PartialView("Form/_DeleteCourseForm", form);
    }
}