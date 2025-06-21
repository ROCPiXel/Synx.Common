namespace Synx.Common.Utils;

public static class StorageUnitExchanger
{
    // ReSharper disable InconsistentNaming
    /// <summary>
    /// 存储容量单位与数值对应表
    /// </summary>
    private static readonly string[] StorageUnits = ["B", "KiB", "MiB", "GiB", "TiB", "PiB"];
    private static readonly long[] StorageBytes = [Definition.Byte, Definition.KiB, Definition.MiB, Definition.GiB, Definition.TiB, Definition.PiB];

    /// <summary>
    /// 获取存储空间单位量级
    /// e.g. 1024 Byte => 1 KiB (1024); 1048576 Byte => 1 MiB (1024*1024)
    /// 以1024为底对传入空间取对数，向下取整后再取1024的幂次方
    /// </summary>
    /// <param name="space">空间</param>
    /// <param name="from">一个传入单位对应的Byte大小</param>
    /// <returns>一个对应单位对应的Bytes大小</returns>
    public static long GetSpaceMagnitude(long space, long from = Definition.Byte)
    {
        if (space == 0 || from == 0) return 0;
        var bytes = Change(space, from, Definition.Byte);
        var type = (long)Math.Pow(1024, (int)Math.Floor(Math.Log(bytes, 1024)));
        return type;
    }
    
    /// <summary>
    /// 存储容量转换
    /// </summary>
    /// <param name="from">源单位，可以使用BasicApproaches的常量定义</param>
    /// <param name="to">目标单位，可以使用BasicApproaches的常量定义</param>
    /// <param name="space">源容量</param>
    /// <returns>目标单位对应的容量</returns>
    public static double Change(long space, long from = Definition.Byte, long to = Definition.GiB)
    {
        if (space == 0 || from == 0) return 0;
        double fromByte = space * from;
        return fromByte / to;
    }

    /// <summary>
    /// 一些常用的单位转换函数
    /// </summary>
    /// <param name="space">容量</param>
    /// <returns></returns>
    public static double GetGiBSpace(long space)
    {
        if (space == 0) return 0;
        return (double) space / Definition.GiB;
    }
    public static double GetGBSpace(long space)
    {
        if (space == 0) return 0;
        return (double) space / Definition.GB;
    }
    
    /// <summary>
    /// ChangeAuto: func
    /// 自动获取合适的存储容量单位字符串
    /// </summary>
    /// <param name="space">值</param>
    /// <param name="from">值对应的单位</param>
    /// <param name="unitBase">若切换为1000请设置新的对应表</param>
    /// <param name="format">详见string.Format()</param>
    /// <returns></returns>
    public static string ChangeAuto(long space, long from = 1, int unitBase = 1024, string format = "F")
    {
        if (space == 0) return "0B";
        long spaceByte = space * (from);
        int type = (int)Math.Floor(Math.Log(spaceByte, unitBase));
        double autoSpace = (double)spaceByte / StorageBytes[type];
        return autoSpace.ToString(format).TrimEnd('0').TrimEnd('.') + StorageUnits[type];
    }
    // ReSharper restore InconsistentNaming
    
    /// <summary>
    /// 对齐
    /// </summary>
    /// <param name="size">源大小</param>
    /// <param name="baseSize">要对齐的基数</param>
    /// <returns></returns>
    public static long Alignment(double size, int baseSize = Definition.MiB)
    {
        return baseSize * (long)Math.Ceiling(size / baseSize);
    }
}