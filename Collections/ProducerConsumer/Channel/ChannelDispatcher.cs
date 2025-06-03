using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Synx.Common.Collections.ProducerConsumer.Channel;

/// <summary>
/// Channel的复用器
/// </summary>
/// <typeparam name="TProducer">消费者，需继承<see cref="IChannelConsumer{T}"/></typeparam>
/// <typeparam name="TConsumer">消费者，需继承<see cref="IChannelConsumer{T}"/></typeparam>
/// <typeparam name="TData">数据类型，与<see cref="Channel{T}"/>的T相同</typeparam>
public class ChannelDispatcher<TProducer, TConsumer, TData> : IChannelDispatcher<TProducer, TConsumer, TData> 
    where TProducer : IChannelProducer<TData>
    where TConsumer : IChannelConsumer<TData>
    where TData : class, IDisposable, IReusable
{
    private bool _isDisposed;
    private ICollection<TProducer> _producers = new List<TProducer>();
    private ICollection<TConsumer> _consumers = new List<TConsumer>();

    public CancellationTokenSource CancellationTokenSource { get; set; }
    
    public Channel<TData> Channel { get; init; }

    /// <summary>
    /// 生产者队列
    /// 修改时会清空原有列表
    /// </summary>
    public ICollection<TProducer> Producers
    {
        get => _producers;
        set => SetProducers(value);
    }

    /// <summary>
    /// 消费者队列
    /// 修改时会清空原有列表
    /// </summary>
    public ICollection<TConsumer> Consumers
    {
        get => _consumers;
        set => SetConsumers(value);
    }

    private void SetProducers(ICollection<TProducer> producers)
    {
        _producers.Clear();
        foreach (var producer in producers)
        {
            RegisterProducer(producer);
        }
    }
    
    private void SetConsumers(ICollection<TConsumer> consumers)
    {
        _consumers.Clear();
        foreach (var consumer in consumers)
        {
            RegisterCustomer(consumer);
        }
    }
    
    protected ChannelDispatcher(){}
    
    public ChannelDispatcher(Channel<TData> originChannel)
    {
        Channel = originChannel;
    }
    
    public ChannelWriter<TData> RegisterProducer(TProducer producer)
    {
        producer.DispatcherWriter = Channel.Writer;
        Producers.Add(producer);
        return Channel.Writer;
    }

    public ChannelReader<TData> RegisterCustomer(TConsumer consumer)
    {
        consumer.DispatcherReader = Channel.Reader;
        Consumers.Add(consumer);
        return Channel.Reader;
    }
}