using Synx.Common.Enums;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Interfaces;

/// <summary>
/// IFileSysObj: Interface
/// 单个文件结构标准
/// </summary>
/// <typeparam name="TFile">文件对象类型</typeparam>
public interface IFileSysObj<TFile>
{
    /// <summary>
    /// ObjectType: iIndex
    /// 对象类型
    /// </summary>
    public static abstract FileObjectType ObjectType { get; }
    
    /// <summary>
    /// iIndex
    /// 基本信息，支持不存在的路径
    /// </summary>
    public string Name { get; set; }
    public string Extension { get; }
    public CPath Path { get; set; }
    public CPath ParentPath { get; set; }
    public bool IsExists { get; set; }
    
    /// <summary>
    /// FillInfo: iFunc, +3 Reloads
    /// 填充文件信息 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parentPath"></param>
    public void FillInfo(string name, string parentPath);
    public void FillInfo(string name, CPath parentPath);
    public void FillInfo(CPath fullCPath);

    /// <summary>
    /// GetInfo: iFunc
    /// 如果文件存在则需要获取信息
    /// </summary>
    /// <returns></returns>
    public TFile GetInfo();
    
    /// <summary>
    /// Refresh: iFunc
    /// 刷新对象的信息
    /// </summary>
    /// <returns></returns>
    public TFile Refresh();
}