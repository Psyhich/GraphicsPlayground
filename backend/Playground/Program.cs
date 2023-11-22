using Microsoft.Extensions.FileProviders;
using Playground.Notebook;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = WebApplication.CreateBuilder(args).Configuration.GetValue<string>("webroot") != null ? WebApplication.CreateBuilder(args).Configuration.GetValue<string>("webroot") : "website"
});

string pathToProject = Path.GetFullPath(Path.Join(builder.Environment.WebRootPath, ".."));
Console.WriteLine(pathToProject);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IPlaygroundRepository>(
	provider => new FilesPlayground(Path.Join(pathToProject, "playgrounds"))
);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
    app.UseStaticFiles(new StaticFileOptions()
    {
		FileProvider = new PhysicalFileProvider(Path.Join(pathToProject, "node_modules")),
		RequestPath = new PathString("/node_modules")
    });
}

app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath)),
    RequestPath = "",
    EnableDefaultFiles = true,
    DefaultFilesOptions = { DefaultFileNames = new[] { "index.html" } }
});

app.UseRouting();
app.MapControllers();
app.Run();
