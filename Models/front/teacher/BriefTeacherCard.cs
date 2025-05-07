using Microsoft.Data.SqlClient;

public class BriefTeacherCard : DataObj
{
    public int id;
    public string name = "";
    public string gender = "";
    public string bday = "";
    public string description = "";

    public override void Fetch(SqlDataReader reader, ref int pos)
    {
        Teacher teacher = QDataReader.GetDataObj<Teacher>(reader, ref pos);
        this.id = teacher.Id;
        this.name = teacher.Name;
        this.gender = IoUtils.convGender(teacher.Gender);
        this.bday = IoUtils.conv(teacher.Bday);
        this.description = teacher.Description;
    }
    // ------------------------------------------------------------------------
}
