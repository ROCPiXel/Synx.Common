using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Synx.Common.Collections.ProducerConsumer;

/// <summary>
/// Channel的复用器
/// </summary>
/// <typeparam name="T"></typeparam>
public class ChannelDispatcher<T> : IDisposable, IDispatcher<T> where T : class, IDisposable, IReusable
{
    private bool _isDisposed = false;
    private readonly Channel<T> _originChannel;
    private readonly ConcurrentQueue<ChannelWriter<T>> _writers = new();
    private readonly Task _dispatchTask;

    public ChannelDispatcher(Channel<T> originChannel)
    {
        this._originChannel = originChannel;
        _dispatchTask = Task.Run(DispatchAsync);
    }

    public ChannelReader<T> RegisterCustomer()
    {
        var customer = Channel.CreateUnbounded<T>();
        _writers.Enqueue(customer.Writer);
        return customer.Reader;
    }
    
    public async ValueTask WriteToChannelAsync(T data)
    {
        await _originChannel.Writer.WriteAsync(data);
    }

    public async Task DispatchAsync()
    {
        await foreach (var item in _originChannel.Reader.ReadAllAsync())
        {
            for (int i = 0; i < _writers.Count; i++)
            {
                item.AddReference();
            }
            foreach(var writer in _writers)
            {
                await writer.WriteAsync(item);
            }
        }

        foreach (var writer in _writers)
        {
            writer.Complete();
        }
        
        await Task.CompletedTask;
    } 

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        
        _originChannel.Writer.Complete();
        _dispatchTask?.Wait();
        GC.SuppressFinalize(this);
    }
}