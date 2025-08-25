using Synx.Common.Progress.EventArgs;
using Synx.Common.Progress.Handler;
using Timer = System.Timers.Timer;

namespace Synx.Common.Progress;

/// <summary>
/// 带有定时器的值(数字)进度助手
/// </summary>
public class TimedNumericalProgressHelper : NumericalProgressHelper
{
    private Timer _timer;
    private double _lastProgress;
    public int Count { get; private set; }
    public TimedNumericalProgressChangedEventHandler ScheduledHandler { get; set; }
    public int Interval { get; init; }
    public double Speed { get; private set; }

    /// <summary>
    /// 新建一个带有定时器的值(数字)进度助手
    /// </summary>
    /// <param name="maximum">最大值</param>
    /// <param name="interval">定时间隔</param>
    /// <param name="handler">在进度变化时执行的Handler</param>
    /// <param name="scheduledHandler">定时执行的Handler</param>
    /// <param name="completedAction">结束时的Action</param>
    public TimedNumericalProgressHelper(
        double maximum, 
        int interval, 
        Action<double> handler,
        TimedNumericalProgressChangedEventHandler scheduledHandler,
        Action? completedAction)
        : base(maximum, handler, completedAction)
    {
        if (interval <= 0)
            throw new ArgumentOutOfRangeException(nameof(Interval), "Interval must be greater than zero.");
        
        Interval = interval;
        ScheduledHandler = scheduledHandler;
        _timer = new Timer(Interval);
    }

    /// <summary>
    /// 重置计数相关属性
    /// </summary>
    public override void Reset()
    {
        base.Reset();
        Speed = 0;
        Count = 0;
        _lastProgress = 0;
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    public void ResetTimer()
    {
        _timer.Stop();
        _timer.Close();
        _timer = new Timer(Interval);
    }

    /// <summary>
    /// 开始定时器
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Start()
    {
        _timer.Elapsed += Elapsed;
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
    
    /// <summary>
    /// 计时器触发时执行的方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        Count++;
            
        if (Maximum == 0 || Interval <= 0) 
            Speed = 0;
        else
            Speed = Current - _lastProgress;
            
        ScheduledHandler.Invoke(
            this, 
            new TimedNumericalProgressChangedEventArgs(Current, Progress, Maximum, IsCompleted, Interval, Count, Speed));
        _lastProgress = Current;
    }
}