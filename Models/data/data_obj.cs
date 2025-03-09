using Microsoft.Data.SqlClient;

abstract class DataObj
{
    // ========================================================================
    // INFO: fetch_data là phương thức dùng để lấy dữ liệu từ SqlDataReader.
    // Cần xác định tham số pos là chỉ số cột bắt đầu để đọc dữ liệu.
    public virtual int fetch_data(SqlDataReader reader, int pos = 0)
    {
        // INFO: Sau khi đọc xong, cần trả về pos để
        // phục vụ cho việc đọc dữ liệu ở các lớp con.
        return pos;
    }

    // ------------------------------------------------------------------------
    // INFO: Chuyển đổi sang dạng xâu csv
    public override string ToString()
    {
        return "";
    }

    // ========================================================================
}

/* EOF */
