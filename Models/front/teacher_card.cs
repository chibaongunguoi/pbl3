struct BriefTeacherCard
{
    public int id;
    public string name;
    public string gender;
    public string bday;
    public string description;
}

struct DetailedTeacherCard
{
    public int id;
    public string name;
    public string bday;
    public string grades;
    public string subjects;
    public string description;
}

struct BriefTeacherPage
{
    public int current_page;
    public int total_page;
    public List<BriefTeacherCard> teachers = new();

    public BriefTeacherPage(int current_page) { }
}
