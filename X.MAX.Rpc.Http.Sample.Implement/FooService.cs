using System;
using System.Collections.Generic;
using X.MAX.Rpc.Http.Sample.Service;
using X.MAX.Rpc.Http.Sample.Service.Model;

namespace X.MAX.Rpc.Http.Sample.Implement
{
    public class FooService : IFooService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public void Do(string a, int? b, decimal? c)
        {
            return;
        }

        public IList<FooModel> List(FooQueryModel q)
        {
            return new List<FooModel>
            {
                new FooModel { Id = 1, Name = "Alice", Money = 1111.11m },
                new FooModel { Id = 2, Name = "Bio", Money = 2222.22m },
                new FooModel { Id = 3, Name = "Cow", Money = 2333m }
            };
        }
    }
}
