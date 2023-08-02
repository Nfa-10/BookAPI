using BookAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options => options.AddPolicy("corsPolicy", policy => {

    policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();

}));

builder.Services.AddControllers();

//DbConnection
builder.Services.AddDbContext<BookAPIContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("connString")));

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

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("corsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
