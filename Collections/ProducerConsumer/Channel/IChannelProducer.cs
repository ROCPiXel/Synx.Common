using System.Threading.Channels;

namespace Synx.Common.Collections.ProducerConsumer.Channel;

/// <summary>
/// 用于与<see cref="ProducerConsumer"/>结合的Channel生产者接口
/// </summary>
/// <typeparam name="TData">传输的数据类型</typeparam>
public interface IChannelProducer<TData> : IPcmOperator
    where TData : class, IDisposable, IReusable
{
    /// <summary>
    /// 向Channel写入数据的Writer
    /// </summary>
    ChannelWriter<TData> DispatcherWriter { get; set; }
}