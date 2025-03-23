using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Synx.Common.Verification;

public class VerifyCode
{
    public string MD5 { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string Base64 { get; set; } = string.Empty;
    public string Base32 { get; set; } = string.Empty;
}