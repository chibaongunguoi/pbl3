using Microsoft.AspNetCore.Mvc;

namespace REPO.Controllers;

[Route("teacherAPI")]
public class TeacherAPI : BaseController
{
    private static Query BriefTeacherPageQuery(BriefTeacherFilter filter)
    {
        Query q = new(Tbl.teacher);

        if (filter.Gender is not null)
        {
            q.Where(Field.teacher__gender, filter.Gender);
        }

        if (filter.Name is not null)
        {
            bool isNameNumerical = int.TryParse(filter.Name, out int id);
            string s = QPiece.Contains(Field.teacher__name, filter.Name);
            if (isNameNumerical)
            {
                s = $"({s} OR {QPiece.Eq(Field.teacher__id, id)})";
            }
            q.WhereClause(s);
        }

        return q;
    }

    [HttpGet("getBriefTeacherCards")]
    public IActionResult getBriefTeacherCards(PaginationInfo paginationInfo, BriefTeacherFilter filter)
    {
        List<BriefTeacherCard> cards = [];
        QDatabase.Exec(
        conn =>
        {
            Query q = BriefTeacherPageQuery(filter);
            q.Offset(paginationInfo.CurrentPage, paginationInfo.ItemsPerPage);
            q.Select(conn, reader => cards.Add(QDataReader.GetDataObj<BriefTeacherCard>(reader)));
        });
        return PartialView(PartialList.BriefTeacherCard, cards);
    }

    [HttpGet("getBriefTeacherCards/Pagination")]
    public IActionResult getBriefTeacherCardsTotalItems(PaginationInfo paginationInfo, BriefTeacherFilter filter, string contextUrl, string contextComponent)
    {
        QDatabase.Exec(conn => paginationInfo.TotalItems = BriefTeacherPageQuery(filter).Count(conn));
        return PartialView(
            "_PaginationAjax",
            ValueTuple.Create(paginationInfo, contextUrl, contextComponent)
        );
    }
}
