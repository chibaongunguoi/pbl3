// using Microsoft.Data.SqlClient;
// using System.Text.Json.Serialization;

// namespace REPO.Models;

// public class AdminStatistics
// {
//     // Cache mechanism fields
//     private static AdminStatistics? _instance;
//     private static readonly object _lock = new object();
//     private static DateTime _lastUpdated = DateTime.MinValue;
//     private static readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5); // Cache expires after 5 minutes
    
//     public int TotalStudents { get; set; }
//     public long TotalRevenue { get; set; }
//     public int TotalSemesters { get; set; }
//     public int TotalRatings { get; set; }
    
//     public List<MonthlyRegistration> MonthlyRegistrations { get; set; } = new();
//     public List<SemestersByStatus> SemestersByStatus { get; set; } = new();
//     public List<RatingDistribution> RatingDistribution { get; set; } = new();
//     public List<CoursesByStatus> CoursesByStatus { get; set; } = new();
//     public List<TopCourse> TopUpcomingCourses { get; set; } = new();
//     public List<TopCourse> TopRatedCourses { get; set; } = new();
    
//     /// <summary>
//     /// Gets a shared instance of AdminStatistics, using the cached copy if it's still valid
//     /// </summary>
//     public static AdminStatistics GetInstance()
//     {
//         bool needsRefresh = false;
        
//         lock (_lock) 
//         {
//             // Check if cache needs refresh (it's expired or first time)
//             needsRefresh = _instance == null || (DateTime.Now - _lastUpdated) > _cacheExpiration;
            
//             // If cache is still valid, return it
//             if (!needsRefresh)
//                 return _instance!;
                
//             // Create new instance or refresh existing
//             if (_instance == null)
//                 _instance = new AdminStatistics();
//             else
//                 _instance.LoadStatistics();
                
//             _lastUpdated = DateTime.Now;
//             return _instance;
//         }
//     }
    
//     public AdminStatistics()
//     {
//         LoadStatistics();
//     }
    
//     // Force refreshing the stats regardless of cache
//     public void RefreshStatistics()
//     {
//         LoadStatistics();
//         lock (_lock) {
//             _lastUpdated = DateTime.Now;
//         }
//     }
    
//     private void LoadStatistics()
//     {
//         // Clear previous data
//         MonthlyRegistrations.Clear();
//         SemestersByStatus.Clear();
//         RatingDistribution.Clear();
//         CoursesByStatus.Clear();
//         TopUpcomingCourses.Clear();
//         TopRatedCourses.Clear();
        
//         QDatabase.Exec(conn =>
//         {
//             // Get total students
//             Query stuQuery = new(Tbl.student);
//             TotalStudents = stuQuery.Count(conn);
            
//             // Get total ratings
//             Query ratingQuery = new(Tbl.rating);
//             TotalRatings = ratingQuery.Count(conn);
            
//             // Get total semesters
//             Query semesterQuery = new(Tbl.semester);
//             TotalSemesters = semesterQuery.Count(conn);
            
//             // Create a query to get the total revenue
//             Query revenueQuery = new(Tbl.semester, "s");
//             revenueQuery.Join(Field.request__semester_id, Field.semester__id, "r", "s");
//             revenueQuery.Where(Field.request__status, RequestStatus.joined, "r");
//             revenueQuery.OutputSumCastBigInt(Field.semester__fee, "s");
            
//             // Get total revenue
//             revenueQuery.Select(conn, reader =>
//             {
//                 TotalRevenue = reader.GetInt64(0);
//             });
            
//             // Get monthly registrations for the past 12 months
//             Query monthlyRegQuery = new(Tbl.request, "r");
//             monthlyRegQuery.WhereClause($"[r].[{Fld.timestamp}] >= DATEADD(month, -12, GETDATE())");
//             monthlyRegQuery.Where(Field.request__status, RequestStatus.joined, "r");
//             monthlyRegQuery.OutputClause($"YEAR([r].[{Fld.timestamp}]) as Year");
//             monthlyRegQuery.OutputClause($"MONTH([r].[{Fld.timestamp}]) as Month");
//             monthlyRegQuery.OutputClause("COUNT(*) as Count");
//             monthlyRegQuery.GroupBy($"YEAR([r].[{Fld.timestamp}])");
//             monthlyRegQuery.GroupBy($"MONTH([r].[{Fld.timestamp}])");
//             monthlyRegQuery.OrderBy($"YEAR([r].[{Fld.timestamp}])", desc: false);
//             monthlyRegQuery.OrderBy($"MONTH([r].[{Fld.timestamp}])", desc: false);
            
//             monthlyRegQuery.Select(conn, reader =>
//             {
//                 MonthlyRegistrations.Add(new MonthlyRegistration
//                 {
//                     Year = reader.GetInt32(0),
//                     Month = reader.GetInt32(1),
//                     Count = reader.GetInt32(2)
//                 });
//             });
            
