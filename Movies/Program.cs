using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Reviews.Areas.Identity.Data;
using MoviesLibraryData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MovieContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Movies")));

builder.Services.AddDbContext<MoviesLibraryUserContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("MoviesLibraryUserContext")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<MoviesLibraryUserContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("admin"))
        await roleManager.CreateAsync(new IdentityRole("admin"));
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var email = "admin@admin.admin";
    var pass = "adminADMIN!1";
    var username = "admin@admin.admin";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser
        {
            Email = email,
            UserName = username,
            EmailConfirmed = true,
        };

        await userManager.CreateAsync(user, pass);

        await userManager.AddToRoleAsync(user, "admin");
    }
}

app.Run();