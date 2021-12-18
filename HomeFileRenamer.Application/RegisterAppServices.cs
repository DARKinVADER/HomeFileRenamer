using HomeFileRenamer.Application.Services;
using HomeFileRenamer.Domain.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace HomeFileRenamer.Application
{
    public static class RegisterAppServices
    {
        public static void AddAppServices(this IServiceCollection service)
        {
            service.AddScoped<IFileService, FileService>();
        }
    }
}
