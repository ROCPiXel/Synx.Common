using Timer = System.Timers.Timer;

namespace Synx.Common.Logging;

/// <summary>
/// 带有定时器的值(数字)进度助手
/// </summary>
public class TimedValueProgressHelper : ValueProgressHelper
{
    private readonly Timer _timer;
    public int Count { get; private set; }
    public EventHandler<ProgressChangedEventArgs> ScheduledHandler { get; set; }
    public int Interval { get; init; }

    /// <summary>
    /// 新建一个带有定时器的值(数字)进度助手
    /// </summary>
    /// <param name="maximum">最大值</param>
    /// <param name="interval">定时间隔</param>
    /// <param name="handler">在进度变化时执行的Handler</param>
    /// <param name="scheduledHandler">定时执行的Handler</param>
    /// <param name="completedAction">结束时的Action</param>
    public TimedValueProgressHelper(
        double maximum, 
        int interval, 
        Action<double> handler,
        EventHandler<ProgressChangedEventArgs> scheduledHandler,
        Action? completedAction)
        : base(maximum, handler, completedAction)
    {
        Interval = interval;
        ScheduledHandler = scheduledHandler;
        _timer = new Timer(Interval);
    }

    /// <summary>
    /// 开始定时器
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Start()
    {
        if (Interval <= 0)
            throw new ArgumentOutOfRangeException(nameof(Interval), "Interval must be greater than zero.");

        _timer.Elapsed += (_, _) =>
        {
            ScheduledHandler.Invoke(this, new ProgressChangedEventArgs(Current, Progress, Maximum, IsCompleted));
            Count++;
        };
        _timer.Start();
    }

    public void Pause()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 结束定时器
    /// </summary>
    /// <remarks>
    /// 注意：此方法不会重置计数器，不会强制标记完成
    /// </remarks>
    public void Stop()
    {
        _timer.Stop();
    }
}