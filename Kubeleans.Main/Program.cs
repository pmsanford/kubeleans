using System;
using Kubeleans.Silo;

namespace Kubeleans.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var silo = new WorkerSilo();
            silo.StartSilo().Wait();
            Console.ReadLine();
        }
    }
}
