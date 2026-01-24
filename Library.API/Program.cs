using Library.Application.Books;
using Library.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using Library.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Library.Domain;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opt =>
{
   var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Konfiguracja DbContext z u¿yciem SQLite
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Dodanie obs³ugi CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
    });
});

//Dodanie MediatR
builder.Services.AddMediatR(cfg =>
cfg.RegisterServicesFromAssembly(typeof(BookList.Handler).Assembly));

// Dodanie FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Rejestracja walidatorów z assembly zawieraj¹cego BookCreate
builder.Services.AddValidatorsFromAssemblyContaining<BookCreate>();

//Serwis autentykacji, rejestracji, logowania etc.
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Library.API/Program.cs
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        // Opcjonalnie: automatyczne wykonanie migracji przy starcie
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await context.Database.MigrateAsync();
        await Seed.SeedData(context, userManager, roleManager);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wyst¹pi³ b³¹d podczas migracji lub seedowania danych.");
    }
}


app.Run();
