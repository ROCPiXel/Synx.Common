namespace Synx.Common.Progress;

/// <summary>
/// 值(数字)进度助手
/// </summary>
public class NumericalProgressHelper : Progress<double>, INumericalProgressHelper
{
    private double _current;

    private bool _isCompletedActionInvoked = false;

    /// <summary>
    /// 当前进度
    /// </summary>
    public double Current
    { 
        get => _current; 
        set => SetCurrent(value);
    }
    /// <summary>
    /// 最大值
    /// </summary>
    public double Maximum { get; set; }

    /// <summary>
    /// 进度 通常为百分比
    /// </summary>
    public double Progress { get; private set; }

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// 完成后执行的Action
    /// </summary>
    private readonly Action? _completedAction;

    /// <summary>
    /// 新建一个值(数字)进度助手
    /// </summary>
    /// <param name="maximum">最大值</param>
    /// <param name="handler">进度变化时执行的Action</param>
    /// <param name="completedAction">结束时的Action</param>
    public NumericalProgressHelper(
        double maximum, 
        Action<double> handler, 
        Action? completedAction = null)
        : base(handler)
    {
        ProgressChanged += UpdateProgress;
        _completedAction = completedAction;
        Reset();
        Maximum = maximum;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public virtual void Reset()
    {
        Current = 0;
        Maximum = 0;
        Progress = 0;
        IsCompleted = false;
    }

    public void SetCurrent(double value)
    {
        _current = value;
        OnReport(_current);
    }

    /// <summary>
    /// 更新并计算进度
    /// </summary>
    private void UpdateProgress(object? sender, double e)
    {
        if (Maximum == 0)
        {
            Progress = 0;
            return;
        }

        Progress = (e / Maximum) * 100;

        if (e >= Maximum && !_isCompletedActionInvoked)
        {
            IsCompleted = true;
            _completedAction?.Invoke();
            _isCompletedActionInvoked = true;
        }
    }
}