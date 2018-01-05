using System;

namespace X.MAX.Rpc.Http
{
    public class RpcRequest
    {
        public string Uri { get; set; }
        public object[] Parameters { get; set; }
    }
}
