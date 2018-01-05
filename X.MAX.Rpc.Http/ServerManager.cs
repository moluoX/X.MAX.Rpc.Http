using System;

namespace X.MAX.Rpc.Http
{
    public class ServerManager
    {
        public static IObjectContainer ObjectContainer { get; set; }

        public static object Invoke(RpcRequest request)
        {
            var lastIndex = request.Uri.LastIndexOf(".");
            var classFullName = request.Uri.Substring(0, lastIndex);
            var methodName = request.Uri.Substring(lastIndex + 1);

            //find interface and implement
            var obj = ObjectContainer.Resolve(classFullName);

            //invoke
            var r = Type.GetType(classFullName).GetMethod(methodName).Invoke(obj, request.Parameters);
            return r;
        }
    }
}
