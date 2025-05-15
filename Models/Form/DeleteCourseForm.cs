using Microsoft.Data.SqlClient;
public class DeleteCourseForm
{
    public int Id { get; set; }

    public Dictionary<string, string> Messages { get; set; } = new();

    public bool ShowForm { get; set; } = true;

    public string SubmitUrl { get; set; } = string.Empty;

    public DeleteCourseForm() { }

    public DeleteCourseForm(int id, string submitUrl)
    {
        Id = id;
        SubmitUrl = submitUrl;
        int? tempId = null;
        QDatabase.Exec(
            conn =>
            {
                Query q = new(Tbl.course);
                q.Where(Field.course__id, id);
                q.Select(conn, reader => tempId = QDataReader.GetInt(reader));
            }
        );

        if (tempId is null)
        {
            Messages["Error"] = "Không tìm thấy khóa học này";
            ShowForm = false;
        }
    }

    public void Execute(SqlConnection conn)
    {
        Query qq = new(Tbl.semester);
        qq.Where(Field.semester__course_id, Id);
        qq.Output(Field.semester__id);

        Query q = new(Tbl.rating);
        q.WhereInQuery(Field.rating__semester_id, qq.SelectQuery());
        q.Delete(conn);

        q = new(Tbl.request);
        q.WhereInQuery(Field.request__semester_id, qq.SelectQuery());
        q.Delete(conn);

        q = new(Tbl.semester);
        q.Where(Field.semester__course_id, Id);
        q.Delete(conn);

        q = new(Tbl.course);
        q.Where(Field.course__id, Id);
        q.Delete(conn);

        Messages["Success"] = "Xóa khóa học thành công";
        ShowForm = false;
    }
}