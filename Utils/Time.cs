using System;
using Synx.Common.Enums;

namespace Synx.Common.Utils;

public static class TimeOperation
{
    public static string DateTimeToString(DateTime dt)
    {
        return dt.ToString();
    }

    /// <summary>
    /// TimeSpanToString - #1Reloads: Func
    /// 用自然语言表述时间段
    /// </summary>
    /// <param name="ts">传入的TimeSpan</param>
    /// <param name="type">详见Definitions.TimeToStringType</param>
    /// <returns>时间段对应的自然语言表述: string</returns>
    public static string TimeSpanToString(TimeSpan ts, TimeToStringType type)
    {
        switch (type)
        {
            case TimeToStringType.Second:
                return ts.TotalSeconds < Definition.SecondsPerMinute ? "刚刚" : $"{(int)ts.TotalMinutes}分钟前";
            case TimeToStringType.Minute:
                return ts.TotalSeconds < Definition.SecondsPerMinute ? "一分钟内" : $"{(int)ts.TotalMinutes}分钟前";
            case TimeToStringType.Hour:
                return ts.TotalMinutes < Definition.SecondsPerMinute ? "一小时内" : $"{(int)ts.TotalHours}小时前";
            case TimeToStringType.Day:
                return ts.TotalHours < Definition.HoursPerDay ? "一天以内" : $"{(int)ts.TotalDays}天前";
            case TimeToStringType.Week:
                return ts.TotalDays < Definition.DaysPerWeek ? "一周内" : $"{(int)(ts.TotalDays / 7)}周前";
            case TimeToStringType.Month:
                return ts.TotalDays < Definition.DaysPerMonth ? "一个月内" : $"{(int)(ts.TotalDays / 30)}个月前";
            case TimeToStringType.Year:
                return ts.TotalDays < Definition.DaysPerYear ? "一年内" : $"{(int)(ts.TotalDays / 365)}年前";
        }
        return string.Empty;
    }

    /// <summary>
    /// TimeSpanToString - #2Reloads: Func
    /// 自动选择调用何种单位
    /// </summary>
    /// <param name="ts">TimeSpan</param>
    /// <returns>时间段对应的自然语言表述: string</returns>
    public static string TimeSpanToString(TimeSpan ts)
    {
        if (ts.TotalMinutes >= 1)
        {
            if (ts.TotalHours >= 1)
            {
                if (ts.TotalDays >= 1)
                {
                    if (ts.TotalDays >= 7)
                    {
                        if (ts.TotalDays >= 30)
                        {
                            if (ts.TotalDays >= 365)
                            {
                                return TimeSpanToString(ts, TimeToStringType.Year);
                            }
                            return TimeSpanToString(ts, TimeToStringType.Month);
                        }
                        return TimeSpanToString(ts, TimeToStringType.Week);
                    }
                    return TimeSpanToString(ts, TimeToStringType.Day);
                }
                return TimeSpanToString(ts, TimeToStringType.Hour);
            }
            return TimeSpanToString(ts, TimeToStringType.Minute);
        }
        return "刚刚";
    }

    /// <summary>
    /// GetMillisecondTimeStamp: func
    /// 获取精确到毫秒(Millisecond)级的时间戳
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static double GetMillisecondTimeStamp(DateTime dt)
    {
        return (dt.ToUniversalTime().Ticks - Definition.TickBegin) / 10000;
    }

    /// <summary>
    /// GetTimeStamp: func
    /// 获取精确到秒级的时间戳
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static int GetTimeStamp(DateTime dt)
    {
        return (int)(dt.ToUniversalTime().Ticks - Definition.TickBegin) / 10000000;
    }

    /// <summary>
    /// TimeStampToTimeSpan: func
    /// 将时间戳（精确到秒）转换为DateTime
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime TimeStampToTimeSpan(long dt)
    {
        // 分两步：先转换为本地时区，后加秒
        return TimeZoneInfo.ConvertTime(Definition.DateBegin, TimeZoneInfo.Local).AddSeconds(dt);
    }
}