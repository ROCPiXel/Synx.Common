namespace Synx.Common.Utils;

public static class SpaceUnitExchange
{
    /// <summary>
    /// 存储容量单位与数值对应表
    /// </summary>
    private static readonly string[] StorageUnits = ["B", "KiB", "MiB", "GiB", "TiB", "PiB"];
    private static readonly long[] StorageBytes = [Definition.Byte, Definition.KiB, Definition.MiB, Definition.GiB, Definition.TiB, Definition.PiB];

    /// <summary>
    /// 存储容量转换
    /// </summary>
    /// <param name="from">源单位，可以使用BasicApproaches的常量定义</param>
    /// <param name="to">目标单位，可以使用BasicApproaches的常量定义</param>
    /// <param name="space">源容量</param>
    /// <returns>目标单位对应的容量</returns>
    public static double Change(long space, long from = Definition.Byte, long to = Definition.GiB)
    {
        double fromByte = space * from;
        return fromByte / to;
    }

    /// <summary>
    /// 一些常用的单位转换函数
    /// </summary>
    /// <param name="length">容量</param>
    /// <returns></returns>
    public static double GetGiBSpace(long length)
    {
        return (double) length / Definition.GiB;
    }
    public static double GetGbSpace(long length)
    {
        return (double) length / Definition.Gb;
    }
    
    /// <summary>
    /// GetSpaceAuto: func
    /// 自动获取合适的存储容量单位字符串
    /// </summary>
    /// <param name="space">值</param>
    /// <param name="from">值对应的单位</param>
    /// <param name="unitBase">若切换为1000请设置新的对应列表</param>
    /// <param name="format">详见string.Format()</param>
    /// <returns></returns>
    public static string GetSpaceAuto(long space, long from, int unitBase = 1024, string format = "F")
    {
        long spaceByte = space * from;
        int type = (int)Math.Floor(Math.Log(spaceByte, unitBase));
        double autoSpace = (double)spaceByte / StorageBytes[type];
        return autoSpace.ToString(format) + StorageUnits[type];
    }
}