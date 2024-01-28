using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//=>adding the dbcontext as a service by defining the configuration for connection string;
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); 
});

builder.Services.AddCors(); //=>to provide CORS support btw client and BE apps (two different hosts). - Part 1

var app = builder.Build();

// Configure the HTTP request pipeline.

//=>to provide CORS support btw client and BE apps (two different hosts). - Part 2 - adding it to middleware part.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); 

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
