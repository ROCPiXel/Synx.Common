using Synx.Common.Enums;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Interfaces;

/// <summary>
/// IFileObject: Interface
/// 单个文件对象的基础信息
/// </summary>
/// <typeparam name="TFileObject">文件对象类型</typeparam>
public interface IFileObject<TFileObject>
{
    /// <summary>对象类型（枚举值）</summary>
    static abstract FileObjectType FileObjectType { get; }
    
    /// <summary>对应的<see cref="IFileSystem">文件系统</see></summary>
    static abstract IFileSystem FileSystem { get; }
    
    /// <summary><see cref="CPath">复合路径</see>，包含路径与名称相关</summary>
    CPath Path { get; set; }
    
    /// <summary>是否存在</summary>
    bool IsExists { get; set; }

    /// <summary>如果文件在文件系统存在则获取信息</summary>
    TFileObject GetInfo();
    
    /// <summary>刷新文件对象的信息</summary>
    TFileObject Refresh();
}