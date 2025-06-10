using System.Security.Cryptography;
using System.Text;
using Synx.Common.Verification.Interfaces;

namespace Synx.Common.Verification.Generator;

public class Md5Generator : IEncryptGenerator
{
    // TODO: 单例实现大统一
    private static readonly Lazy<Md5Generator> _instance 
        = new Lazy<Md5Generator>(() => new Md5Generator());
    
    public static Md5Generator Instance => _instance.Value;

    private MD5? _hasher;

    public MD5 Hasher
    {
        get
        {
            if(_hasher is null) return _hasher = MD5.Create();
            return _hasher;
        }
    }

    public byte[]? HashBytes => Hasher.Hash;

    public string HashString => ByteTransToString(HashBytes);
    
    public string Generate(string data) => Generate(Encoding.UTF8.GetBytes(data));

    public string Generate(byte[] data)
    {
        var dataBytes = Hasher.ComputeHash(data);
        return ByteTransToString(dataBytes);
    }

    public async Task<string> GenerateFromStreamAsync(Stream stream)
    {
        var dataBytes = await Hasher.ComputeHashAsync(stream);
        return ByteTransToString(dataBytes);
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        => Hasher.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
        throw new NotImplementedException();
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        => Hasher.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

    public static string ByteTransToString(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);
        var sBuilder = new StringBuilder();
        foreach (var t in bytes)
        {
            sBuilder.Append(t.ToString("x2"));
        }
        return sBuilder.ToString();
    }
}