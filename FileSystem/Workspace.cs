using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem;

public class Workspace
{
    // 来源文件与目标文件及相关文件流
    public CPath sourceFilePath { get; set; }
    public CPath targetFilePath { get; set; }
    private FileStream SourceFileStream { get; set; }
    private FileStream TargetFileStream { get; set; }
    // 文件指针
    public int Pointer { get; set; }
    // 缓冲区
    public int BufferSize { get; set; }
    public char[] Buffer { get; set; }
    // 状态
    public bool IsRunning { get; set; } = false;
    public bool IsBegined { get; set; } = false;
    public bool IsCompleted { get; set; } = false;
    public bool IsCancelled { get; set; } = false;
    public bool IsPaused { get; set; } = false;
    public Exception ErrException { get; }
    public double TotalSize { get; }
    public double CompletedSize { get; }
    public double Progress { get; }

    public Workspace()
    {

    }
}