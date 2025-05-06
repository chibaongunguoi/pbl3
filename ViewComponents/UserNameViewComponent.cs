using Microsoft.AspNetCore.Mvc;

public class UserNameViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        string username = HttpContext.User.Identity?.Name ?? string.Empty;
        User? user = QDatabase.Exec(conn => AccountQuery.GetUser(conn, username));
        return await Task.FromResult<IViewComponentResult>(Content(user?.Name ?? string.Empty));
    }
}