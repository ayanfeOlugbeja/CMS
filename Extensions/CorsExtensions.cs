namespace CMS.Extensions
{
    public static class CorsExtensions
    {
        public static void AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>() 
                        ?? new[] { "http://localhost:3000", "http://localhost:4200", "http://localhost:5173" };
                    
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
        }
    }
}
