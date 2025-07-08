using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synx.Common.Logging;

/// <summary>
/// 提供进度变化事件的数据，包括当前值、进度百分比、最大值和完成状态。
/// </summary>
/// <remarks>
/// 此类通常用于报告长时间运行操作中的进度更新。
/// 它包含当前进度状态的信息，包括当前值、进度完成百分比、
/// 最大值以及操作是否完成。
/// </remarks>
public class ProgressChangedEventArgs : EventArgs
{
    public double Current { get; }
    public double Progress { get; }
    public double Maximum { get; }
    public bool IsCompleted { get; }
    public ProgressChangedEventArgs(double current, double progress, double maximum, bool isCompleted)
    {
        Current = current;
        Progress = progress;
        Maximum = maximum;
        IsCompleted = isCompleted;
    }
}
