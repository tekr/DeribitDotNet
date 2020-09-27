using System;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using Serilog.Events;

namespace DeribitDotNet
{
    public static class SignatureFactory
    {
        private static readonly SHA256Managed Sha256Managed = new SHA256Managed();

        public static string Get(string input)
        {
            var hash = Sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
            var signatureHash = Convert.ToBase64String(hash);

            if (Log.IsEnabled(LogEventLevel.Verbose))
            {
                Log.Verbose($"Sig input: {input}. Hash: {Encoding.UTF8.GetString(hash)}. Base64: {signatureHash}");
            }

            return signatureHash;
        }
    }
}
