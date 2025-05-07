// Trang tổng quan các khóa học
class BriefCoursePage
{
    const int NUM_DISPLAYED_COURSES = 20;
    public PaginationInfo MPaginationInfo { get; set; } = new();
    public BriefCourseFilterForm MBriefCourseFilter { get; set; } = new();
    public BriefCoursePage(BriefCourseFilterForm filter)
    {
        MBriefCourseFilter = filter;
        QDatabase.Exec(
            conn =>
            {
                MBriefCourseFilter.Reset(conn);
                Query q = new(Tbl.course);
                q.Join(Field.semester__course_id, Field.course__id);
                q.WhereQuery(Field.semester__id, SemesterQuery.getLatestSemesterIdQuery("s"));
                q.Where(Field.semester__status, [SemesterStatus.waiting, SemesterStatus.started]);

                if (filter.SubjectName is not null || filter.Grade != 0)
                {
                    q.Join(Field.subject__id, Field.course__sbj_id);
                }

                if (filter.SubjectName is not null)
                {
                    q.WhereNString(Field.subject__name, filter.SubjectName);
                }
                if (filter.Grade != 0)
                {
                    q.Where(Field.subject__grade, filter.Grade);
                }

                if (filter.Gender is not null)
                {
                    q.Join(Field.teacher__id, Field.course__tch_id);
                    q.Where(Field.teacher__gender, filter.Gender);
                }

                if (filter.CourseName is not null)
                {
                    q.WhereContains(Field.course__name, filter.CourseName);
                }

                MPaginationInfo.TotalItems = q.Count(conn);
                MPaginationInfo.ItemsPerPage = NUM_DISPLAYED_COURSES;
                MPaginationInfo.CurrentPage = 1;
            }
        );
    }
}

/* EOF */
