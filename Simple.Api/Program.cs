using Simple.Service.Interfaces;
using Simple.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Simple.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();

var connStr = builder.Configuration.GetConnectionString(nameof(SimpleDataDbContext));

builder.Services.AddPooledDbContextFactory<SimpleDataDbContext>(delegate (DbContextOptionsBuilder options)
{
    options.UseSqlServer(connStr);
    options.EnableDetailedErrors();
});
builder.Services.AddDbContext<SimpleDataDbContext>(delegate (DbContextOptionsBuilder options)
{
    options.UseSqlServer(connStr);
    options.EnableDetailedErrors();
}, ServiceLifetime.Scoped);

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
