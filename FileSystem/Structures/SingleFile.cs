using Synx.Common.Enums;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Operations;
using Synx.Common.FileSystem.Providers;

namespace Synx.Common.FileSystem.Structures;

public class SingleFile : IFileObject<SingleFile>
{
    // 对象类型
    public static FileObjectType ObjectType { get; } = FileObjectType.File;
    public static IFileSystem FileSystem { get; } = SingleFileSystem.Instance;

    // 基本信息
    public string Name { get; set; } = string.Empty; // 名称
    public string RealName { get; set; } = string.Empty; // 除去后缀的名字
    public string Extension { get; set; } = string.Empty; // 扩展名
    public CPath Path { get; set; } // 路径+文件名，完整路径
    public bool IsExists { get; set; } // 是否存在

    // 以下为可空属性，当实例不存在为空
    public FileInfo? FileInfo { get; set; } // FileInfo类
    public bool? IsReadOnly { get; set; }
    public long? Length { get; set; } // 字节长度
    public long? DiskSpace { get; set; }
    public FileFormat? Type { get; set; } // 类型
    public DateTime? CreateTime { get; set; }
    public DateTime? ModifyTime { get; set; }
    public DateTime? AccessTime { get; set; }
    public string? OpenWith { get; set; } = string.Empty;

    /// <summary>
    /// 空对象
    /// 只输入文件名以及路径将自动获取大小等属性
    /// 全输入将定义新文件，可能引发与现有文件冲突
    /// </summary>
    public SingleFile()
    {
        Name = string.Empty;
        Extension = string.Empty;
        RealName = string.Empty;
        Path = new();
    }

    /// <summary>
    /// 路径为字符串的构造函数
    /// </summary>
    /// <param name="name">文件名</param>
    /// <param name="path">文件/文件夹（此处为文件）路径</param>
    /// <exception cref="Exception"></exception>
    public SingleFile(string name, string path)
    {
        FillInfo(name, path);
    }

    public SingleFile(string name, CPath path)
    {
        FillInfo(name, path.AbsolutePath);
    }

    public SingleFile(CPath fullPath)
    {
        FillInfo(fullPath);
    }

    // 文件信息相关
    public void FillInfo(string name, string parentPath)
    {
        Name = name;
        Extension = PathOperation.GetExtension(Name);
        RealName = PathOperation.GetRealName(Name);
        Path = new CPath([parentPath, name]);
        IsExists = File.Exists(Path.AbsolutePath);
    }

    public void FillInfo(string name, CPath cPath)
    {
        FillInfo(name, cPath.AbsolutePath);
    }

    public void FillInfo(CPath fullCPath)
    {
        FillInfo(fullCPath.Name, fullCPath.ParentPath);
    }

    /// <summary>
    /// 获取真实的文件信息
    /// </summary>
    /// <returns></returns>
    public SingleFile GetInfo()
    {
        FillInfo(Name, Path.ParentPath);
        return FileAttribute.GetFileInfo(this);
    }

    /// <summary>
    /// 刷新并重新获取信息
    /// </summary>
    /// <returns></returns>
    public SingleFile Refresh()
    {
        return GetInfo();
    }

    // 以下：文件操作
    public OpenWithApp GetAppOpenWith(string extension)
    {
        return 0;
    }

    public string GetExtension()
    {
        return Extension;
    }

    public double GetLength()
    {
        return FileAttribute.GetLength(this);
    }

    /// <summary>
    /// 使用实例信息创建文件
    /// </summary>
    /// <param name="creationMethod">创建方式<see cref="CreationMethod"/></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public SingleFile? Create(CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        return FileObjectOperation<SingleFile>.Create(this, creationMethod, suffix);
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
    /// <param name="creationMethod"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public SingleFile? Rename(string newFullPath,
        CreationMethod creationMethod = CreationMethod.Keep,
        string suffix = Definition.DefaultSuffix)
    {
        var newFile = FileObjectOperation<SingleFile>.Rename(this,
            newFullPath,
            creationMethod,
            suffix);
        GetInfo();
        return newFile;
    }

    public override string ToString() => Path.ToString();
}