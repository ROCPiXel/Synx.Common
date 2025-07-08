namespace Synx.Common.Logging;

public interface IValueProgressHelper : IProgress<double>
{
    /// <summary>
    /// 当前的进度
    /// </summary>
    double Current { get; }
    
    /// <summary>
    /// 总量
    /// </summary>
    double Maximum { get; set; }
    
    /// <summary>
    /// 进度
    /// </summary>
    double Progress { get; }
    
    /// <summary>
    /// 是否完成
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// 重置
    /// </summary>
    void Reset();
}