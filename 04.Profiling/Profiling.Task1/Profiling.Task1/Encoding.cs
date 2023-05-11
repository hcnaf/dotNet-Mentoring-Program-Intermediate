﻿using System.Security.Cryptography;

namespace Profiling.Task1;


internal static class Encoding
{
    public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
    {

        var iterate = 10000;
        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        var passwordHash = Convert.ToBase64String(hashBytes);

        return passwordHash;

    }
    public static string GeneratePasswordHashUsingSalt_Span(string passwordText, byte[] salt)
    {
        var iterate = 10000;
        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        Span<byte> hashBytes = stackalloc byte[36];
        salt.CopyTo(hashBytes[0..16]);
        hash.CopyTo(hashBytes[16..36]);

        var passwordHash = Convert.ToBase64String(hashBytes);

        return passwordHash;
    }
}