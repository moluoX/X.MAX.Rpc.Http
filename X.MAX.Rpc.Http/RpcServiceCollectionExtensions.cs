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
        public static IServiceCollection AddRpc(this IServiceCollection services, Func<IServiceCollection, IServiceProvider> func, string serviceAssemblyRegexStr = @"^[\w\.]*\.service\.*(dll|exe)$", string implementAssemblyRegexStr = @"^[\w\.]*\.implement\.*(dll|exe)$")
        {
            //auto register services
            if (!string.IsNullOrWhiteSpace(serviceAssemblyRegexStr) && !string.IsNullOrWhiteSpace(implementAssemblyRegexStr))
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                var filePaths = Directory.EnumerateFiles(directory);

                //get services
                var serviceAssemblyRegex = new Regex(serviceAssemblyRegexStr, RegexOptions.IgnoreCase);
                var serviceAssemblyPaths = filePaths.Where(x => serviceAssemblyRegex.IsMatch(Path.GetFileName(x)));
                var serviceTypes = serviceAssemblyPaths.SelectMany(x => Assembly.Load(File.ReadAllBytes(x)).GetTypes().Where(y => y.IsInterface && y.IsPublic));
                //var serviceAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => serviceAssemblyRegex.IsMatch(x.FullName));
                //var serviceTypes = serviceAssemblies.SelectMany(x => x.GetTypes().Where(y => y.IsInterface && y.IsPublic));

                //get implements and register
                var implementAssemblyRegex = new Regex(implementAssemblyRegexStr, RegexOptions.IgnoreCase);
                var implementAssemblyPaths = filePaths.Where(x => implementAssemblyRegex.IsMatch(Path.GetFileName(x)));
                //var implementAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => implementAssemblyRegex.IsMatch(x.FullName));
                foreach (var serviceType in serviceTypes)
                {
                    var implements = implementAssemblyPaths.SelectMany(x => Assembly.Load(File.ReadAllBytes(x)).GetTypes().Where(y => y.IsPublic && y.IsClass && y.GetInterface(serviceType.FullName) != null));

                    if (implements.Any())
                        services.AddTransient(serviceType, implements.First());
                }
            }

            ServerManager.ServiceProvider = func(services);
            return services;
        }
    }
}
