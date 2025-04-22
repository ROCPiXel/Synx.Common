using System.Management;
using System.Runtime.Versioning;

namespace Synx.Common;

/// <summary>
/// Rte: Runtime Environment - Class
/// 创建实例以获取运行环境信息以及各项属性
/// </summary>
[UnsupportedOSPlatform("iOS")]
[UnsupportedOSPlatform("Android")]
[UnsupportedOSPlatform("Browser")]
[UnsupportedOSPlatform("macOS")]
[UnsupportedOSPlatform("Linux")]
public static class Rte
{
    /// <summary>
    /// 要获取的属性名称
    /// </summary>
    private static string[] PropertyNameList { get; set; } =
    [
        "Win32_ComputerSystemProduct", 
          "Win32_BaseBoard", 
          "Win32_Processor", 
          "Win32_PhysicalMemory", 
          "Win32_VideoController", 
          "Win32_LogicalDisk"
    ];
    
    /// <summary>
    /// 数据提供方
    /// ManagementClass当前仅限Windows平台
    /// </summary>
    public static ManagementClass ManagementClassProvider { get; set; } = new ManagementClass();

    // 需要含有private以供内部操作
    private static string _userName = Environment.MachineName; //系统名称
    private static string _osVersionDescription = Environment.OSVersion.ToString(); //系统版本概述
    private static string _osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString(); //系统架构
    private static string _osPath = Environment.SystemDirectory; //系统路径
    private static string _workspacePath = Environment.CurrentDirectory; //工作环境目录

    public static string UserName => _userName; //系统名称
    public static string OsVersionDescription => _osVersionDescription; //系统版本概述
    public static string OsArchitecture => _osArchitecture; //系统架构
    public static string OsPath => _osPath; //系统路径
    public static string WorkspacePath => _workspacePath; //工作环境目录

    //以下属性为列表，含详细信息
    //默认为空，获取系统信息请使用GetHardwareInfo();
    public static Dictionary<string, string> BasicSystemInfo { get; } = new(); //计算机基本信息
    public static Dictionary<string, string> BaseBoardInfo { get; } = new(); // 主板信息
    public static Dictionary<string, string> CpuInfo { get; } = new();
    public static Dictionary<string, string> RamInfo { get; } = new();
    public static Dictionary<string, string> GpuInfo { get; } = new();
    public static Dictionary<string, string> DiskInfo { get; } = new();

    private static void GetBasicSystemInfo()
    {
        _userName = Environment.MachineName;
        _osVersionDescription = Environment.OSVersion.ToString();
        _osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
        _osPath = Environment.SystemDirectory;
        _workspacePath = Environment.CurrentDirectory;
    }

    /// <summary>
    /// GetHardwareInfo - function(void)
    /// 由于获取硬件信息需要较长时间，而大部分并不需要这一操作，故将其移出构造函数中，有需要时调用
    /// </summary>
    /// <returns>None</returns>
    public static void GetHardwareInfo()
    {
        List<Dictionary<string, string>> infoLists = new();
        infoLists.Add(BasicSystemInfo); infoLists.Add(BaseBoardInfo);
        infoLists.Add(CpuInfo); infoLists.Add(RamInfo); infoLists.Add(GpuInfo); infoLists.Add(DiskInfo);

        // TODO：重复项较难操作，建议更换数据类型
        for (int i = 0; i < infoLists.Count; i++)
        {
            Dictionary<string, string> list = infoLists[i];
            list.Clear();
            //string propertyName = PropertyNameList[i];
            // [ATTENTION] 此方法导致跨平台不可用
            var moc = ManagementClassProvider.GetInstances();
            int errCount = 0;
            foreach (ManagementObject mo in moc)
            {
                foreach (var sysInfoItem in mo.Properties)
                {
                    if (sysInfoItem.Value is not null) //检查是否为null
                    {
                        try
                        {
                            list.Add(sysInfoItem.Name, sysInfoItem.Value.ToString());
                        }
                        catch (Exception)
                        {
                            //Console.WriteLine(ex.ToString());
                            errCount++;
                            list.Add(sysInfoItem.Name + errCount.ToString(), sysInfoItem.Value.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            list.Add(sysInfoItem.Name, "NULL");
                        }
                        catch (Exception)
                        {
                            //Console.WriteLine(ex.ToString());
                            errCount++;
                            list.Add(sysInfoItem.Name + errCount.ToString(), "NULL");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 暂未实现
    /// 获取当前环境的各项系统信息
    /// 非从列表中提取而获取的属性信息，请使用索引器
    /// </summary>
    /// <returns>string</returns>
    public static string GetCpuInfo()
    {
        return string.Empty;
    }

    /// <summary>
    /// 获取当前环境的（工作路径）
    /// </summary>
    /// <returns>path</returns>
    public static string GetWorkspacePath()
    {
        return WorkspacePath;
    }
}