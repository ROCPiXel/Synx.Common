using System.Threading.Channels;

namespace Synx.Common.Collections.ProducerConsumer;

public interface IDispatcher<T> where T : class, IDisposable, IReusable
{
    ChannelReader<T> RegisterCustomer();
    
    ValueTask WriteToChannelAsync(T data);
    
    Task DispatchAsync();
    
    void Dispose();
}