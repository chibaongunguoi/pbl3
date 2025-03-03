using Microsoft.Data.SqlClient;

sealed class DataReader
{
    // ========================================================================
    public static int get_int(SqlDataReader reader, int pos)
    {
        return reader.GetInt32(pos);
    }

    // ------------------------------------------------------------------------
    public static string get_string(SqlDataReader reader, int pos)
    {
        return reader.GetString(pos);
    }

    // ------------------------------------------------------------------------
    public static T get_enum<T>(SqlDataReader reader, int pos)
        where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), reader.GetInt32(pos));
    }

    // ========================================================================
}

/* EOF */
