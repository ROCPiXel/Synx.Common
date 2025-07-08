using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Helpers;

public static class FilePropertyHelper
{
    /// <summary>
    /// GetFileInfo - func
    /// 获取文件详细信息，如果该文件真实存在
    /// </summary>
    /// <param name="singleFile"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    public static SingleFile GetFileInfo(this SingleFile singleFile)
    {
        ArgumentException.ThrowIfNullOrEmpty(singleFile.Path.Absolute, nameof(singleFile));
        if (!File.Exists(singleFile.Path.Absolute))
        {
            throw new FileNotFoundException();
        }

        singleFile.IsExists = true;
        FileInfo fileInfo = new(singleFile.Path.Absolute);
        singleFile.FileInfo = fileInfo;

        singleFile.Size = fileInfo.Length;
        singleFile.IsReadOnly = fileInfo.IsReadOnly;

        singleFile.CreateTime = fileInfo.CreationTimeUtc; // 创建时间
        singleFile.ModifyTime = fileInfo.LastWriteTimeUtc; // 修改时间
        singleFile.AccessTime = fileInfo.LastAccessTimeUtc; // 访问时间

        return singleFile;
    }

    public static SingleFile GetFileInfo(string name, string path)
    {
        SingleFile singleFile = new SingleFile(name, path);
        return GetFileInfo(singleFile);
    }

    public static SingleFile GetFileInfo(CPath fullCPath)
    {
        SingleFile singleFile = new SingleFile(fullCPath);
        return GetFileInfo(singleFile);
    }

    public static double GetLength(SingleFile singleFile)
    {
        return new FileInfo(singleFile.Path.Absolute).Length;
    }
}