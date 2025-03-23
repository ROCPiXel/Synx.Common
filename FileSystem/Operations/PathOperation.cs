using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Operations;

public static class PathOperation
{
    /// <summary>
    /// GetAbsolutePath: static string
    /// 获取绝对路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="basePath">可选，默认为工作路径</param>
    /// <returns></returns>
    public static string GetAbsolutePath(string path, string? basePath = null)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;
        basePath = basePath ?? AppDomain.CurrentDomain.BaseDirectory;
        return Path.GetFullPath(path, basePath);
    }

    public static string GetAbsolutePath(CPath cPath, string? basePath = null)
    {
        return GetAbsolutePath(cPath.RelativePath, basePath);
    }

    /// <summary>
    /// GetRelativePath: static string
    /// 获取相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="basePath">可选，默认为工作路径</param>
    /// <returns>相对路径</returns>
    public static string GetRelativePath(string path, string? basePath = null)
    {
        basePath ??= RTE.WorkspacePath;
        Uri uriPath = new(path);
        Uri uriBase = new(basePath);
        return uriBase.MakeRelativeUri(uriPath).ToString();
    }

    public static string GetRelativePath(CPath cPath, string? basePath = null)
    {
        return Path.GetRelativePath(cPath.RelativePath, basePath);
    }

    /// <summary>
    /// 返回传入路径的绝对路径的Uri
    /// </summary>
    /// <param name="cPath"></param>
    /// <returns>绝对路径的Uri</returns>
    public static Uri GetUri(CPath cPath)
    {
        return new Uri(cPath.AbsolutePath);
    }

    public static CPath RelativeToAbsolute(CPath cPath)
    {
        return cPath;
    }

    /// <summary>
    /// 从路径获取代表的文件名或目录名
    /// </summary>
    /// <param name="path">路径，最好是绝对路径</param>
    /// <returns></returns>
    public static string GetNameFromPath(string path)
    {
        string absolutePath = PathOperation.GetAbsolutePath(path);
        string[] part = absolutePath.Split("\\");
        return part[^1];
    }

    public static string GetNameFromPath(CPath cPath)
    {
        return GetNameFromPath(cPath.AbsolutePath);
    }

    /// <summary>
    /// 从路径获取父目录路径
    /// </summary>
    /// <param name="path">路径，最好是绝对路径</param>
    /// <returns></returns>
    public static string GetParentPath(string path)
    {
        if(string.IsNullOrEmpty(path)) return string.Empty;
        
        string absolutePath = PathOperation.GetAbsolutePath(path);
        string[] part = absolutePath.Split("\\");
        string parentPath = string.Empty;
        for (int i = 0; i < part.Length - 1; i++)
        {
            parentPath += part[i];
            parentPath += "\\";
        }
        parentPath = parentPath.Substring(0, parentPath.Length - 1);
        return parentPath;
    }

    public static string GetParentPath(CPath cPath)
    {
        return GetParentPath(cPath.AbsolutePath);
    }
}