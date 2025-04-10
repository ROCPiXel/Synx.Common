namespace Synx.Common.Enums;

/// <summary>
/// CreationMethod: enum
/// 操作文件时的方式
/// </summary>
public enum CreationMethod
{
    /// <summary>保持，默认方法</summary>
    Keep = 0,
    
    /// <summary>覆盖，极其危险的操作，建议进行二次确认</summary>
    Cover = 1,
    
    /// <summary>新建，如果目标已经存在会新建唯一路径，其余保持默认</summary>
    New = 2,
}

public enum OpenWithApp
{
    None = 0,
}

public enum FileFormat
{
    exe,
    dll,
    com,
    cmd,
    ps1,
    text,
    markdown,
    html,
    js,
    db,
    mp4,
    mp3,
    srt,
    lrf,
    jpg,
    png,
    dng,
    arw,
    psd,
    psb,
    Null,
    Unknown
}

public enum FileObjectType
{
    Path = 0,
    File = 1,
    Directory = 2,
    Drive = 3
}