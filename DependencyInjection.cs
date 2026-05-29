// Example: Register in Startup.cs ConfigureServices or Program.cs (ASP.NET Core 6+)

// For ASP.NET Core 6+:
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// For older versions in Startup.cs:
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddControllers();
        services.AddAuthorization();
    }
}
