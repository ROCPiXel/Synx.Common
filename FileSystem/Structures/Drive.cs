namespace Synx.Common.FileSystem.Structures;

public struct SingleDrive
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
    public double SpaceGiB { get; set; } = 0;
    public double UsedGiB { get; set; } = 0;
    public double FreeGiB { get; set; } = 0;
    public double Usage {  get; set; } = 0;
    // 附加属性
    public bool IsLocked { get; set; } = false;
    public bool IsReady { get; set; } = false;

    public DriveInfo? DriveInfo { get; set; }
    public Dictionary<string, string>? Properties { get; set; } = [];

    public SingleDrive() => FillInfo(string.Empty, string.Empty);
    public SingleDrive(CPath driveLetter) => FillInfo(driveLetter);
    public SingleDrive(string driveLetter) => FillInfo(driveLetter);

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
        FillInfo(string.Empty, driveLetter.AbsolutePath);
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

    public Dictionary<string,string> GetStringProperties()
    {
        Properties.Clear();
        Properties = new()
        {
            ["Name"] = Name,
            ["UNCPath"] = Path.GetRelativePath(),
            ["Path"] = Path.GetAbsolutePath(),
            ["DriveLetter"] = DriveLetter,
            ["FileSystem"] = FileSystem.ToString(),
            ["DriveType"] = DriveType.ToString(),
            ["TotalSpace"] = Space.ToString(),
            ["Used"] = Used.ToString(),
            ["Free"] = Free.ToString(),
            ["TotalSpaceGiB"] = SpaceGiB.ToString(),
            ["UsedGiB"] = UsedGiB.ToString(),
            ["FreeGiB"] = FreeGiB.ToString(),
            ["Usage"] = Usage.ToString(),
        };
        return Properties;
    }
}