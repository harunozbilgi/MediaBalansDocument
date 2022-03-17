using MediaBalansDocument.Data;
using MediaBalansDocument.Respository;
using MediaBalansDocument.Services;
using MediaBalansDocument.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.Configure<DocumentSetting>(builder.Configuration.GetSection(nameof(DocumentSetting)));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
    .ConfigureWarnings(warnings =>
    {
        warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
    }));
builder.Services.AddScoped<IDocumentRepository, DocumentService>();
builder.Services.AddScoped<IProductRepository, ProductService>();

builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
