using System;
using X.MAX.Rpc.Http.Sample.Contract;

namespace X.MAX.Rpc.Http.Sample.Service
{
    public class FooService : IFooService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
