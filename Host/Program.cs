using System;
using Orleans.Runtime.Host;
using Orleans;
using System.Net;
using GrainInterfaces;

namespace Host
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });

            GrainClient.Initialize("DevTestClientConfiguration.xml");

            // LOGIC HERE //
            // Regular grain call works
            Guid doesntMatter = Guid.NewGuid();
            var regularGrain = GrainClient.GrainFactory.GetGrain<IHelloGrain>(doesntMatter);
            var regularResult = regularGrain.SayHello("this should echo").Result;

            var grainWithString = GrainClient.GrainFactory.GetGrain<IEchoGrain<string>>(doesntMatter);
            //var stringAltResult = grainWithString.EchoStringAsync("easy echo").Result;
            var stringResult = grainWithString.EchoAsync("this should echo").Result;

            var grainWithInt = GrainClient.GrainFactory.GetGrain<IEchoGrain<int>>(doesntMatter);
            var intResult = grainWithInt.EchoAsync(54).Result;


            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

            hostDomain.DoCallBack(ShutdownSilo);
        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

        private static OrleansHostWrapper hostWrapper;
    }
}