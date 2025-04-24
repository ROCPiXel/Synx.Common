using Synx.Common.FileSystem.Interfaces;
using Synx.Common.FileSystem.Structures;
using System.Text.RegularExpressions;

namespace Synx.Common.FileSystem.Operations;

/// <summary>
/// 路径的一些常规操作
/// 与<see cref="System.IO.Path">System.IO.Path</see>高度重合的方法将不再实现（提交fb52007b已经存档）
/// 更多的是小工具以及<see cref="CPath">CPath</see>的扩展方法
/// </summary>
/// <seealso href="https://learn.microsoft.com/zh-cn/dotnet/api/system.io.path?view=net-8.0">System.IO.Path：Path类与基本方法</seealso>
/// <seealso href="https://learn.microsoft.com/zh-cn/dotnet/fundamentals/code-analysis/quality-rules/ca1847">CA1847：优先使用char</seealso>
public static class PathOperation
{
    /// <summary>
    /// （根据顺序）拼接路径
    /// 推荐使用<see cref="System.IO.Path.Combine(string[])">Path.Combine</see>代替
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static string Combine(params string[] paths) => Path.Combine(paths);

    /// <summary>
    /// 标准化一个路径
    /// 包含 去除末尾空格，标准化分隔符，去除多余的分隔符
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string StandardizePath(this string path) => path.Replace('/', '\\').TrimEnd().TrimEnd('\\', '/');

    /// <summary>
    /// 获取绝对路径
    /// 推荐使用<see cref="System.IO.Path.GetFullPath(string)">Path.GetFullPath</see>代替
    /// </summary>
    /// <param name="path"></param>
    /// <param name="basePath">可选，默认为工作路径</param>
    /// <returns></returns>
    public static string GetAbsolutePath(string path, string? basePath = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        return  basePath is null 
            ? Path.GetFullPath(path)
            : Path.GetFullPath(path, basePath);
    }

    /// <summary>
    /// 获取相对路径
    /// 注意：使用<see cref="System.IO.Path.GetRelativePath"/>方法时，路径与相对路径形参位置相反
    /// 推荐使用<see cref="Path.GetRelativePath(string, string)">Path.GetRelativePath(relativeTo, path)</see>代替
    /// Unittest 20240416: 不再使用Uri作为相对路径的计算
    /// </summary>
    /// <param name="path"></param>
    /// <param name="basePath">可选，默认为工作路径</param>
    /// <returns>相对路径</returns>
    public static string GetRelativePath(string path, string? basePath = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(path, nameof(path));
        return basePath is null
            ? Path.GetRelativePath(AppDomain.CurrentDomain.BaseDirectory, path)
            : Path.GetRelativePath(basePath, path);
    }

    /// <summary>
    /// 获取路径的根目录
    /// 可以使用<see cref="System.IO.Path.GetPathRoot(string?)">Path.GetPathRoot</see>代替
    /// 检测到string.Empty时返回null
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string? GetPathRoot(string? path)
    {
        var root = Path.GetPathRoot(path);
        return string.IsNullOrEmpty(root) ? null : root;
    }

    /// <summary>
    /// 从路径获取代表的文件名或目录名
    /// </summary>
    /// <param name="path">路径，最好是绝对路径</param>
    /// <returns>包含后缀的文件或者目录名</returns>
    public static string GetNameFromPath(string path)
    {
        string absolutePath =GetAbsolutePath(path);
        string[] part = absolutePath.Split('\\');
        return part[^1];
    }

    /// <summary>
    /// 从路径获取父目录路径
    /// 可以使用<see cref="System.IO.Directory.GetParent(string)">Directory.GetParent</see>代替
    /// UnitTest20250411: 适配相对路径
    /// </summary>
    /// <param name="path">路径，不要传入根目录否则报错</param>
    /// <returns>父路径</returns>
    public static string GetParentPath(string path)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));
        var pathParts = path.Split('\\');
        var partsLength = pathParts.Length;
        if (partsLength == 1) // 是根目录
        {
            throw new ArgumentException("[ERR]根目录没有父路径");
        }
        if (pathParts[^1] == string.Empty) // 末尾是斜杠
        {
            if(partsLength == 2) // （且）是根目录
            {
                throw new ArgumentException("[ERR]根目录没有父路径"); 
            }
            return path[..^(pathParts[^2].Length + 2)]; // 处理掉末尾斜杠+结果末尾的斜杠
        }
        return path[..^(pathParts[^1].Length + 1)]; // 处理掉结果末尾的斜杠
    }
    
    /// <summary>
    /// GetExtension: Func
    /// 如果有，获取扩展名
    /// 注意：此方法不区别文件与目录，请不要传入目录造成错误
    /// 推荐使用<see cref="System.IO.Path.GetExtension(string?)">Path.GetExtension</see>代替
    /// </summary>
    /// <param name="name"></param>
    /// <param name="includeDot">是否在前面加上点作为分隔符</param>
    /// <returns></returns>
    public static string GetExtension(string name, bool includeDot = true)
    {
        if ((!name.Contains('.')) || String.IsNullOrEmpty(name)) return string.Empty;
        var extension = includeDot ? "." + name.Split('.')[^1] : name.Split('.')[^1];
        return extension;
    }

    /// <summary>
    /// 获取除扩展名之外的部分文件名或路径
    /// 可以使用<see cref="System.IO.Path.GetFileNameWithoutExtension(string)">Path.GetFileNameWithoutExtension</see>代替
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetRealName(string name)
    {
        string[] part = name.Split('\\')[^1].Split('.');
        if (part.Length == 1) return part[0]; // 没有扩展名
        string realName = string.Join('.', part[..^1]);
        return realName;
    }
    
    /// <summary>
    /// 根据提供的文件系统生成唯一的路径以避免重复
    /// </summary>
    /// <param name="basePath"></param>
    /// <param name="suffix"></param>
    /// <typeparam name="TFileSysObj"></typeparam>
    /// <returns></returns>
    public static string GenerateUniquePath<TFileSysObj>(string basePath, string suffix = Definition.DefaultSuffix)
        where TFileSysObj : IFileObject<TFileSysObj>
    {
        ArgumentNullException.ThrowIfNull(basePath, nameof(basePath));
        string extension = TFileSysObj.FileSystem.GetExtension(basePath, true); // 取后缀
        string name = basePath[..^extension.Length];

        // 以下：匹配
        int existingCounter = 0;
        int suffixLength = suffix.Length;
        if (name[^suffixLength..] == suffix) // 名称包含后缀
        {
            name = name[..^suffixLength];
            existingCounter++;
        }
        else // 名称包含后缀，剥离旧后缀并提取现有计数器
        {
            var match = Regex.Match(name, $@"{Regex.Escape(suffix)}(\d+)$");
            if (match.Success) // 提取已有计数器的最大值（如 "_2" → 2）
            {
                name = name[..^(match.Groups[^1].Length + suffixLength)]; // 移除 "后缀_数字"
                existingCounter = int.Parse(match.Groups[^1].Value);
            }
        }

        // 以下：生成
        // 生成唯一路径
        int counter = existingCounter + 1;
        string candidate;
        do
        {
            candidate = counter == 1
                ? $"{name}{suffix}{extension}"
                : $"{name}{suffix}{counter}{extension}";
            counter++;
        } while (TFileSysObj.FileSystem.Exists(candidate));

        return candidate;
    }
}