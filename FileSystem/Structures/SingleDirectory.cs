using Synx.Common.Enums;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Operations;
using Synx.Common.FileSystem.Providers;
using Synx.Common.FileSystem.Providers.FIle;

namespace Synx.Common.FileSystem.Structures;

public class SingleDirectory : IFileObject<SingleDirectory>
{
    // 对象类型
    public static FileObjectType FileObjectType { get; } = FileObjectType.Directory;
    public static IFileSystem FileSystem { get; } = DirectoryFileSystem.Instance;
    
    // 基本信息
    /// <summary>目录对应的路径</summary>
    public CPath Path { get; set; } // 路径+文件名，完整路径
    
    /// <summary>是否真实存在</summary>
    public bool IsExists { get; set; }

    // 以下为可空属性，当实例不存在或代表空文件夹时为空
    /// <summary>包含的DirectoryInfo</summary>
    public DirectoryInfo? DirectoryInfo { get; set; } // 本文件夹的信息
    
    /// <summary>目录下所有实体的数量</summary>
    public long? NumOfItems { get; set; }
    
    /// <summary>目录的深度</summary>
    public int? Depth { get; set; }
    
    /// <summary>子目录</summary>
    public List<SingleDirectory> SubDirectory { get; set; } = new();
    /// <summary>子文件（不包含文件夹）</summary>
    public List<SingleFile> SubFile { get; set; } = new();
    /// <summary>所有路径信息</summary>
    public Dictionary<CPath, FileObjectType> ChildCPathDictionary { get; set; } = new();

    // TODO
    public bool? IsReadOnly { get; set; }
    public long? Size { get; set; } // 字节长度
    
    /// <summary>创建时间</summary>
    public DateTime? CreateTime { get; set; }
    /// <summary>最后修改时间</summary>
    public DateTime? ModifyTime { get; set; }
    /// <summary>最后访问时间</summary>
    public DateTime? AccessTime { get; set; }
    
    // 构造函数
    public SingleDirectory()
        : this(string.Empty, string.Empty) { }
    public SingleDirectory(string name, string parentPath)
        => Path = new CPath(System.IO.Path.Combine(parentPath, name));
    public SingleDirectory(string name, CPath parentPath)
        :this(name, parentPath.Absolute) { }
    public SingleDirectory(string fullPath) 
        => Path = new CPath(fullPath);
    public SingleDirectory(CPath fullCPath)
        :this(fullCPath.Absolute) { }
    public SingleDirectory(DirectoryInfo directoryInfo)
        :this(directoryInfo.FullName) { }
        
    // 以下：信息与属性相关
    /// <summary>
    /// 获取本实例的实际信息
    /// </summary>
    /// <returns></returns>
    public SingleDirectory GetInfo()
    {
        return DirectoryPropertyHelper.GetDirectoryInfo(this);
    }
        
    /// <summary>
    /// 获取本实例的深度，0为无嵌套
    /// </summary>
    /// <returns></returns>
    public SingleDirectory GetDepth()
    {
        if (DirectoryInfo is null) DirectoryPropertyHelper.GetDirectoryInfo(this);
        Depth = DirectoryPropertyHelper.GetDepth(DirectoryInfo!);
        return this;
    }
        
    // 以下：内容相关
    /// <summary>
    /// 获取一层级的内容
    /// </summary>
    /// <returns></returns>
    public SingleDirectory GetContent()
    {
        DirectoryPropertyHelper.GetContent(this);
        return this;
    }
        
    /// <summary>
    /// 遍历
    /// </summary>
    /// <returns></returns>
    public SingleDirectory Traverse()
    {
        DirectoryPropertyHelper.Traverse(this);
        return this;
    }

    /// <summary>
    /// 尝试从现有文件系统中刷新
    /// </summary>
    /// <returns></returns>
    public SingleDirectory Refresh()
    {
        GetInfo();
        return this;
    }

    /// <summary>
    /// 向<see cref="ChildCPathDictionary"/>
    /// <see cref="SubDirectory"/>
    /// <see cref="SubFile"/>添加项目
    /// </summary>
    /// <param name="objectType"></param>
    /// <param name="objectCPath"></param>
    internal void AddObjectToList(CPath objectCPath, FileObjectType objectType)
    {
        switch (objectType)
        {
            case FileObjectType.Directory:
                SubDirectory.Add(new SingleDirectory(objectCPath));
                break;
            case FileObjectType.File:
                SubFile.Add(new SingleFile(objectCPath));
                break;
        }
        ChildCPathDictionary.Add(objectCPath, objectType);
    }
    
    // 以下：目录操作
    /// <summary>
    /// 创建本实例
    /// </summary>
    /// <param name="fileConflictResolution">创建方式<see cref="FileConflictResolution"/></param>
    /// <param name="suffix"></param>
    public void Create(FileConflictResolution fileConflictResolution = FileConflictResolution.Keep, string suffix = Definition.DefaultSuffix)
    {
        FileObjectOperation<SingleDirectory>.Create(this, fileConflictResolution, suffix);
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
    /// <param name="fileConflictResolution">创建方式<see cref="FileConflictResolution"/></param>
    /// <param name="suffix">后缀</param>
    /// <returns>重命名后的新对象</returns>
    public SingleDirectory? Rename(string newFullPath,
        FileConflictResolution fileConflictResolution = FileConflictResolution.Keep,
        string suffix = Definition.DefaultSuffix)
    {
        var newDirectory = FileObjectOperation<SingleDirectory>.Rename(this, newFullPath, fileConflictResolution, suffix); 
        GetInfo();
        return newDirectory;
    }

    // 方法
    public override string ToString() => Path.ToString();
}