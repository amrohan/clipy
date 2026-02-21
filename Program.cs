using clipy.Hubs;
using clipy.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=clipy.db"));

builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddSingleton<IRoomManager, RoomManager>();
builder.Services.AddSingleton<IRoomCleanupService, RoomCleanupService>();
builder.Services.AddHostedService(p =>
    (RoomCleanupService)p.GetRequiredService<IRoomCleanupService>());


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); 

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages().WithStaticAssets();
app.MapHub<RoomHub>("/roomHub");

app.Run();