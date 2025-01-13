namespace module_object;

using Microsoft.Data.SqlClient;

sealed class Student : User
{
    // ========================================================================
    private int grade;

    // ========================================================================
    public Student() { }

    // ========================================================================
    public override int fetch(SqlDataReader reader, int pos)
    {
        pos = base.fetch(reader, pos);
        this.grade = reader.GetInt32(pos++);
        return pos;
    }

    // ------------------------------------------------------------------------
    public override void print()
    {
        base.print();
    }

    // ========================================================================
}

/* EOF */
