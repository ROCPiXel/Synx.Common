using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Structures;

[Serializable]
public class SingleDrive
{
    // 常规与路径
    public string Name { get; set; } = string.Empty;
    public CPath Path { get; set; }
    public string DriveLetter { get; set; } = string.Empty;
    
    // 以下：可空属性，磁盘信息不可为空，但可以延迟获取属性
    public string? FileSystem { get; set; }
    public DriveType? DriveType { get; set; }
    // 容量
    public long Space { get; set; } = 0;
    public long Used { get; set; } = 0;
    public long Free { get; set; } = 0;
    public double SpaceGiB { get; private set; }
    public double UsedGiB { get; private set; }
    public double FreeGiB { get; private set; }
    public double Usage { get; private set; }
    // 附加属性
    // public bool IsLocked { get; set; } = false;
    public bool IsReady { get; set; }

    public DriveInfo? DriveInfo { get; set; }
    // public Dictionary<string, string>? Properties { get; set; } = [];

    public SingleDrive() { }
    public SingleDrive(CPath driveLetter) => FillInfo(driveLetter);
    public SingleDrive(string driveLetter) => FillInfo(driveLetter);

    /// <summary>
    /// 计算空间相关属性
    /// </summary>
    public void CalculateSpace()
    {
        SpaceGiB = SpaceUnitExchanger.GetGiBSpace(Space);
        Used = Space - Free;
        UsedGiB = SpaceUnitExchanger.GetGiBSpace(Used);
        FreeGiB = SpaceUnitExchanger.GetGiBSpace(Free);
        Usage = (double)Used / Space;
    }

    /// <summary>
    /// 获取信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="driveLetter"></param>
    public void FillInfo(string name, string driveLetter)
    {
        Name = name;
        DriveLetter = driveLetter;
        Path = new(driveLetter);
    }
    
    public void FillInfo(string driveLetter)
    {
        FillInfo(string.Empty, driveLetter);
    }

    public void FillInfo(CPath driveLetter)
    {
        FillInfo(string.Empty, driveLetter.Absolute);
    }

    // TODO
    public SingleDrive GetInfo()
    {
        return this;
    }

    public SingleDrive Refresh()
    {
        throw new NotImplementedException();
    }
}