//             // Get semesters by status count
//             Query statusQuery = new(Tbl.semester);
//             statusQuery.Output(Field.semester__status);
//             statusQuery.OutputClause("COUNT(*) as Count");
//             statusQuery.GroupBy(Field.semester__status);
            
//             statusQuery.Select(conn, reader =>
//             {
//                 SemestersByStatus.Add(new SemestersByStatus
//                 {
//                     Status = reader.GetString(0) switch
//                     {
//                         SemesterStatus.waiting => "Sắp diễn ra",
//                         SemesterStatus.started => "Đang diễn ra",
//                         SemesterStatus.finished => "Đã kết thúc",
//                         _ => string.Empty
//                     },
//                     Count = reader.GetInt32(1)
//                 });
//             });
            
//             // Get rating distribution
//             Query ratingDistQuery = new(Tbl.rating);
//             ratingDistQuery.Output(Field.rating__stars);
//             ratingDistQuery.OutputClause("COUNT(*) as Count");
//             ratingDistQuery.GroupBy(Field.rating__stars);
//             ratingDistQuery.OrderBy(Field.rating__stars);
            
//             ratingDistQuery.Select(conn, reader =>
//             {
//                 RatingDistribution.Add(new RatingDistribution
//                 {
//                     Stars = reader.GetInt32(0),
//                     Count = reader.GetInt32(1)
//                 });
//             });
            
//             // Get courses by their latest semester status
//             Query latestSemesterQuery = new(Tbl.semester, "s");
//             latestSemesterQuery.WhereField(Field.semester__course_id, Field.semester__course_id, "s", null);
//             latestSemesterQuery.OrderBy(Field.semester__start_date, desc: true, "s");
//             latestSemesterQuery.OutputTop(Field.semester__id, 1, "s");

//             Query courseStatusQuery = new(Tbl.semester);
//             courseStatusQuery.WhereQuery(Field.semester__id, latestSemesterQuery.SelectQuery());
//             courseStatusQuery.Output(Field.semester__status);
//             courseStatusQuery.OutputClause("COUNT(*) as Count");
//             courseStatusQuery.GroupBy(Field.semester__status);
            
//             courseStatusQuery.Select(conn, reader =>
//             {
//                 CoursesByStatus.Add(new CoursesByStatus
//                 {
//                     Status = reader.GetString(0) switch
//                     {
//                         SemesterStatus.waiting => "Sắp diễn ra",
//                         SemesterStatus.started => "Đang diễn ra",
//                         SemesterStatus.finished => "Đã kết thúc",
//                         _ => string.Empty
//                     },
//                     Count = reader.GetInt32(1)
//                 });
//             });
            
//             // Get top upcoming courses (waiting status, sorted by start date)
//             Query upcomingCoursesQuery = new(Tbl.course, "c");
//             upcomingCoursesQuery.Join(Field.semester__course_id, Field.course__id, "s", "c");
//             upcomingCoursesQuery.Join(Field.teacher__id, Field.course__tch_id, "t", "c");
//             upcomingCoursesQuery.Join(Field.subject__id, Field.course__sbj_id, "sub", "c");
//             upcomingCoursesQuery.Where(Field.semester__status, SemesterStatus.waiting, "s");
            
//             upcomingCoursesQuery.Output(Field.course__id, "c");
//             upcomingCoursesQuery.Output(Field.course__name, "c");
//             upcomingCoursesQuery.Output(Field.teacher__name, "t");
//             upcomingCoursesQuery.Output(Field.subject__name, "sub");
//             upcomingCoursesQuery.Output(Field.semester__capacity, "s");
            
//             // Use SemesterQuery helper method to get participants count
//             SemesterQuery.GetParticipantsCount(ref upcomingCoursesQuery, "s");
            
//             // Sort by start date and limit to top 5
//             upcomingCoursesQuery.OrderBy(Field.semester__start_date, desc: false, "s");
//             upcomingCoursesQuery.OutputClause("TOP 5");
            
//             upcomingCoursesQuery.Select(conn, reader =>
//             {
//                 TopUpcomingCourses.Add(new TopCourse
//                 {
//                     CourseId = reader.GetInt32(0),
//                     CourseName = reader.GetString(1),
//                     TeacherName = reader.GetString(2),
//                     SubjectName = reader.GetString(3),
//                     Slots = reader.GetInt32(4),
//                     EnrolledStudents = reader.GetInt32(5)
//                 });
//             });
            
//             // Get top rated courses
//             Query topRatedCoursesQuery = new(Tbl.course, "c");
//             topRatedCoursesQuery.Join(Field.teacher__id, Field.course__tch_id, "t", "c");
//             topRatedCoursesQuery.Join(Field.subject__id, Field.course__sbj_id, "sub", "c");
            
