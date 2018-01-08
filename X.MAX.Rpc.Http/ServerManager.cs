using System;

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

        public static IServiceProvider ServiceProvider { get; set; }

        public static object Invoke(RpcRequest request)
        {
            var lastIndex = request.uri.LastIndexOf(".");
            var typeFullName = request.uri.Substring(0, lastIndex);
            var type = Type.GetType(typeFullName);
            var methodName = request.uri.Substring(lastIndex + 1);

            //find interface and implement
            var obj = ServiceProvider.GetService(type);

            //invoke
            var r = type.GetMethod(methodName).Invoke(obj, request.parameters);
            return r;
        }
    }
}
