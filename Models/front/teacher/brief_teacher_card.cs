using Microsoft.Data.SqlClient;

class BriefTeacherCard : DataObj
{
    public int id;
    public string name = "";
    public string gender = "";
    public string bday = "";
    public string description = "";

    public override void fetch_data(SqlDataReader reader, ref int pos)
    {
        Teacher teacher = DataReader.get_data_obj<Teacher>(reader, ref pos);
        this.id = teacher.id;
        this.name = teacher.name;
        this.gender = IoUtils.conv_gender(teacher.gender);
        this.bday = IoUtils.conv(teacher.bday);
        this.description = teacher.description;
    }
    // ------------------------------------------------------------------------
}
