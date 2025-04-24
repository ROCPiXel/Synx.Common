using Synx.Common.FileSystem.Interfaces;

namespace Synx.Common.FileSystem.Providers.Drive;

public class LocalDriveSource:IDriveSource
{
    public DriveInfo[] GetDrives() => DriveInfo.GetDrives();
}