using API;
using API.Data;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//**=>In order to call the app related services on the extension metot that we defined in it's specific class;
builder.Services.AddApplicationServices(builder.Configuration);
//**=>In order to call the identity related services on the extension metot that we defined in it's specific class;
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); //=>to use the specifically defined exception middleware.

//=>to provide CORS support btw client and BE apps (two different hosts). - Part 2 - adding it to middleware part.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); 

//=>In order to check whether the user passes authentication and authorization to reach the endpoints;
app.UseAuthentication(); //=>Asks if the visitor/client has a valid token or not. - kimlik dogrulama
app.UseAuthorization(); //=>If you have a valid token then what are you allowed to do? - yetkinlik/yetki sorgulama

app.MapControllers();

//--------=>Db'yi seed data ile guncellemek icin; --------
//=>"app.MapControllers()" sonrasinda ve "app.Run()" oncesinde tanimlanmasi onemli;
using var scope = app.Services.CreateScope(); //this gives us access to all of the exist services inside this class.
var services = scope.ServiceProvider;
try
{
  var context = services.GetRequiredService<DataContext>();
  await context.Database.MigrateAsync();
  await Seed.SeedUsers(context);
}
catch(Exception ex)
{
   var logger = services.GetService<ILogger<Program>>();
   logger.LogError(ex, "An error occured during migration!");
}
//--------------------------------------------------------

app.Run();
