using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public class RpcProxy : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
