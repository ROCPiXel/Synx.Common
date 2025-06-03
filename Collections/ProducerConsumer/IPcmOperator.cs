namespace Synx.Common.Collections.ProducerConsumer;

/// <summary>
/// 生产者消费者模型中的操作单元（生产者或消费者）接口
/// PCM: Producer Consumer Model
/// </summary>
public interface IPcmOperator
{
    /// <summary>
    /// 用于协调或控制生产消费者的共享信号
    /// </summary>
    SemaphoreSlim Semaphore { get; init; }
    
    /// <summary>
    /// 取消操作的Token
    /// </summary>
    CancellationToken CancellationToken { get; set; }
}