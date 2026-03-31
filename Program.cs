using HaberSitesi.Interfaces;
using HaberSitesi.Repositories;
using Microsoft.EntityFrameworkCore;
using HaberSitesi.Data; 


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HaberContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("HaberPortalConnection")));
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Guvenlik/Login";
        options.AccessDeniedPath = "/Home/Index";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
