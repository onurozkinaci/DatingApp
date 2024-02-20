using API;
using API.Extensions;

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

app.Run();
