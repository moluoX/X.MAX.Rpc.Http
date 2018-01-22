using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public class RpcClient
    {
        private static ConcurrentDictionary<Type, object> _services = new ConcurrentDictionary<Type, object>();

        public static T GetService<T>(string serviceUrl, int? timeoutMillisecond = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(serviceUrl))
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
