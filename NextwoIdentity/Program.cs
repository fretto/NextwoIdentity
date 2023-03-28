using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NextwoDbContext>(options =>//same as the previous project
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("constr"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);//i added this for tracking


});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //here we modified the password constraints
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength =5;


}).AddEntityFrameworkStores<NextwoDbContext>();//new
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
