using CAG.Interfaces;
using CAG.Similarities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CAG.Utils.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddCAG(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IComputeSimilarity, CosineComputeSimilarity>();
        }
    }
}
