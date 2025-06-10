using System.Threading.Channels;

namespace Synx.Common.Collections.ProducerConsumer.Channel;

/// <summary>
/// 用于与<see cref="ProducerConsumer"/>结合的Channel消费者接口
/// </summary>
/// <typeparam name="TData">传输的数据类型</typeparam>
public interface IChannelConsumer<TData> : IPcmOperator
    where TData : class, IDisposable, IReusable
{
    /// <summary>
    /// 从Channel读取数据的Reader
    /// </summary>
    ChannelReader<TData> DispatcherReader { get; set; }
}