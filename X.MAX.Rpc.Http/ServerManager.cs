using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace X.MAX.Rpc.Http
{
    public class ServerManager
    {
        private static IServiceCollection _serviceCollection;
        private static IServiceProvider _serviceProvider;
        public static void SetServiceProvider(IServiceCollection serviceCollection, Func<IServiceCollection, IServiceProvider> serviceProviderResolver)
        {
            _serviceCollection = serviceCollection;
            _serviceProvider = serviceProviderResolver(_serviceCollection);
        }

        public static object Invoke(string uri, string argJson)
        {
            uri = uri.Replace('-', '.');
            var lastIndex = uri.LastIndexOf(".");
            var typeFullName = uri.Substring(0, lastIndex);
            var methodName = uri.Substring(lastIndex + 1);

            //find interface and implement
            var serviceDescriptor = _serviceCollection.FirstOrDefault(x => x.ServiceType.FullName == typeFullName);
            if (serviceDescriptor == null)
                throw new Exception("未找到服务");
            var obj = _serviceProvider.GetService(serviceDescriptor.ServiceType);

            //parameters count
            var args = JArray.Parse(argJson);
            var method = serviceDescriptor.ServiceType.GetMethod(methodName);
            if (method == null)
                throw new Exception("未找到接口");
            var parameterInfos = method.GetParameters();
            if (!method.IsPublic || args.Count != parameterInfos.Length)
            {
                method = serviceDescriptor.ServiceType.GetMethods().FirstOrDefault(x => x.Name == methodName && x.IsPublic && x.GetParameters().Length == args.Count);
                parameterInfos = method.GetParameters();
            }

            //parameters type
            var parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (args[i] == null)
                {
                    parameters[i] = null;
                    continue;
                }
                //parameters[i] = JsonConvert.DeserializeObject(args[i].ToString(), parameterInfos[i].ParameterType);
                parameters[i] = args[i].ToObject(parameterInfos[i].ParameterType);
            }

            //invoke
            var r = method.Invoke(obj, parameters);
            return r;
        }
    }
}
