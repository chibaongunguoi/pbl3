using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient;

namespace REPO.Controllers;

[Route("TeacherManageAPI")]
[Authorize(Roles = UserRole.Teacher)]
public class TeacherManageAPI : BaseController
{
    private static Query GetManageCourseQueryCreator(string username, string? searchQuery)
    {
        Query q = ManageCourseCard.GetQueryCreator();
        q.OrderBy(Field.semester__start_date, desc: true);
        q.Where(Field.teacher__username, username);
        if (searchQuery is not null)
        {
            q.WhereContains(Field.course__name, searchQuery);
        }
        return q;
    }

    [HttpGet("ManageCourse")]
    public IActionResult ManageCourse(PaginationInfo paginationInfo, string? searchQuery)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        List<ManageCourseCard> cards = [];
        Query q = GetManageCourseQueryCreator(username, searchQuery);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        int pos = 0;
        int current_table_index = 1;
        QDatabase.Exec(conn => q.Select(conn, reader => cards.Add(ManageCourseCard.GetCard(reader, ref pos, ref current_table_index))));
        return PartialView("List/_ManageCourseCardList", cards);
    }

    [HttpGet("ManageCourse/Pagination")]
    public IActionResult ManageCoursePagination(PaginationInfo paginationInfo, string? searchQuery, string contextUrl, string contextComponent)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        Query q = GetManageCourseQueryCreator(username, searchQuery);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
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

    private static Query GetManageRequestQueryCreator(string username, string? searchQuery = null)
    {
        Query q = ManageRequestCard.GetQueryCreator();
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.Where(Field.teacher__username, username);
        q.Where(Field.request__status, RequestStatus.waiting);
        if (searchQuery is not null)
        {
            string pattern = $"N'%{searchQuery}%'";
            List<string> s = [
                $"{Field.course__name} LIKE {pattern}",
                $"{Field.student__name} LIKE {pattern}",
                $"{Field.student__tel} LIKE {pattern}",
            ];

            if (int.TryParse(searchQuery, out int id))
            {
                s.Add($"{Field.student__id} = {id}");
            }

            q.WhereClause($"({string.Join(" OR ", s)})");
        }
        return q;
    }

    [HttpGet("ManageRequest")]
    public IActionResult ManageRequest(PaginationInfo paginationInfo, string? searchQuery = null)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        List<ManageRequestCard> cards = [];
        Query q = GetManageRequestQueryCreator(username, searchQuery);
        q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
        int tableIdx = 1;
        QDatabase.Exec(
            conn =>
            {
                q.Select(
                    conn,
                    reader => cards.Add(ManageRequestCard.GetCard(reader, ref tableIdx))
                );
            }
        );
        return PartialView("List/_ManageRequestCardList", cards);
    }

    [HttpGet("ManageRequest/Pagination")]
    public IActionResult ManageRequestPagination(PaginationInfo paginationInfo, string? searchQuery, string contextUrl, string contextComponent)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        Query q = GetManageRequestQueryCreator(username, searchQuery);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }

    [HttpGet("AcceptRequest")]
    public IActionResult AcceptRequest(int stuId, int semesterId, PaginationInfo paginationInfo, string? searchQuery = null)
    {
        Query q = new(Tbl.request);
        q.Set(Field.request__status, RequestStatus.joined);
        q.Where(Field.request__semester_id, semesterId);
        q.Where(Field.request__stu_id, stuId);
        QDatabase.Exec(q.Update);
        // Add notification for student
        string? courseName = null;
        string? teacherName = null;
        QDatabase.Exec(conn =>
        {
            Query courseQuery = new(Tbl.semester);
            courseQuery.Join(Field.course__id, Field.semester__course_id);
            courseQuery.Join(Field.teacher__id, Field.course__tch_id);
            courseQuery.Where(Field.semester__id, semesterId);
            courseQuery.Output(Field.course__name);
            courseQuery.Output(Field.teacher__name);
            courseQuery.Select(conn, reader =>
            {
                int pos = 0;
                courseName = QDataReader.GetString(reader, ref pos);
                teacherName = QDataReader.GetString(reader, ref pos);
            });
        });
        string message = $"Bạn đã được chấp nhận vào khóa học {courseName} của giảng viên {teacherName}";
        QDatabase.Exec(conn => Notification.Add(conn, stuId, message));
        return ManageRequest(paginationInfo, searchQuery);
    }

    [HttpGet("AcceptRequest/Pagination")]
    public IActionResult AcceptRequestPagination(PaginationInfo paginationInfo, string? searchQuery, string contextUrl, string contextComponent)
    {
        return ManageRequestPagination(paginationInfo, searchQuery, contextUrl, contextComponent);
    }

    [HttpPost("RejectRequest")]
    public IActionResult RejectRequest(int stuId, int semesterId, string? reason = null, PaginationInfo? paginationInfo = null, string? searchQuery = null)
    {
        Query q = new(Tbl.request);
        q.Where(Field.request__semester_id, semesterId);
        q.Where(Field.request__stu_id, stuId);
        QDatabase.Exec(q.Delete);
        string? courseName = null;
        string? teacherName = null;
        
        QDatabase.Exec(conn =>
        {
            Query courseQuery = new(Tbl.semester);
            courseQuery.Join(Field.course__id, Field.semester__course_id);
            courseQuery.Join(Field.teacher__id, Field.course__tch_id);
            courseQuery.Where(Field.semester__id, semesterId);
            courseQuery.Output(Field.course__name);
            courseQuery.Output(Field.teacher__name);
            courseQuery.Select(conn, reader =>
            {
                int pos = 0;
                courseName = QDataReader.GetString(reader, ref pos);
                teacherName = QDataReader.GetString(reader, ref pos);
            });
            string message = string.IsNullOrWhiteSpace(reason)
                ? $"Yêu cầu tham gia khóa học {courseName} của giảng viên {teacherName} đã bị từ chối."
                : $"Yêu cầu tham gia khóa học {courseName} của giảng viên {teacherName} đã bị từ chối. Lý do: {reason}";
            Notification.Add(conn, stuId, message);
        });
        paginationInfo ??= new PaginationInfo { CurrentPage = 1, ItemsPerPage = 20 };
        return ManageRequest(paginationInfo, searchQuery);
    }
    [HttpGet("GetEditCourseForm")]
    public IActionResult GetEditCourseForm(int courseId)
    {
        // Verify that the course belongs to the current teacher
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        bool hasAccess = false;

        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.course);
            q.Join(Field.teacher__id, Field.course__tch_id);
            q.Where(Field.course__id, courseId);
            q.Where(Field.teacher__username, username);
            hasAccess = q.Count(conn) > 0;
        });

        if (!hasAccess)
        {
            return Unauthorized();
        }

        EditCourseForm form = new(courseId);
        return PartialView("Form/_EditCourseForm", form);
    }

    [HttpPost("EditCourse")]
    public IActionResult EditCourse(EditCourseForm form)
    {
        if (!ModelState.IsValid)
        {
            return PartialView("Form/_EditCourseForm", form);
        }

        // Verify that the course belongs to the current teacher
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        bool hasAccess = false;

        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.course);
            q.Join(Field.teacher__id, Field.course__tch_id);
            q.Where(Field.course__id, form.CourseId);
            q.Where(Field.teacher__username, username);
            hasAccess = q.Count(conn) > 0;
        });

        if (!hasAccess)
        {
            form.Messages["Error"] = "Bạn không có quyền chỉnh sửa khóa học này";
            return PartialView("Form/_EditCourseForm", form);
        }

        Semester? semester = null;
        List<int> affectedStudents = [];

        QDatabase.Exec(conn =>
        {
            // Get list of students waiting or joined for this semester
            Query studentQuery = new(Tbl.request);
            studentQuery.Where(Field.request__semester_id, form.SemesterId);
            studentQuery.Output(Field.request__stu_id);
            studentQuery.Select(conn, reader => 
            {
                int pos = 0;
                affectedStudents.Add(QDataReader.GetInt(reader, ref pos));
            });

            // Get course name for notification
            string? courseName = null;
            string? teacherName = null;

            Query teacherQuery = new(Tbl.teacher);
            teacherQuery.Where(Field.teacher__username, username);
            teacherQuery.Output(Field.teacher__name);
            teacherQuery.Select(conn, reader => {
                int pos = 0;
                teacherName = QDataReader.GetString(reader, ref pos);
            });

            Query courseQuery = new(Tbl.course);
            courseQuery.Where(Field.course__id, form.CourseId);
            courseQuery.Output(Field.course__name);
            courseQuery.Select(conn, reader => {
                int pos = 0;
                courseName = QDataReader.GetString(reader, ref pos);
            });

            form.Execute(conn, out semester);

            if (semester != null)
            {
                // Send notifications to all affected students
                foreach (var studentId in affectedStudents)
                {
                    string message = $"Khóa học {courseName} của giảng viên {teacherName} đã được cập nhật. Vui lòng kiểm tra thông tin mới.";
                    Notification.Add(conn, studentId, message);
                }
            }
        });

        return PartialView("Form/_EditCourseForm", form);
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

    private static Query GetManageRatingQueryCreator(string username, string? searchQuery = null, int? stars = null)
    {
        Query q = ManageRatingCard.GetQueryCreator();
        q.Where(Field.teacher__username, username);
        q.Join(Field.teacher__id, Field.course__tch_id);
        q.OrderBy(Field.rating__timestamp, desc: true);
        if (searchQuery is not null)
        {
            string pattern = $"N'%{searchQuery}%'";
            List<string> s = [
                $"{Field.course__name} LIKE {pattern}",
                $"{Field.student__name} LIKE {pattern}",
                $"{Field.rating__description} LIKE {pattern}",
            ];
            q.WhereClause($"({string.Join(" OR ", s)})");
        }
        if (stars is not null)
        {
            q.Where(Field.rating__stars, stars);
        }
        return q;
    }

    [HttpGet("ManageRating")]
    public IActionResult ManageRating(PaginationInfo paginationInfo, string? searchQuery = null, int? stars = null)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        List<ManageRatingCard> cards = [];
        int tableIdx = 1;

        Query q = GetManageRatingQueryCreator(username, searchQuery, stars);
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
    public IActionResult ManageRatingPagination(PaginationInfo paginationInfo, string contextUrl, string contextComponent, string? searchQuery = null, int? stars = null)
    {
        string username = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        Query q = GetManageRatingQueryCreator(username, searchQuery, stars);
        QDatabase.Exec(conn => paginationInfo.TotalItems = q.Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
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
        {
            return PartialView("Form/_DeleteCourseForm", form);
        }

        QDatabase.Exec(form.Execute);
        return PartialView("Form/_DeleteCourseForm", form);
    }
}