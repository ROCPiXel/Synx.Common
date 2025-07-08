using System.Diagnostics;
using System.Reflection;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Providers.Drive;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Helpers;

/// <summary>
/// <see cref="SingleDrive"/>的单例管理器
/// </summary>
public class DriveManager
{
    private static volatile DriveManager? _instance = null;
    private static object _instanceLock = new object();
    /// <summary>
    /// DriveManager Singleton模式实现
    /// </summary>
    public static DriveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new DriveManager();
                    }
                }
            }
            return _instance;
        }
    }
    
    private DriveManager() { }
    
    /// <summary>DriveInfo -> SingleDrive 属性映射表</summary>
    private Dictionary<string, string> _propertyMap = new()
    {
        { "VolumeLabel", "Name" }, {"Name", "DriveLetter"}, 
        { "DriveFormat", "FileSystem" }, { "DriveType", "DriveType" },
        { "TotalSize", "Space" }, { "TotalFreeSpace", "Free" },
        { "IsReady", "IsReady" },
    };
    
    // Drive Source
    /// <summary>数据源</summary>
    public List<IDriveSource> DriveSources { get; set; } = [new LocalDriveSource()];
    /// <summary>数据源获取的磁盘属性</summary>
    public List<DriveInfo> AllDriveInfo { get; set; } = new();
    /// <summary>所有磁盘</summary>
    public List<SingleDrive> DriveList { get; set; } = new();
    /// <summary>磁盘数量</summary>
    public static int NumOfDisks { get; set; } = 0;
    
    /// <summary>
    /// 异步获取所有磁盘属性
    /// </summary>
    public async Task RefreshDriveSourcesAsync()
    {
        foreach (var driveSource in DriveSources)
        {
            AllDriveInfo.AddRange(await Task.Run(() 
                => driveSource.GetDrives().ToList()));
        }
    }

    /// <summary>
    /// 将DriveInfo转换为SingleDrive
    /// </summary>
    public void ConvertToSingleDrive()
    {
        foreach (var drive in AllDriveInfo)
        {   
            // 磁盘未就绪
            if (!drive.IsReady)
            {
                DriveList.Add(new SingleDrive()
                {
                    Path = new CPath(drive.Name),
                    IsReady = false,
                    DriveInfo = drive
                });
                continue;
            }

            var sd = new SingleDrive();
            try
            {
                foreach (var entry in _propertyMap)
                {
                    // 键值对获取对应的属性名称 + 反射取值
                    string sourceProp = entry.Key;
                    string targetProp = entry.Value;
                    PropertyInfo? propertyInfo = typeof(DriveInfo).GetProperty(sourceProp);
                    PropertyInfo? targetProperty = typeof(SingleDrive).GetProperty(targetProp);

                    // 磁盘属性或目标属性为空：下一次循环
                    if (propertyInfo == null || targetProperty == null) continue;

                    object? value = propertyInfo.GetValue(drive);
                    if (value == null) continue;
                    // 处理空卷标（VolumeLabel 可能返回空字符串）
                    if (sourceProp == "VolumeLabel" && string.IsNullOrEmpty(value.ToString()))
                        value = "Local Disk";
                    targetProperty.SetValue(sd, value); // 注意：被设置对象按值传递
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
                continue;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
                continue;
            }
            sd.DriveInfo = drive;
            sd.Path = new CPath(drive.Name);
            sd.CalculateSpace();
            DriveList.Add(sd);
        }
        NumOfDisks = DriveList.Count;
    }

    public void AddDriveSource(IDriveSource[] driveSources) => DriveSources.AddRange(driveSources);

    public async Task Refresh()
    {
        await RefreshDriveSourcesAsync();
        ConvertToSingleDrive();
    }
}