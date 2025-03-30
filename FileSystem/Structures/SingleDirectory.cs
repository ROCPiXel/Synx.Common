using Synx.Common.Base;
using Synx.Common.Enums;
using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Operations;
using Synx.Common.Utils;

namespace Synx.Common.FileSystem.Structures;

public class SingleDirectory : IFileSysAct, IFileSysObj<SingleDirectory>
{
    // 对象类型
    public static FileObjectType ObjectType { get; } = FileObjectType.Directory;
    
    // 基本信息
    public string Name { get; set; } = string.Empty;
    public string Extension { get; } = string.Empty;
    public CPath ParentPath { get; set; } // 所在文件夹路径
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
    
    // 实现IFileSysAct
    static bool IFileSysAct.GetExistsAction(string fullPath) => Directory.Exists(fullPath);
    static void IFileSysAct.CreateAction(string fullPath) => Directory.CreateDirectory(fullPath);
    static void IFileSysAct.DeleteAction(string fullPath) => Directory.Delete(fullPath);
    static void IFileSysAct.MoveAction(string sourceFPath, string targetFPath) => Directory.Move(sourceFPath, targetFPath);
    static string IFileSysAct.GenerateUniquePathAction(string fullPath, string suffix) => PathStringProc.GenerateDirectoryPath(fullPath, suffix);
    
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
        
    
    // 文件夹信息
    /// <summary>
    /// 填充文件夹的对应信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentPath"></param>
    /// <returns></returns>
    public void FillInfo(string name, string parentPath)
    {
        Name = name;
        ParentPath = new(parentPath);
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
        DirectoryAttribute.GetDirectoryInfo(this);
        Depth = DirectoryAttribute.GetDepth(DirectoryInfo);
        return this;
    }
        
    // 内容
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
        FillInfo(Name, ParentPath);
        GetInfo();
        return this;
    }
    
    // 方法
    public override string ToString()
    {
        return Path.ToString();
    }
}