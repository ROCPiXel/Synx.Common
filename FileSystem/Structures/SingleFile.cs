using Synx.Common.Enums;
using Synx.Common.FileSystem.Helpers;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Providers;
using Synx.Common.FileSystem.Providers.File;

namespace Synx.Common.FileSystem.Structures;

public class SingleFile : IFileObject<SingleFile>
{
    // 对象类型
    public static FileObjectType FileObjectType { get; } = FileObjectType.File;
    public static IFileSystem FileSystem { get; } = SingleFileSystem.Instance;

    // 基本信息
    public CPath Path { get; set; } // 路径+文件名，完整路径
    public bool IsExists { get; set; } // 是否存在

    // 以下为可空属性，当实例不存在为空
    public FileInfo? FileInfo { get; set; } // FileInfo类
    public bool? IsReadOnly { get; set; }
    public long? Size { get; set; } // 字节长度
    public FileFormat? Type { get; set; } // 类型
    public DateTime? CreateTime { get; set; }
    public DateTime? ModifyTime { get; set; }
    public DateTime? AccessTime { get; set; }
    public string? OpenWith { get; set; }

    /// <summary>
    /// 空对象
    /// 只输入文件名以及路径将自动获取大小等属性
    /// 全输入将定义新文件，可能引发与现有文件冲突
    /// </summary>
    public SingleFile()
    {
        Path = new();
    }

    /// <summary>
    /// 路径为字符串的构造函数
    /// </summary>
    /// <param name="name">文件名</param>
    /// <param name="parentPath">路径</param>
    /// <exception cref="Exception"></exception>
    public SingleFile(string name, string parentPath) => Path = new CPath([name, parentPath]);
    public SingleFile(string fullPath) => Path = new CPath(fullPath);
    public SingleFile(CPath cPath) => Path = cPath;

    // 文件信息相关

    /// <summary>
    /// 获取真实的文件信息
    /// </summary>
    /// <returns></returns>
    public SingleFile GetInfo() => this.GetFileInfo();

    /// <summary>
    /// 刷新并重新获取信息
    /// </summary>
    /// <returns></returns>
    public SingleFile Refresh()
    {
        Path.Sync();
        return GetInfo();
    }

    // 以下：文件操作
    public OpenWithApp GetAppOpenWith(string extension)
    {
        return 0;
    }

    public double GetLength()
    {
        return FilePropertyHelper.GetLength(this);
    }

    /// <summary>
    /// 使用实例信息创建文件
    /// </summary>
    /// <param name="fileConflictResolution">创建方式<see cref="FileConflictResolution"/></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public SingleFile? Create(FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        return FileObjectOperation<SingleFile>.Create(this, fileConflictResolution, suffix);
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <returns></returns>
    public bool Delete()
    {
        return FileObjectOperation<SingleFile>.Delete(this);
    }

    /// <summary>
    /// 重命名文件，本实例数据将自动更新
    /// </summary>
    /// <param name="newFullPath"></param>
    /// <param name="fileConflictResolution"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public SingleFile? Rename(string newFullPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep,
        string suffix = Definition.DefaultSuffix)
    {
        var newFile = FileObjectOperation<SingleFile>.Rename(this,
            newFullPath,
            fileConflictResolution,
            suffix);
        GetInfo();
        return newFile;
    }

    public override string ToString() => Path.ToString();
}