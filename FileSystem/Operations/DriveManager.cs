using Synx.Common.FileSystem.Structures;
using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Operations;

// TODO: DriveManager
public static class DriveManager
{
    public static List<SingleDrive> DiskList { get; set; } = new(); // 将包含网络磁盘驱动器，目前暂不支持
    public static int NumOfDisks { get; set; } = 0;
    public static readonly DriveInfo[] AllDrives = DriveInfo.GetDrives();

    /// <summary>
    /// 获取本地磁盘属性列表
    /// </summary>
    public static void GetLocalDiskList()
    {
        for (int i = 0; i < AllDrives.Length; i++)
        {
            try
            {
                var drive = AllDrives[i];
                SingleDrive singleDrive = new();
                singleDrive.Name = drive.VolumeLabel;
                singleDrive.DriveLetter = drive.Name;
                singleDrive.Path = new CPath(drive.Name);
                singleDrive.FileSystem = drive.DriveFormat;
                singleDrive.DriveType = drive.DriveType;

                singleDrive.Space = drive.TotalSize;
                singleDrive.Free = drive.TotalFreeSpace;
                singleDrive.Used = singleDrive.Space - singleDrive.Free;
                singleDrive.Usage = (double)singleDrive.Used / singleDrive.Space;
                singleDrive.SpaceGiB = SpaceUnitExchange.Change(singleDrive.Space);
                singleDrive.FreeGiB = SpaceUnitExchange.Change(singleDrive.Free);
                singleDrive.UsedGiB = SpaceUnitExchange.Change(singleDrive.Used);

                singleDrive.GetStringProperties();
                DiskList.Add(singleDrive);
            }
            catch (IOException ioex)
            {
                SingleDrive singleDrive = new();
                singleDrive.Name = "IOException";

                singleDrive.Properties = new() { ["Exception"] = singleDrive.Name, };
                DiskList.Add(singleDrive);
                Console.WriteLine(ioex);
            }
            catch (Exception)
            {
                //Console.WriteLine(ex);
            }
        }

        NumOfDisks = AllDrives.Length;
    }
}