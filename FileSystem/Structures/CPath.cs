using Synx.Common.Enums;
using Synx.Common.FileSystem.Operations;

namespace Synx.Common.FileSystem.Structures;

/// <summary>
/// CPath - CompositePath: struct
/// 复合路径，含有绝对与相对路径
/// </summary>
public struct CPath //TODO：总觉得哪里不对
{
    private string? _absolutePath;
    private string? _relativePath;
    private string? _base;
    private Uri? _uri;
    private string? _parentPath;
    private string? _name;
    
    /// <summary>
    /// 绝对路径
    /// 获取时 若为空则依赖相对路径生成，获取时绝对路径或<see cref="RelativePath">相对路径</see>
    /// 至少有一者不能为空
    /// </summary>
    public string AbsolutePath
    {
        get
        {
            if (_absolutePath != null) return _absolutePath;
            ArgumentException.ThrowIfNullOrEmpty(_relativePath, nameof(_relativePath));
            return _absolutePath = Path.GetFullPath(_relativePath, _base ?? AppDomain.CurrentDomain.BaseDirectory);
        }
        set => _absolutePath = value;
    }
    
    /// <summary>
    /// 相对路径，相对于<see cref="Base">基路径</see>的路径
    /// 获取时 若为空则依赖绝对路径生成，获取时相对路径或<see cref="AbsolutePath">绝对路径</see>
    /// 至少有一者不能为空
    /// </summary>
    public string RelativePath
    {
        get
        {
            if (_relativePath != null) return _relativePath;
            ArgumentException.ThrowIfNullOrEmpty(_absolutePath, nameof(_absolutePath));
            _base ??= AppDomain.CurrentDomain.BaseDirectory;
            return _relativePath = Path.GetRelativePath(_base, _absolutePath);
        }
        set => _relativePath = value;
    }
    
    /// <summary>
    /// 基路径，相对路径相对于的路径
    /// 获取时 若为空则根据<see cref="RelativePath">相对路径</see>
    /// 与<see cref="AbsolutePath">绝对路径</see>反推，若两者有一为空则返回工作目录路径
    /// </summary>
    public string Base
    {
        get
        {
            if (_base != null) return _base;
            if(_relativePath == null || _absolutePath == null) return AppDomain.CurrentDomain.BaseDirectory;
            return _base = Path.GetFullPath(_relativePath, AppDomain.CurrentDomain.BaseDirectory);
        }
        set => _base = value;
    }

    /// <summary>
    /// <see cref="System.Uri">URI</see> 只读
    /// 获取时 若为空则根据<see cref="AbsolutePath">绝对路径</see>
    /// 与<see cref="AbsolutePath">绝对路径</see>尝试生成新的Uri
    /// </summary>
    /// <exception cref="ArgumentException">无法生成Uri</exception>
    public Uri Uri
    {
        get
        {
            if (_uri is not null) return _uri;
            try
            {
                _uri = new Uri(AbsolutePath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"无法获取URI，因为路径格式错误{ex.Message}", ex);
            }
            return _uri;
        }
    }

    public string Name
    {
        get
        {
            if (_name != null) return _name;
            if (_relativePath == null || _absolutePath == null)
            {
                if (_relativePath == null && _absolutePath == null) throw new ArgumentException("无法获取name，因为两个路径都为空");
                if (_relativePath == null) return PathOperation.GetNameFromPath(_absolutePath!);
            }
            return _name = PathOperation.GetNameFromPath(_relativePath);
        }
    }

    public string ParentPath
    {
        get
        {
            if (_parentPath != null) return _parentPath;
            if (_relativePath == null || _absolutePath == null)
            {
                if (_relativePath == null && _absolutePath == null) throw new ArgumentException("无法获取name，因为两个路径都为空");
                if (_relativePath == null) return PathOperation.GetParentPath(_absolutePath!);
            }
            return _parentPath = PathOperation.GetParentPath(_relativePath);
        }
    }
    
    /// <summary>
    /// CPath构造函数，默认传入绝对路径
    /// </summary>
    /// <param name="absolutePath"></param>
    /// <param name="basePath"></param>
    public CPath(string absolutePath, string? basePath = null)
    {
        Base = basePath ?? AppDomain.CurrentDomain.BaseDirectory;
        AbsolutePath = Path.GetFullPath(absolutePath, Base);
    }

    /// <summary>
    /// CPath构造函数，传入后拼接路径并更新绝对路径
    /// </summary>
    /// <param name="paths"></param>
    public CPath(params string[] paths):
        this(Path.Combine(paths)) {}

    public CPath(){}

    public CPath Sync(CPathSyncTrigger trigger = CPathSyncTrigger.AbsolutePath)
    {
        // 转换为绝对路径操作，为防止依赖使用私有成员避免走索引器逻辑
        string? primaryPath = trigger switch
        {
            CPathSyncTrigger.AbsolutePath => _absolutePath,
            CPathSyncTrigger.RelativePath => _relativePath,
            _ => throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null)
        };
        
        // 为空则抛出异常，说明作为trigger的参数出现问题
        ArgumentNullException.ThrowIfNull(primaryPath, nameof(primaryPath));
        
        // 更新所有字段
        _absolutePath = primaryPath;
        _base = Base; // Base.getter 反推base或者返回工作目录
        _relativePath = Path.GetRelativePath(_base, primaryPath);
        _uri = new Uri(primaryPath);
        _parentPath = PathOperation.GetParentPath(primaryPath);
        _name = PathOperation.GetNameFromPath(_absolutePath);
        return this;
    }

    /// <summary>获取绝对路径</summary>
    public override string ToString() => _absolutePath ?? string.Empty;
}