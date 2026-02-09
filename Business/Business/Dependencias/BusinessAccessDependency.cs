using Business.Business.P2hBll;
using Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Dependencias
{
    public static class BusinessAccessDependency
    {
        public static IServiceCollection BusinessDependencyInjectionAccess(this IServiceCollection services)
        {

            #region [ Repository Data Access ]

            services.AddScoped<IP2hBusiness, P2hBusiness>();
            services.AddScoped<IOfimaBusiness, OfimaBusiness>();

            //
            #endregion

            return services;
        }
    }
}