//             topRatedCoursesQuery.Output(Field.course__id, "c");
//             topRatedCoursesQuery.Output(Field.course__name, "c");
//             topRatedCoursesQuery.Output(Field.teacher__name, "t");
//             topRatedCoursesQuery.Output(Field.subject__name, "sub");
            
//             // Use SemesterQuery helper method to get rating average
//             SemesterQuery.GetRatingAvg(ref topRatedCoursesQuery, "c");
            
//             // Create a query for enrolled students count
//             Query enrolledStudentsQuery = new(Tbl.semester, "sem");
//             enrolledStudentsQuery.Join(Field.request__semester_id, Field.semester__id, "req", "sem");
//             enrolledStudentsQuery.WhereField(Field.semester__course_id, Field.course__id, "sem", "c");
//             enrolledStudentsQuery.Where(Field.request__status, RequestStatus.joined, "req");
//             enrolledStudentsQuery.Output(QPiece.countAll);
//             topRatedCoursesQuery.OutputQuery(enrolledStudentsQuery.SelectQuery());
            
//             // Filter to only include courses with at least one rating
//             Query ratingExistsQuery = new(Tbl.rating, "rt");
//             ratingExistsQuery.Join(Field.semester__id, Field.rating__semester_id, "sem", "rt");
//             ratingExistsQuery.WhereField(Field.semester__course_id, Field.course__id, "sem", "c");
//             ratingExistsQuery.Output("1");
//             topRatedCoursesQuery.WhereClause($"EXISTS ({ratingExistsQuery.SelectQuery()})");
            
//             // Sort by rating average (column 4) - highest first and limit to top 5
//             topRatedCoursesQuery.OrderByClause("5 DESC");
//             topRatedCoursesQuery.OutputClause("TOP 5");
            
//             topRatedCoursesQuery.Select(conn, reader =>
//             {
//                 TopRatedCourses.Add(new TopCourse
//                 {
//                     CourseId = reader.GetInt32(0),
//                     CourseName = reader.GetString(1),
//                     TeacherName = reader.GetString(2),
//                     SubjectName = reader.GetString(3),
//                     Rating = reader.IsDBNull(4) ? null : reader.GetFloat(4),
//                     EnrolledStudents = reader.GetInt32(5)
//                 });
//             });
//         });
        
//         // Ensure we have entries for all star ratings 1-5 even if count is 0
//         var existingStars = RatingDistribution.Select(r => r.Stars).ToHashSet();
//         for (int i = 1; i <= 5; i++)
//         {
//             if (!existingStars.Contains(i))
//             {
//                 RatingDistribution.Add(new RatingDistribution { Stars = i, Count = 0 });
//             }
//         }
//         RatingDistribution = RatingDistribution.OrderBy(r => r.Stars).ToList();
        
//         // Ensure we have entries for all possible semester statuses in CoursesByStatus
//         var statuses = new[] { SemesterStatus.waiting, SemesterStatus.started, SemesterStatus.finished };
//         var statusLabels = new Dictionary<string, string>
//         {
//             { SemesterStatus.waiting, "Sắp diễn ra" },
//             { SemesterStatus.started, "Đang diễn ra" },
//             { SemesterStatus.finished, "Đã kết thúc" }
//         };
        
//         var existingStatusLabels = CoursesByStatus.Select(c => c.Status).ToHashSet();
        
//         foreach (var status in statuses)
//         {
//             var label = statusLabels[status];
//             if (!existingStatusLabels.Contains(label))
//             {
//                 CoursesByStatus.Add(new CoursesByStatus { Status = label, Count = 0 });
//             }
//         }
//     }
// }

// public class MonthlyRegistration
// {
//     public int Year { get; set; }
//     public int Month { get; set; }
    
//     [JsonIgnore]
//     public string MonthName => new DateTime(Year, Month, 1).ToString("MM/yyyy");
    
//     public string Label => MonthName;
//     public int Count { get; set; }
// }

// public class SemestersByStatus
// {
//     public string Status { get; set; } = "";
//     public int Count { get; set; }
// }

// public class RatingDistribution
// {
//     public int Stars { get; set; }
//     public int Count { get; set; }
// }

// public class CoursesByStatus
// {
//     public string Status { get; set; } = "";
//     public int Count { get; set; }
// }

// public class TopCourse
// {
//     public int CourseId { get; set; }
//     public string CourseName { get; set; } = "";
//     public string TeacherName { get; set; } = "";
//     public string SubjectName { get; set; } = "";
//     public float? Rating { get; set; }
//     public int Slots { get; set; }
//     public int EnrolledStudents { get; set; }
// }
