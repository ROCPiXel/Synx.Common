using Synx.Common.FileSystem.Operations;

namespace Synx.Common.FileSystem.Structures;

/// <summary>
/// CPath - CompositePath: struct
/// 复合路径，含有绝对与相对路径
/// </summary>
public struct CPath
{
    // 绝对路径与相对路径
    public string AbsolutePath { get; set; }
    public string RelativePath { get; set; }
    public string Base { get; set; }
    public string Name { get; set; }
    public string ParentPath { get; set; }
    public Uri? Uri { get; set; }
        
    public CPath(string path, string? basePath = null)
    {
        AbsolutePath = PathOperation.GetAbsolutePath(path, basePath);
        RelativePath = PathOperation.GetAbsolutePath(path, basePath);
        Name ??= PathOperation.GetNameFromPath(path);
        ParentPath ??= PathOperation.GetParentPath(path);
    }

    public CPath(params string[] paths):
        this(PathOperation.Combine(paths)) {}

    public CPath()
    {
        AbsolutePath = string.Empty;
        RelativePath = string.Empty;
        Name = string.Empty;
        ParentPath = string.Empty;
    }

    /// <summary>
    /// ToString: fOverride
    /// 获取相对路径
    /// </summary>
    /// <returns>string-Path</returns>
    public override string ToString()
    {
        return AbsolutePath;
    }

    /// <summary>
    /// 获取绝对路径
    /// </summary>
    /// <returns></returns>
    public string GetAbsolutePath()
    {
        return AbsolutePath;
    }

    /// <summary>
    /// 获取相对路径
    /// </summary>
    /// <returns>string-RELATIVE_Path</returns>
    public string GetRelativePath()
    {
        return RelativePath;
    }
}