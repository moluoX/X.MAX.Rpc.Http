using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public class RpcClient
    {
        private static ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();

        public static string DefaultServiceUrl { get; set; }

        public static T GetService<T>(string serviceUrl = null, int? timeoutMillisecond = null) where T : class
        {
            var url = string.IsNullOrWhiteSpace(serviceUrl) ? DefaultServiceUrl : serviceUrl;
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("ServiceUrl");

            if (!_services.ContainsKey(typeof(T)))
            {
                var proxy = RpcProxy.Create<T, RpcProxy>();
                typeof(RpcProxy).GetProperty("ServiceUrl").SetValue(proxy, serviceUrl);
                if (timeoutMillisecond > 0)
                    typeof(RpcProxy).GetProperty("TimeoutMillisecond").SetValue(proxy, timeoutMillisecond);
                _services.TryAdd(typeof(T), proxy);
            }
            return _services[typeof(T)] as T;
        }
    }
}
