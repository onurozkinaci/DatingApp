using System.Net;
using System.Text.Json;

namespace API;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;


    /*  => "RequestDelegate" has to be used while defining a middleware. It shows what is gonna happen after this middleware's part finishes. This parameter is essential, the other two are optional!
        =>"ILogger" is used to see the exception details in the terminal too.
        => "IHostEnvironment" is used to see whether we're running on development mode or production mode.*/
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware>logger, IHostEnvironment env)
    { 
        _next = next;
        _logger = logger;
        _env = env;
    }

   //=>"HttpContext context" gives us access to the HttpRequest that currently being passed through the middleware.
    public async Task InvokeAsync(HttpContext context)
    {  
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, ex.Message); //the exception will be seen on the terminal for both dev. and production mode.
        context.Response.ContentType = "application/json"; //API.Controller icerisinde olmadigimizdan biz belirtiyoruz, API.Controller icin default olarak bu tanimlama yapiliyor.
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError; //500

        var response = _env.IsDevelopment()
            ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) //ex.StackTrace? means optional in case if there is no stacktrace to prevent another exception.
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");
        
        //API.Controllers use this by default too;
        var options = new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
        var json = JsonSerializer.Serialize(response,options);
        await context.Response.WriteAsync(json);
      }
      
    }
}
