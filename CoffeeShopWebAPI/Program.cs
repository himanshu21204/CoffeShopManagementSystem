using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Data;
using CoffeeShopWebAPI.Models;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
	.AddFluentValidation(c =>
	c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

IEnumerable<Assembly> GetAssemby(bool v)
{
	throw new NotImplementedException();
}

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<BillRepository>();
builder.Services.AddScoped<OrderDetailRepository>();
builder.Services.AddScoped<DropDownRepository>();
builder.Services.AddScoped<CityRepository>();
builder.Services.AddScoped<StateRepository>();
builder.Services.AddScoped<CountryRepository>();
builder.Services.AddScoped<DashboardRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
