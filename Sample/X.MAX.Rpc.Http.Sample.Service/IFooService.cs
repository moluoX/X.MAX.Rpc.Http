using System;
using System.Collections.Generic;
using X.MAX.Rpc.Http.Sample.Service.Model;

namespace X.MAX.Rpc.Http.Sample.Service
{
    public interface IFooService
    {
        int Add(int a, int b);
        void Do(string a, int? b, decimal? c);
        IList<FooModel> List(FooQueryModel q);
    }
}
