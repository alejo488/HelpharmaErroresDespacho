namespace ErrorDespacho.DependencyContainer
{
    using Business.Dependencias;
    using Data.Dependences;

    public static class DependencyContainer
    {
        public static IServiceCollection DependencyInjection(this IServiceCollection services)
        {
            services.BusinessDependencyInjectionAccess();
            services.DataDependencyInjectionAccess();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .WithOrigins("*"));
            });

            return services;
        }
    }
}
