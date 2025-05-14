using System.Security.Cryptography;

namespace Synx.Common.Verification.Interfaces;

public interface IEncryptGenerator
{
    MD5 Hasher { get; }

    byte[]? HashBytes { get; }

    string HashString { get; }
    
    string Generate(string data);

    string Generate(byte[] data);
    
    Task<string> GenerateFromStreamAsync(Stream stream);
    
    int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount,
        byte[] outputBuffer, int outputOffset);
    
    byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);

    static abstract string ByteTransToString(byte[] bytes);
}