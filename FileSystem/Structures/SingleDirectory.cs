using Synx.Common.Base;
using Synx.Common.Enums;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Operations;
using Synx.Common.FileSystem.Providers;
using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Structures;

public class SingleDirectory : IFileObject<SingleDirectory>
{
    // 对象类型
    public static FileObjectType ObjectType { get; } = FileObjectType.Directory;
    public static IFileSystem FileSystem { get; } = DirectoryFileSystem.Instance;
    
    // 基本信息
    public string Name { get; set; } = string.Empty;
    public string Extension { get; } = string.Empty;
    public CPath Path { get; set; } // 路径+文件名，完整路径
    public DirectoryInfo? DirectoryInfo { get; set; } // 本文件夹的信息
    public bool IsExists { get; set; } = false;

    // 以下为可空属性，当实例不存在或代表空文件夹时为空
    public long? NumOfItems { get; set; }
    public int? Depth { get; set; }
    public List<SingleDirectory> SubDirectory { get; set; } = new();
    public List<SingleFile> SubFile { get; set; } = new();
    //public FileSystemInfo[]? ChildFileSystemInfo { get; set; } // 所有实体信息
    public List<CPath> ChildPathList { get; set; } = new(); // 所有路径信息
    // public bool? IsReadOnly { get; set; }
    // public long? Length { get; set; } // 字节长度
    // public long? DiskSpace { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? ModifyTime { get; set; }
    public DateTime? AccessTime { get; set; }
    
    // 构造函数
    public SingleDirectory()
        : this(string.Empty, string.Empty) { }
    public SingleDirectory(string name, string parentPath)
    {
        FillInfo(name, parentPath);
    }
    public SingleDirectory(string name, CPath parentPath)
        :this(name, parentPath.AbsolutePath) { }
    public SingleDirectory(string fullPath)
        :this(PathOperation.GetNameFromPath(fullPath), PathOperation.GetParentPath(fullPath)) { }
    public SingleDirectory(CPath fullCPath)
        :this(fullCPath.AbsolutePath) { }
    public SingleDirectory(DirectoryInfo directoryInfo)
        :this(directoryInfo.FullName) { }
        
    // 以下：信息与属性相关
    /// <summary>
    /// 填充文件夹的对应信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public void FillInfo(string name, string parentPath)
    {
        Name = name;
        Path = new($"{parentPath}\\{name}");
    }

    public void FillInfo(string name, CPath parentPath)
    {
        FillInfo(name, parentPath.AbsolutePath);
    }

    public void FillInfo(CPath fullCPath)
    {
        FillInfo(fullCPath.Name, fullCPath.ParentPath);
    }
        
    /// <summary>
    /// 获取本实例的实际信息
    /// </summary>
    /// <returns></returns>
    public SingleDirectory GetInfo()
    {
        return DirectoryAttribute.GetDirectoryInfo(this);
    }
        
    /// <summary>
    /// 获取本实例的深度，0为无嵌套
    /// </summary>
    /// <returns></returns>
    public SingleDirectory GetDepth()
    {
        if (DirectoryInfo is null) DirectoryAttribute.GetDirectoryInfo(this);
        Depth = DirectoryAttribute.GetDepth(DirectoryInfo!);
        return this;
    }
        
    // 以下：内容相关
    public SingleDirectory GetContent()
    {
        DirectoryAttribute.GetContent(this);
        return this;
    }
        
    public SingleDirectory Traverse()
    {
        DirectoryAttribute.Traverse(this);
        return this;
    }

    public SingleDirectory Refresh()
    {
        FillInfo(Name, Path.ParentPath);
        GetInfo();
        return this;
    }
    
    // 以下：目录操作
    /// <summary>
    /// 创建本实例
    /// </summary>
    /// <param name="creationMethod">创建方式<see cref="Synx.Common.Enums.CreationMethod"/></param>
    /// <param name="suffix"></param>
    public void Create(CreationMethod creationMethod = CreationMethod.Keep, string suffix = Definition.DefaultSuffix)
    {
        FileObjectOperation<SingleDirectory>.Create(this, creationMethod, suffix);
    }
    
    /// <summary>
    /// 删除本实例
    /// </summary>
    /// <returns>是否删除成功</returns>
    public bool Delete() => FileObjectOperation<SingleDirectory>.Delete(this);
    
    /// <summary>
    /// 重命名本实例
    /// </summary>
    /// <param name="newFullPath">新的全路径</param>
    /// <param name="creationMethod">创建方式<see cref="Synx.Common.Enums.CreationMethod"/></param>
    /// <param name="suffix">后缀</param>
    /// <returns>重命名后的新对象</returns>
    public SingleDirectory? Rename(string newFullPath,
        CreationMethod creationMethod = CreationMethod.Keep,
        string suffix = Definition.DefaultSuffix)
    {
        var newDirectory = FileObjectOperation<SingleDirectory>.Rename(this, newFullPath, creationMethod, suffix); 
        GetInfo();
        return newDirectory;
    }

    // 方法
    public override string ToString() => Path.ToString();
}