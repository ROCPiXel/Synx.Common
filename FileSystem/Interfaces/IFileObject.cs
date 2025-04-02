using Synx.Common.Enums;
using Synx.Common.FileSystem.Providers;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Interfaces;

/// <summary>
/// IFileObject: Interface
/// 单个文件对象的基础信息
/// </summary>
/// <typeparam name="TFileObject">文件对象类型</typeparam>
public interface IFileObject<TFileObject>
{
    /// <summary>
    /// ObjectType: iIndex
    /// 对象类型
    /// </summary>
    static abstract FileObjectType ObjectType { get; }
    static abstract IFileSystem FileSystem { get; }
    // TODO: 抽出IFileSysAct
    
    /// <summary>
    /// iIndex
    /// 基本信息，支持不存在的路径
    /// </summary>
    string Name { get; set; }
    string Extension { get; }
    CPath Path { get; set; }
    CPath ParentPath { get; set; }
    bool IsExists { get; set; }
    
    /// <summary>
    /// FillInfo: iFunc, +3 Reloads
    /// 填充文件信息 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentPath"></param>
    void FillInfo(string name, string parentPath);
    void FillInfo(string name, CPath parentPath);
    void FillInfo(CPath fullCPath);

    /// <summary>
    /// GetInfo: iFunc
    /// 如果文件存在则需要获取信息
    /// </summary>
    /// <returns></returns>
    TFileObject GetInfo();
    
    /// <summary>
    /// Refresh: iFunc
    /// 刷新对象的信息
    /// </summary>
    /// <returns></returns>
    TFileObject Refresh();
}