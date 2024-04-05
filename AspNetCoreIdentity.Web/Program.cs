using AspNetCoreIdentity.Web.ClaimProvider;
using AspNetCoreIdentity.Web.Extentions;
using AspNetCoreIdentity.Web.Models;
using AspNetCoreIdentity.Web.OptionsModels;
using AspNetCoreIdentity.Web.Requirements;
using AspNetCoreIdentity.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

//ýdentity
builder.Services.AddIdentityWithExtentions();

builder.Services.AddScoped<IAuthorizationHandler,ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolonceExpireRequirementHandler>();

builder.Services.AddAuthorization(opt =>
{

    opt.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("City", "Ankara");
    });

    opt.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });
    opt.AddPolicy("VioloncePolicy", policy =>
    {
        policy.AddRequirements(new ViolonceRequirement() {ThresholdAge=18 });
    });


});


builder.Services.ConfigureApplicationCookie(opt => {

    var cookieBuilder =new CookieBuilder();

    cookieBuilder.Name = "UdemyAppCookie";
    opt.LoginPath = new PathString("/Home/Signin");

    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    opt.Cookie= cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;


});



builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();


builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));


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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
