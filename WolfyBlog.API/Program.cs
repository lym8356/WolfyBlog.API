using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WolfyBlog.API.Database;
using WolfyBlog.API.Entities;
using WolfyBlog.API.Services;
using WolfyBlog.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    o.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddSignInManager<SignInManager<AppUser>>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// header services
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Add repo services to container
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<IAlbumRepository, AlbumRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<ISiteLogRepository, SiteLogRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IAboutPageRepository, AboutPageRepository>();

// Add jwt token service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWTSettings:Audience"],

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ImageService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "http://127.0.0.1:5173");
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
} catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}


await app.RunAsync();

