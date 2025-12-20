using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;



var MyAllowSpecificOrigins = "_myAllowSpcificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
                           .AllowAnyMethod()
                           .AllowAnyHeader();// g X-Pagination
    });
});
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
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
app.Run();
