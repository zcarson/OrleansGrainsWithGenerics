using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace Grains
{
    class HelloGrain : Grain<HelloState>, IHelloGrain
    {
        public async Task<string> SayHello(string msg)
        {
            State.NumCalls++;
            await WriteStateAsync();
            return msg;
        }
    }

    public class HelloState : GrainState
    {
        public int NumCalls { get; set; }
    }
}