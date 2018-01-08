using System;

namespace X.MAX.Rpc.Http
{
    public class RpcRequest
    {
        public string uri { get; set; }
        public object[] parameters { get; set; }
    }
}
