namespace Synx.Common;

/// <summary>
/// Definition: Class
/// 定义最基础的常量
/// </summary>
public static class Definition
{
    // 一些常用存储容量关于Byte关系的常量，KiB = 1024 Bytes, KB = 1000 Bytes

    public const int Byte = 1;

    public const long KiB = 1024 * Byte;
    public const long MiB = 1024 * KiB;
    public const long GiB = 1024 * MiB;
    public const long TiB = 1024 * GiB;
    public const long PiB = 1024 * GiB;

    public const long Kb = 1000 * Byte;
    public const long Mb = 1000 * Kb;
    public const long Gb = 1000 * Mb;
    public const long Tb = 1000 * Gb;
    public const long Pb = 1000 * Gb;

    public const string DefaultSuffix = "_new";
    public const string LoggerFolder = @"\__Log\";
    public const string FileOpTestFolder = "\\__FileOpTest\\";

    // 以下常量定义来源于TimeSpan(System.Runtime)
    // 用于描述对应时间段内的Tick数, 1 Tick含有 100 Nanosecond
    // Tick/Microsecond = 10, Tick/Second = 10^7;
    // Second > (x1000) Millisecond > (x1000) MicroSecond > (x1000) Nanosecond

    /// <summary>
    /// 每个Tick所含有的ns纳秒
    /// </summary>
    public const long NanosecondsPerTick = 100; 

    public const long TicksPerMicrosecond = 10; // 微秒
    public const long TicksPerMillisecond = 10000; // ms毫秒
    public const long TicksPerSecond = 10000000;
    public const long TicksPerMinute = 600000000;
    public const long TicksPerHour = 36000000000;
    public const long TicksPerDay = 864000000000;

    public const long TicksPerWeek = 6048000000000;
    public const long TicksPerYear = 315360000000000;
    public const int SecondsPerMinute = 60;
    public const int MinutesPerHour = 60;
    public const int HoursPerDay = 24;
    public const int DaysPerWeek = 7;
    public const int DaysPerMonth = 30;
    public const int MonthsPerYear = 12;
    public const int DaysPerYear = 365;
    public const int DaysPerLeapYear = 366; // 闰年366天

    /// <summary>
    /// 格林威治时间1970年01月01日00时00分00秒
    /// </summary>
    public static DateTime DateBegin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    /// <summary>
    /// 格林威治时间1970年01月01日00时00分00秒所代表的Tick数
    /// </summary>
    public const long TickBegin = 621355968000000000;
    public const long TimeStampBegin = 0;


    // 文件相关的定义
    /// <summary>
    /// 最大扫描深度
    /// </summary>
    public const int DirectoryScanningMaxDepth = 1024;
}