public class AccountUtils
{
    // ========================================================================
    public static int? getAdminTeacherId()
    {
        int? teacherId = null;
        QDatabase.Exec(conn =>
        {
            Query q = new(Tbl.teacher);
            // q.Where(Field.is_admin, true);
            q.Select(conn, reader =>
            {
                teacherId = QDataReader.GetInt(reader, 0);
            });
        });
        return teacherId;
    }

    // ========================================================================
}

/* EOF */
