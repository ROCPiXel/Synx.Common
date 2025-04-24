using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Interfaces;

public interface IDriveSource
{
    /// <summary>获取所有磁盘信息</summary>
    DriveInfo[] GetDrives();
}