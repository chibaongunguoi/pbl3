class AccountUtils
{
    // ========================================================================
    // INFO: Xác định vai trò của người dùng bằng id
    public static Table get_account_type(int id)
    {
        if (1000 <= id && id < 2000)
        {
            return Table.student;
        }
        else if (2000 <= id && id < 3000)
        {
            return Table.teacher;
        }

        return Table.admin;
    }

    // ========================================================================
}

/* EOF */
