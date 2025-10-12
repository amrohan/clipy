using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var runTimeUrl = builder.Configuration.GetConnectionString("Url");

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=notes.db"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run(runTimeUrl);
