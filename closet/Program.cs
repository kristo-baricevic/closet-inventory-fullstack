using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Configure the static file middleware to serve static files from the "wwwroot" directory
app.UseStaticFiles();

// Configure the routes
app.MapGet("/", async (context) =>
{
    await context.Response.SendFileAsync("wwwroot/index.html");
});

app.Run();
