using Microsoft.Data.SqlClient;

class DetailedCourseCard : BriefCourseCard
{
    public string description = "";

    public new static Query GetQueryCreator(string? role = null, string? username = null)
    {
        Query q = BriefCourseCard.GetQueryCreator(role, username);
        q.Output(Field.semester__description);
        return q;
    }

    public new static DetailedCourseCard GetCard(SqlDataReader reader, ref int pos, string? role=null)
    {
        pos = 0;
        BriefCourseCard card = BriefCourseCard.GetCard(reader, ref pos, role);
        var description = QDataReader.GetString(reader, ref pos);
        return new DetailedCourseCard()
        {
            courseId = card.courseId,
            courseName = card.courseName,
            subject = card.subject,
            grade = card.grade,
            dates = card.dates, 
            tchName = card.tchName,
            participants = card.participants,
            avgRating = card.avgRating,
            numRatings = card.numRatings,
            fee = card.fee,
            description = description,
            CanJoin = card.CanJoin,
        };
    }
}

/* EOF */
