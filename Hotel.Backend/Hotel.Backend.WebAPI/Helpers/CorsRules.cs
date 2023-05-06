namespace Hotel.Backend.WebAPI.Helpers;

public static class CorsRules
{
    public static IServiceCollection AddCorsRules(this IServiceCollection services)
    {
        services.AddCors(options => options
                .AddDefaultPolicy(policyConfig => policyConfig
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()));

                //TODO manage origin
                //.WithOrigins("http://localhost:4200")))

        return services;
    }
}
