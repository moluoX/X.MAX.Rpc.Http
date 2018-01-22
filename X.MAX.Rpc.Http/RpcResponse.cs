using System;
using System.Collections.Generic;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public class RpcResponse
    {
        public int code { get; set; }
        public string errorMessage { get; set; }
        public string errorMessageDetail { get; set; }
        public object data { get; set; }
    }
}
