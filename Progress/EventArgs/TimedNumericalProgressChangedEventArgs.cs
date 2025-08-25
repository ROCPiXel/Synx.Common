namespace Synx.Common.Progress.EventArgs;

/// <summary>
/// 继承: <see cref="NumericalProgressChangedEventArgs"/>
/// 提供计时进度助手的间隔与计数
/// </summary>
public class TimedNumericalProgressChangedEventArgs : NumericalProgressChangedEventArgs
{
    public readonly int Interval;
    public readonly int Count;
    public readonly double Speed;
    
    public TimedNumericalProgressChangedEventArgs(
        double current, double progress, double maximum, bool isCompleted,
        int interval, int count, double speed)
        : base(current, progress, maximum, isCompleted)
    {
        Interval = interval;
        Count = count;
        Speed = speed;
    }
}