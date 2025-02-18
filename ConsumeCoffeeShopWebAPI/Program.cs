using ConsumeCoffeeShopWebAPI;
using FluentValidation.AspNetCore;
using OfficeOpenXml;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddControllersWithViews()
	.AddFluentValidation(c =>
	c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<CheckAccess>();
ExcelPackage.LicenseContext = LicenseContext.Commercial;
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Property}/{action=Display}/{id?}");

app.Run();
