using Synx.Common.Enums;
using Synx.Common.FileSystem.Structures;

namespace Synx.Common.FileSystem.Helpers;

public static class DirectoryPropertyHelper
{
    /// <summary>
    /// GetDirectoryInfo - func
    /// 获取目录信息，如果目录真实存在
    /// </summary>
    /// <param name="singleDirectory"></param>
    /// <returns></returns>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public static SingleDirectory GetDirectoryInfo(SingleDirectory singleDirectory)
    {
        DirectoryInfo directoryInfo = new(singleDirectory.Path.Absolute);
        singleDirectory.DirectoryInfo = directoryInfo;
        singleDirectory.IsExists = directoryInfo.Exists;

        if (!singleDirectory.IsExists)
        {
            throw new DirectoryNotFoundException("Directory does not exist");
        }

        singleDirectory.CreateTime = directoryInfo.CreationTime; // 创建时间
        singleDirectory.ModifyTime = directoryInfo.LastWriteTime; // 修改时间
        singleDirectory.AccessTime = directoryInfo.LastAccessTime; // 访问时间

        return singleDirectory;
    }

    /// <summary>
    /// GetDepth: func
    /// 获取目录深度，当前层级为0，逐级各加一
    /// </summary>
    /// <param name="directoryInfo"></param>
    /// <param name="maxDepth"></param>
    /// <returns></returns>
    public static int GetDepth(DirectoryInfo directoryInfo, int maxDepth = Definition.DirectoryScanningMaxDepth)
    {
        int depth = 0;
        try
        {
            foreach (var di in directoryInfo.GetDirectories())
            {
                int step = GetDepth(di);
                if (step > depth)
                {
                    if (step >= maxDepth)
                    {
                        return step;
                    }

                    depth = step;
                }
            }
        }
        catch (Exception)
        {
            return 0;
        }

        return depth + 1;
    }

    /// <summary>
    /// GetContent: func
    /// 获取文件夹下一层级中的所有内容
    /// 承担了所有内容的填充
    /// </summary>
    /// <param name="sd">SingleDirectory</param>
    /// <returns></returns>
    public static SingleDirectory GetContent(SingleDirectory sd)
    {
        if (sd.DirectoryInfo is null) GetDirectoryInfo(sd);
        try
        {
            // 当递归传入空目录，引发异常返回
            if (sd.DirectoryInfo == null) GetDirectoryInfo(sd);
            
            // 文件夹
            DirectoryInfo[] directoriesInfo = sd.DirectoryInfo!.GetDirectories();
            foreach (DirectoryInfo di in directoriesInfo)
            {
                sd.SubDirectory.Add(new SingleDirectory(di));
                // sd.ChildCPathDictionary.Add(new CPath(di.FullName));
            }
            
            // 文件
            FileInfo[] fileInfos = sd.DirectoryInfo!.GetFiles();
            foreach (FileInfo fi in fileInfos)
            {
                sd.SubFile.Add(new SingleFile(fi.FullName));
            }
        }
        catch (Exception)
        {
            return new SingleDirectory();
        }
        return sd;
    }

    /// <summary>
    /// Traverse: func
    /// 遍历整个文件夹（指定层级）
    /// </summary>
    /// <param name="singleDirectory"></param>
    /// <param name="targetDepth"></param>
    /// <param name="recursionDepth"></param>
    /// <returns></returns>
    public static SingleDirectory Traverse(this SingleDirectory singleDirectory, 
        int? targetDepth = Definition.DirectoryScanningMaxDepth, int? recursionDepth = 0)
    {
        int currentDepth = recursionDepth ?? 0;
        if (singleDirectory.DirectoryInfo is null) GetDirectoryInfo(singleDirectory);
        
        foreach (FileInfo fi in singleDirectory.DirectoryInfo!.GetFiles())
        {
            singleDirectory.AddObjectToList(new CPath(fi.FullName), FileObjectType.File);
        }

        // 基本情况1：到达目标
        if (targetDepth == currentDepth) return singleDirectory;

        try
        {
            foreach (DirectoryInfo di in singleDirectory.DirectoryInfo!.GetDirectories())
            {
                // currentDepth++; //有点深奥
                SingleDirectory dir = new(di);
                singleDirectory.AddObjectToList(new CPath(di.FullName), FileObjectType.Directory);
                singleDirectory.SubDirectory.Add(Traverse(dir, targetDepth, currentDepth++));
            }
        }
        catch (Exception)
        {
            // 基本情况2：到达底层
            return singleDirectory;
        }
        return singleDirectory;
    }

    /// <summary>
    /// ExpandChild: func
    /// 根据指定项<see cref="SingleDirectory.ChildCPathDictionary"/>展开至平面字典中
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetDepth"></param>
    /// <param name="recursionDepth"></param>
    /// <returns></returns>
    public static Dictionary<CPath, FileObjectType> ExpandChild(this SingleDirectory source,
        int? targetDepth = Definition.DirectoryScanningMaxDepth, int? recursionDepth = 0)
    {
        ArgumentNullException.ThrowIfNull(source.ChildCPathDictionary, nameof(source.ChildCPathDictionary));

        Dictionary<CPath, FileObjectType> dict = new();
        int currentDepth = recursionDepth ?? 0;
        
        if(currentDepth == targetDepth) return dict;

        try
        {
            foreach (var p in source.ChildCPathDictionary)
            {
                dict.Add(p.Key, p.Value);
            }

            foreach (var d in source.SubDirectory)
            {
                var c = ExpandChild(d, targetDepth, currentDepth++);
                foreach (var p in c)
                {
                    dict.Add(p.Key, p.Value);
                }
            }
        }
        catch (Exception)
        {
            return dict;
        }
        return dict;
    }
}