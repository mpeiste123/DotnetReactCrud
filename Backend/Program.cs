using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ).ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
            .EnableDetailedErrors();
});
var app = builder.Build();

//MiddleWares

app.MapControllers();
app.Run();
