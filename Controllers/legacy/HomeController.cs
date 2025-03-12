using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using REPO.Models;

namespace REPO.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Dictionary<string, string>> teacher_dicts = new();
        void func(SqlDataReader reader)
        {
            Teacher teacher = DataReader.get_data_obj<Teacher>(reader);
            Dictionary<string, string> dict = teacher.to_dict();
            teacher_dicts.Add(dict);
        }

        void conn_func(SqlConnection conn)
        {
            Query q = new(Table.teacher);
            q.select(conn, func);
            for (int i = 0; i < teacher_dicts.Count; i++)
            {
                Query q2 = new(Table.teacher_subject);
                q2.join(Field.subject__id, Field.teacher_subject__sbj_id);
                q2.where_(Field.teacher_subject__tch_id, int.Parse(teacher_dicts[i]["id"]));
                q2.output(Field.subject__name);
                List<string> subjects = q2.select(conn);
                teacher_dicts[i]["subjects"] = string.Join(", ", subjects);
            }
        }

        Database.exec(conn_func);

        ViewBag.teachers = teacher_dicts;
        Console.WriteLine($"Fetched {teacher_dicts.Count} teachers");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
