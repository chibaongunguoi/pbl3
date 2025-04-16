using Microsoft.Data.SqlClient;

abstract class DataObj
{
    // ========================================================================
    // INFO: fetch là phương thức dùng để lấy dữ liệu từ SqlDataReader.
    // Cần xác định tham số pos là chỉ số cột bắt đầu để đọc dữ liệu.
    public virtual void fetch(SqlDataReader reader, ref int pos)
    {
        // INFO: Sau khi đọc xong, cần trả về pos để
        // phục vụ cho việc đọc dữ liệu ở các lớp con.
    }

    // ------------------------------------------------------------------------
    // INFO: Chuyển đổi sang dạng xâu csv
    public virtual List<string> ToListString()
    {
        return [];
    }

    // ========================================================================
}

/* EOF */
