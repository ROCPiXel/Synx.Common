using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.Logger;

public enum LogTag
{
    INFO,
    WARNING,
    ERROR,
    FATAL,
    FOCUS
}

public class Logger
{
    public SingleFile LogFile;
    public bool UsingDefaultDatetime = true;
    public bool UsingTag = true;
    public string FocusPoint = string.Empty;

    public Logger(SingleFile logFile)
    {
        LogFile = logFile;
        logFile.Create();
    }
    public Logger(CPath logFilePath, string logName)
    {

    }
}