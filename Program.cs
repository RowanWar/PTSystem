
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTManagementSystem;
using NuGet.Packaging;
using NuGet.Protocol.Plugins;
using PTManagementSystem.Database;
using PTManagementSystem.Extensions;
using PTManagementSystem.Services;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);



//var connectionString = builder.Configuration["ConnectionStrings:PtSystemDb"];

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(connectionString));


//builder.Services.AddSingleton<WorkoutDAO>(provider => new WorkoutDAO(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();



// Working on

//builder.Services.AddAuthentication("cookie")
//    .AddCookie("cookie");


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();
//builder.Services.AddAuthentication(options =>
//    {
//        .options.DefaultScheme = IdentityConstants.ApplicationScheme;
//        .options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//    }).AddCookie(IdentityConstants.ApplicationScheme).AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthentication(options =>

{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

}).AddCookie(IdentityConstants.ApplicationScheme).AddBearerToken(IdentityConstants.BearerScheme);



builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.AddSingleton<WorkoutDAO>(provider => new WorkoutDAO(connectionString));


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
{
    string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    return await context.Users.FindAsync(userId);
}).RequireAuthorization();


app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapIdentityApi<User>();

//app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseHttpsRedirection();





app.Run();

//
