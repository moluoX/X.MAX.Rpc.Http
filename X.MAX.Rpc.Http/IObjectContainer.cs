using System;
using System.Collections.Generic;
using System.Text;

namespace X.MAX.Rpc.Http
{
    public interface IObjectContainer
    {
        object Resolve(string fullName);
    }
}
