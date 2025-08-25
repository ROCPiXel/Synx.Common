using Synx.Common.Progress.EventArgs;

namespace Synx.Common.Progress.Handler;

/// <summary>
/// <see cref="TimedNumericalProgressHelper"/>的定时时间处理程序
/// </summary>
public delegate void TimedNumericalProgressChangedEventHandler(object? sender, TimedNumericalProgressChangedEventArgs e);