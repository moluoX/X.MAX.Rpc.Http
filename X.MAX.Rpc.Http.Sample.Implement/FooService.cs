using System;
using X.MAX.Rpc.Http.Sample.Service;

namespace X.MAX.Rpc.Http.Sample.Implement
{
    public class FooService : IFooService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
