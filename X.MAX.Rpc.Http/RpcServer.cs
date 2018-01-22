using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace X.MAX.Rpc.Http
{
    public class RpcServer
    {
        public static bool ShowErrorMessageDetail { get; set; }
        private static IServiceCollection _serviceCollection;
        private static IServiceProvider _serviceProvider;
        public static void SetServiceProvider(IServiceCollection serviceCollection, Func<IServiceCollection, IServiceProvider> serviceProviderResolver)
        {
            _serviceCollection = serviceCollection;
            _serviceProvider = serviceProviderResolver(_serviceCollection);
        }

        public static string Invoke(string uri, string argJson)
        {
            var rpcResponse = new RpcResponse();
            try
            {
                uri = uri.Replace('-', '.');
                var lastIndex = uri.LastIndexOf(".");
                var typeFullName = uri.Substring(0, lastIndex);
                var methodName = uri.Substring(lastIndex + 1);

                //find interface and implement
                var serviceDescriptor = _serviceCollection.FirstOrDefault(x => x.ServiceType.FullName == typeFullName);
                if (serviceDescriptor == null)
                    throw new RpcInvokeException("can not find service", 2);
                var obj = _serviceProvider.GetService(serviceDescriptor.ServiceType);

                //parameters count
                var args = JArray.Parse(argJson);
                var method = serviceDescriptor.ServiceType.GetMethod(methodName);
                if (method == null)
                    throw new RpcInvokeException("can not find interface", 3);
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
                    parameters[i] = args[i].ToObject(parameterInfos[i].ParameterType);
                }

                //invoke
                var returnObj = method.Invoke(obj, parameters);
                rpcResponse.data = returnObj;
            }
            catch (RpcInvokeException ex)
            {
                rpcResponse.code = ex.Code ?? 4;
                rpcResponse.errorMessage = ex.Message;
                rpcResponse.errorMessageDetail = ShowErrorMessageDetail ? ex.ToString() : null;
            }
            catch (Exception ex)
            {
                rpcResponse.code = 1;
                rpcResponse.errorMessage = ex.Message;
                rpcResponse.errorMessageDetail = ShowErrorMessageDetail ? ex.ToString() : null;
            }
            return JsonConvert.SerializeObject(rpcResponse);
        }
    }
}
