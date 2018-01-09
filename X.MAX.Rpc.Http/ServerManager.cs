using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace X.MAX.Rpc.Http
{
    public class ServerManager
    {
        //#region singleton

        //private ServerManager()
        //{
        //}
        //private static object _sync;
        //private static ServerManager _instance;
        //public static ServerManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_sync)
        //            {
        //                _instance = _instance ?? new ServerManager();
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        //#endregion

        private static IServiceCollection _serviceCollection;
        private static IServiceProvider _serviceProvider;
        public static void SetServiceProvider(IServiceCollection serviceCollection, Func<IServiceCollection, IServiceProvider> serviceProviderResolver)
        {
            _serviceCollection = serviceCollection;
            _serviceProvider = serviceProviderResolver(_serviceCollection);
        }

        public static object Invoke(RpcRequest request)
        {
            var lastIndex = request.uri.LastIndexOf(".");
            var typeFullName = request.uri.Substring(0, lastIndex);
            //var type = Type.GetType(typeFullName);
            var methodName = request.uri.Substring(lastIndex + 1);

            //find interface and implement
            var serviceDescriptor = _serviceCollection.FirstOrDefault(x => x.ServiceType.FullName == typeFullName);
            var obj = _serviceProvider.GetService(serviceDescriptor.ServiceType);

            //parameters type
            var method = serviceDescriptor.ServiceType.GetMethod(methodName);
            var parameterInfos = method.GetParameters();
            var parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (request.parameters[i] == null)
                {
                    parameters[i] = null;
                    continue;
                }
                if (parameterInfos[i].ParameterType == request.parameters[i].GetType())
                {
                    parameters[i] = request.parameters[i];
                    continue;
                }
            }

            //invoke
            var r = method.Invoke(obj, parameters);
            return r;
        }
    }
}
