using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseStaticFiles(new StaticFileOptions()
    {
		FileProvider = new PhysicalFileProvider(Path.Join(Directory.GetCurrentDirectory(), "..", "..", "node_modules")),
		RequestPath = new PathString("/node_modules")
    });
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Join(Directory.GetCurrentDirectory(), "..", "..", "wwwroot")),
    RequestPath = new PathString("")
});

app.UseRouting();
app.MapControllers();
app.Run();
