using System.Threading.Tasks;
using GrainInterfaces;
using Orleans;

namespace Grains
{
    public class EchoGrain<T> : Grain<EchoState>, IEchoGrain<T>
    {
        public async Task<T> EchoAsync(T entity)
        {
            State.NumCalls++;
            await WriteStateAsync();
            return entity;
        }

        public async Task<string> EchoStringAsync(string msg)
        {
            State.NumCalls++;
            await WriteStateAsync();
            return msg;
        }
    }

    public class EchoState : GrainState
    {
        public int NumCalls { get; set; }
    }
}