using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Playground.Controllers;
using Playground.Data;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = WebApplication.CreateBuilder(args).Configuration.GetValue<string>("webroot") != null ? WebApplication.CreateBuilder(args).Configuration.GetValue<string>("webroot") : "website"
});

string pathToProject = Path.GetFullPath(Path.Join(builder.Environment.WebRootPath, ".."));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IProjectRepository>(
	provider => new FilesPlayground(Path.Join(pathToProject, "playgrounds"))
);
if (!builder.Environment.IsDevelopment())
{
	builder.Services
		.AddAuthentication()
		.AddGitHub(options =>
			{
				options.ClientId = Environment.GetEnvironmentVariable("GitHub:ClientId") ?? string.Empty;
				options.ClientSecret = Environment.GetEnvironmentVariable("GitHub:ClientSecret") ?? string.Empty;
			});
}

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
