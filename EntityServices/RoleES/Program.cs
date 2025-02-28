using Microsoft.EntityFrameworkCore;

using Common.Constants;
using Common.Lib;

using RoleES.Data;
using RoleES.Repository;
using RoleES.Repository.Interfaces;
using RoleES.Services;
using RoleES.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString(Config.DbConnection));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IRepository, Repository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
