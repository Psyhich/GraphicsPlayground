
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
var contr = new Playground.Controllers.APIController();

app.UseRouting();
app.MapGet("/", () => "Жопа");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
