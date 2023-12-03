using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;

using Playground.Data;
using Playground.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = WebApplication.CreateBuilder(args).Configuration.GetValue<string>("webroot") != null ? WebApplication.CreateBuilder(args).Configuration.GetValue<string>("webroot") : "website"
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// AUTHORIZATION/AUTHENTICATION
if (!builder.Environment.IsDevelopment())
{
	builder.Services
		.AddAuthentication()
		.AddGitHub(options =>
			{
				options.ClientId = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID") ?? string.Empty;
				options.ClientSecret = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID") ?? string.Empty;
			});
}
else
{
	builder.Services.AddAuthentication().AddCookie();
}

builder.Services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ProjectAuthorizationHandler>();

// REPOSITORIES
string pathToProject = Path.GetFullPath(Path.Join(builder.Environment.WebRootPath, ".."));
builder.Services.AddSingleton<IProjectRepository>(
	provider => new ProjectsRepository(Path.Join(pathToProject, "playgrounds"))
);
builder.Services.AddSingleton<IPlaygroundUsersRepository, InMemoryUsersRepository>();

// ENABLE SWAGGER IF DEVELOPMENT
if (builder.Environment.IsDevelopment())
{
	builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// ENABLE node_modules IF DEVELOPMENT
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
    EnableDefaultFiles = false,
});
app.UseRouting();
app.MapControllers();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}"
);

app.Run();
