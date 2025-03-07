// Các tag dùng để note bao gồm:
// INFO: Thông tin về code
// NOTE: Ghi chú, mô tả chung về code.
// TODO: Đánh dấu việc cần làm trong tương lai.
// WARN: Cảnh báo về vấn đề hoặc hậu quả tiềm ẩn.
// HACK: Giải pháp nhanh gọn nhưng không lí tưởng.
// OPTIMIZE: Code cần được tối ưu.
// BUG: Xác định bug chưa cần sửa ngay lập tức.
// FIXME: Xác định bug cần được sửa càng sớm càng tốt.

// NOTE: Đây là điểm khởi đầu của toàn bộ hệ thống web.

// NOTE: Backend là tập hợp các lệnh của lập trình viên dùng để khởi tạo cơ sở
// dữ liệu cũng như nạp các thông tin phụ khác vào hệ thống.
Backend.start();

// WARN: Từ đây trở về sau là code của hệ thống, hạn chế sửa đổi.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// NOTE: Có thể thay đổi `pattern` để thay đổi trang khởi đầu của web.
app.MapControllerRoute(
        name: "default",
        // pattern: "{controller=Home}/{action=Index}/{id?}"
        pattern: "{controller=DemoPage}/{action=Index}/{id?}"
    )
    .WithStaticAssets();

app.Run();
