using CMS.Data;
using CMS.DTOs;
using CMS.Helpers;
using CMS.Repositories.Interfaces;
using CMS.Repositories.Services;
using CMS.Middleware;
using CMS.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Enhanced Swagger configuration
builder.Services.AddEnhancedSwagger();

// CORS configuration
builder.Services.AddCorsPolicy(builder.Configuration);

// Database
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection")
//         ?? throw new InvalidOperationException("Sorry connection not found.")
//     );
// });

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    
    // Use PostgreSQL instead of SQL Server
    options.UseNpgsql(connectionString);
});

// Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserAccount, UserAccountRepository>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

builder.Services.AddScoped<IBaseRepository<TownDto, CreateTownDto, UpdateTownDto>, TownRepository>();
builder.Services.AddScoped<IBaseRepository<BranchDto, CreateBranchDto, UpdateBranchDto>, BranchRepository>();
builder.Services.AddScoped<IBaseRepository<DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto>, DepartmentRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ISubModuleRepository, SubModuleRepository>();


builder.Services.AddScoped<IJwtServices, JwtService>();


// JWT Config
builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection("JwtSection").Get<JwtSection>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection.Issuer,
        ValidAudience = jwtSection.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!))
    };
});

// Authorization with roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Global exception handling middleware (should be first)
app.UseMiddleware<GlobalExceptionMiddleware>();

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMS API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
// }

app.UseHttpsRedirection();

// CORS
app.UseCors("FrontendPolicy");

// Order matters
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Apply any pending migrations
}
app.Run();


