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

var wwwrootPath = Path.Join(Directory.GetCurrentDirectory(), "..", "..", "wwwroot");

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(wwwrootPath),
    RequestPath = new PathString("")
});

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"..\..\wwwroot")),
    RequestPath = "",
    EnableDefaultFiles = true,
    DefaultFilesOptions = { DefaultFileNames = new[] { "index.html" } }
});

app.UseRouting();
app.MapControllers();
app.Run();
