class AccountUtils
{
    // ========================================================================
    public static InfoAccountType get_account_type(int id)
    {
        if (1000 <= id && id < 2000)
        {
            return InfoAccountType.STUDENT;
        }
        else if (2000 <= id && id < 3000)
        {
            return InfoAccountType.TEACHER;
        }
        return InfoAccountType.NONE;
    }

    // ========================================================================
}

/* EOF */
