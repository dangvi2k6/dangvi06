using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotorcycleShopMVC.Models;
using System.Security.Policy;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Cookie.HttpOnly = true;

        options.ExpireTimeSpan = TimeSpan.FromDays(7);

        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

//SYNC SESSION FROM COOKIE CLAIMS
app.Use(async (context, next) =>
{
    if (context.User.Identity?.IsAuthenticated == true)
    {
        var claimUserId = context.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
            .Value;

        var claimRole = context.User
            .FindFirst(System.Security.Claims.ClaimTypes.Role)?
            .Value;

        var claimEmail = context.User
            .FindFirst(System.Security.Claims.ClaimTypes.Email)?
            .Value;

        var claimName = context.User
            .FindFirst(System.Security.Claims.ClaimTypes.Name)?
            .Value;

        if (!string.IsNullOrEmpty(claimUserId))
        {
            context.Session.SetString("UserId", claimUserId);
        }

        if (!string.IsNullOrEmpty(claimRole))
        {
            context.Session.SetString("UserRole", claimRole);
        }

        if (!string.IsNullOrEmpty(claimEmail))
        {
            context.Session.SetString("UserEmail", claimEmail);
        }

        if (!string.IsNullOrEmpty(claimName))
        {
            context.Session.SetString("UserFullName", claimName);
        }
    }

    await next();
});


// Seed admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var hasher = new PasswordHasher<User>();

    //var vendor = context.Users
    //.FirstOrDefault(u => u.Email == "trandh16022005@gmail.com");

    //if (vendor != null)
    //{
    //    vendor.PasswordHash =
    //        hasher.HashPassword(vendor, "123456");

    //    context.SaveChanges();
    //}


    var adminEmail = "admin@gmail.com";

    var user = context.Users.FirstOrDefault(u => u.Email == adminEmail);

    if (user == null)
    {
        user = new User
        {
            FullName = "Admin System",
            Email = adminEmail,
            PhoneNumber = "0900000000",
            Address = "Hà Nội",
            Role = "Admin",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        user.PasswordHash = hasher.HashPassword(user, "123456");

        context.Users.Add(user);
    }
    else
    {
        user.PasswordHash = hasher.HashPassword(user, "123456");
        user.Role = "Admin";
        user.UpdatedAt = DateTime.Now;
    }

    context.SaveChanges();
}


// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();