using System.Text.Json.Serialization;
using BlogApp.Data;
using BlogApp.Repository.Abstract;
using BlogApp.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("BlogApp"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserConnection, UserConnectionService>();
builder.Services.AddScoped<IBlogInteraction, BlogInteractionService>();

builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		   Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
	RequestPath = "/Uploads"
}

);
app.UseAuthorization();

app.MapControllers();

app.Run();
