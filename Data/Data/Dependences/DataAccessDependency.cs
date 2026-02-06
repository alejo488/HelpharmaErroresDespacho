using Data.Interfaces;
using Data.OfService;
using Data.P2HServices;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Dependences
{
    public static class DataAccessDependency
    {
        public static IServiceCollection DataDependencyInjectionAccess(this IServiceCollection services)
        {

            #region [ Repository Data Access ]

            services.AddScoped<IOfimaServices, OfimaServices>();
            services.AddScoped<IP2hService, P2hService>();
            //
            #endregion


            #region [General]
            //services.AddScoped<AppDbContext>();
            #endregion

            return services;
        }
    }
}
