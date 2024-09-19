//using AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using NuGet.Protocol.Plugins;
using PTManagementSystem.Data;
using PTManagementSystem.Services;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

//<FrameworkReference Include = "Microsoft.AspNetCore.App" />;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("PtSystemDb") 
//    ?? throw new InvalidOperationException("Connection string 'PtSystemDb' not found.");


var connectionString = builder.Configuration["ConnectionStrings:PtSystemDb"];

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<WorkoutDAO>(provider => new WorkoutDAO(connectionString));
//builder.Services.AddScoped<WorkoutDAO>(provider => new WorkoutDAO(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<WorkoutDAO>();

builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();


app.Use((ctx, next) =>
{
    // Checks if user authorization cookie exists, if so gets it as an object
    if (ctx.Request.Cookies.TryGetValue("auth", out var authCookie))
    {
        var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
        var protector = idp.CreateProtector("auth-cookie");


        // Splits the encrypted cookie on the auth*=*usr:username_value
        var protectedPayload = authCookie.Split("=").Last();
        var payload = protector.Unprotect(protectedPayload);

        // Splits the string into two based on "usr*:*username"
        var parts = payload.Split(":");

        var key = parts[0];
        var value = parts[1];

        var claims = new List<Claim>();
        claims.Add(new Claim(key, value));
        var_identity = new ClaimsIdentity(claims);
        ctx.User = new ClaimsIdentity;

        return next();
    }
    else
    {
        throw new ArgumentException("ERROR: No cookie has been established for this user!");
    }
});


app.MapGet("/username", (HttpContext ctx) =>
    {
        return ctx.User;
    }
);


app.MapGet("/login", (AuthService auth) =>
{
    auth.SignIn();
    return "ok";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();

public class AuthService
{
    private readonly IDataProtectionProvider _idp;
    private readonly IHttpContextAccessor _accessor;

    public AuthService(IDataProtectionProvider idp, IHttpContextAccessor accessor)
    {
        _idp = idp;
        _accessor = accessor;
    }

    public void SignIn()
    {
        var protector = _idp.CreateProtector("auth-cookie");
        _accessor.HttpContext.Response.Headers["set-cookie"] = $"auth={protector.Protect("usr:anton")}";
    }
}


