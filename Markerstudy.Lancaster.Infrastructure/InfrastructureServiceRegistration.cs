 using Markerstudy.Lancaster.Application.Contracts.Infrastructure;
using Markerstudy.Lancaster.Infrastructure.Conversion;
using Markerstudy.Lancaster.Infrastructure.FileService;
using Markerstudy.Lancaster.Infrastructure.WebService;
using Microsoft.Extensions.DependencyInjection;

namespace Markerstudy.Lancaster.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ICSVService, CSVService>();
            services.AddTransient<IXMLService, XMLService>();
            services.AddTransient<IWsdlService, WsdlService>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<ILocalFileService, LocalFileService>();

            return services;
        }
    }
}
