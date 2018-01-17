using System;
using X.MAX.Rpc.Http.Sample.Service;

namespace X.MAX.Rpc.Http.Sample.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var service = RpcClient.GetService<IFooService>();
            var r = service.Add(1, 2);
            Console.WriteLine(r);

            Console.ReadLine();
        }
    }
}
