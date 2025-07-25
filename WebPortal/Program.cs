using WebSite.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
CommonWeb.UrlApi = builder.Configuration[key: "Setting:UrlApi"].ToString();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute
    (
        name: "default",
        pattern: "{controller=Account}/{action=Login}/{id?}"
    );

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/favicon.ico")
    {
        context.Response.StatusCode = 204; // No Content
        return;
    }
    await next();
});

app.Run();









