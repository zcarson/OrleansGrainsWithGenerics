using System.Threading.Tasks;
using Orleans;

namespace GrainInterfaces
{
    public interface IEchoGrain<T> : IGrainWithGuidKey
    {
        Task<T> EchoAsync(T entity);

        Task<string> EchoStringAsync(string msg);
    }
}
