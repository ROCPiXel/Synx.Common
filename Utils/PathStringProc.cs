using Synx.Common.FileSystem.Structures;
using Synx.Common.Base;
using Synx.Common.Enums;
using Synx.Common.FileSystem.Interfaces;

namespace Synx.Common.Utils;

public static class PathStringProc
{
    /// <summary>
    /// GetExtension: Func
    /// 如果有，获取扩展名
    /// 注意：此方法不区别文件与目录，请不要传入目录造成错误
    /// </summary>
    /// <param name="name"></param>
    /// <param name="withPoint">是否在前面加上点作为分隔符</param>
    /// <returns></returns>
    public static string GetExtension(string name, bool withPoint = true)
    {
        try
        {
            var extension = withPoint ? "." + name.Split(".")[^1] : name.Split(".")[^1];
            return extension;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 获取除扩展名之外的部分文件名或路径
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetRealName(string name)
    {
        string[] part = name.Split(".");
        string realName = string.Empty;
        for (int i = 0; i < part.Length - 1; i++)
        {
            realName += part[i];
            realName += ".";
        }
        realName = realName.Substring(0, realName.Length - 1);
        return realName;
    }

    // TODO: 准备删除
    public static string GenerateDirectoryPath(string basePath, string suffix)
    {
        string candidate = $"{basePath}{suffix}";
        int counter = 1;
        while (Directory.Exists(candidate))
        {
            candidate = $"{basePath}{suffix}_{counter++}";
        }
        return candidate;
    }

    public static string GenerateFilePath(string basePath, string suffix)
    {
        string extension = GetExtension(basePath);
        string baseName = GetRealName(basePath);
        string candidate = $"{baseName}{suffix}{extension}";
        int counter = 1;
        while (File.Exists(candidate))
        {
            candidate = $"{basePath}{suffix}_{counter++}{extension}";
        }
        return candidate;
    }

    public static string GenerateUniquePath<TFileSysObj>(string basePath, string suffix = Definition.DefaultSuffix)
        where TFileSysObj : IFileSysAct, IFileSysObj<TFileSysObj>
    {
        string extension = string.Empty;
        if (TFileSysObj.ObjectType == FileObjectType.File)
        {
            extension = GetExtension(basePath);
        }

        string candidate = $"{basePath}{suffix}{extension}";
        int counter = 1;
        while (File.Exists(candidate))
        {
            candidate = $"{basePath}{suffix}_{counter++}{extension}";
        }

        return candidate;
    }
}