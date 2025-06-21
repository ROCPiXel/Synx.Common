using System.Threading.Channels;

namespace Synx.Common.Collections.ProducerConsumer.Channel;

/// <summary>
/// Channel的复用器接口
/// </summary>
/// <typeparam name="TProducer">消费者，需继承<see cref="IChannelConsumer{T}"/></typeparam>
/// <typeparam name="TConsumer">消费者，需继承<see cref="IChannelConsumer{T}"/></typeparam>
/// <typeparam name="TData">数据类型，与<see cref="Channel{T}"/>的T相同</typeparam>
public interface IChannelDispatcher<TProducer, TConsumer, TData> 
    where TProducer : IChannelProducer<TData>
    where TConsumer : IChannelConsumer<TData>
    where TData : class, IDisposable, IReusable
{
    /// <summary>
    /// 所管理的主通道
    /// </summary>
    Channel<TData> Channel { get; set; }
    
    /// <summary>
    /// 生产者队列
    /// </summary>
    ICollection<TProducer> Producers { get; set; }
    
    /// <summary>
    /// 消费者队列
    /// </summary>
    ICollection<TConsumer> Consumers { get; set; }
    
    /// <summary>
    /// 向<see cref="Channel"/>注册生产者
    /// </summary>
    /// <param name="producer">生产者需要的<see cref="ChannelWriter{T}"/></param>
    /// <returns></returns>
    ChannelWriter<TData> RegisterProducer(TProducer producer);
    
    /// <summary>
    /// 向<see cref="Channel"/>注册消费者
    /// </summary>
    /// <param name="consumer">消费者需要的<see cref="ChannelReader{T}"/></param>
    /// <returns></returns>
    ChannelReader<TData> RegisterCustomer(TConsumer consumer);
    
    /// <summary>
    /// 取消操作的Token源 用于向<see cref="IPcmOperator"/>分发Token
    /// </summary>
    CancellationTokenSource CancellationTokenSource { get; set; }
}