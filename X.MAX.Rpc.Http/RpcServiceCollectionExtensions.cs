using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.MAX.Rpc.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RpcServiceCollectionExtensions
    {
        public static IServiceCollection AddRpc(this IServiceCollection services, Func<IServiceCollection, IServiceProvider> serviceProviderResolver, string serviceAssemblyRegexStr = @"^[\w\.]*\.service$", string implementAssemblyRegexStr = @"^[\w\.]*\.implement$")
        {
            //auto register services
            if (!string.IsNullOrWhiteSpace(serviceAssemblyRegexStr) && !string.IsNullOrWhiteSpace(implementAssemblyRegexStr))
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                var filePaths = Directory.EnumerateFiles(directory, "*.dll");
                var serviceAssemblyRegex = new Regex(serviceAssemblyRegexStr, RegexOptions.IgnoreCase);
                var implementAssemblyRegex = new Regex(implementAssemblyRegexStr, RegexOptions.IgnoreCase);

                //load assemblies to appdomain by regex
                foreach (var filePath in filePaths)
                {
                    var assemblyName = AssemblyName.GetAssemblyName(filePath);
                    if (!serviceAssemblyRegex.IsMatch(assemblyName.Name)
                        && !implementAssemblyRegex.IsMatch(assemblyName.Name))
                        continue;
                    if (AppDomain.CurrentDomain.GetAssemblies().Any(x => x.GetName() == assemblyName))
                        continue;
                    AppDomain.CurrentDomain.Load(assemblyName);
                }

                //get services
                var serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => serviceAssemblyRegex.IsMatch(x.GetName().Name))
                    .SelectMany(x => x.GetTypes().Where(y => y.IsInterface && y.IsPublic));
                foreach (var serviceType in serviceTypes)
                {
                    var implements = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => implementAssemblyRegex.IsMatch(x.GetName().Name))
                        .SelectMany(x => x.GetTypes().Where(y => y.IsPublic && y.IsClass && y.GetInterface(serviceType.FullName) != null));

                    if (implements.Any() && !services.Any(x => x.ServiceType == serviceType))
                        services.AddTransient(serviceType, implements.First());
                }
            }

            RpcServer.SetServiceProvider(services, serviceProviderResolver);
            return services;
        }
    }
}
