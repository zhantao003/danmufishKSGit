﻿using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// Des 的摘要说明
/// </summary>
public class CEncryptHelper
{
    public static string KEY = "";
    public static string IV = "";

    public static string TBLKEY = "3CpeuchMhymM7Gr8";
    public static string TBLIV = "qTjzKIjBIwBOzUwE";

    public static string ASSETKEY = "ayb6jc8om43a9wce";

    public static string AesEncrypt(string str)
    {
        Encoding encoder = Encoding.UTF8;
        var toEncryptBytes = Encoding.UTF8.GetBytes(str);
        using (var provider = new AesCryptoServiceProvider())
        {
            provider.Key = encoder.GetBytes(KEY);
            provider.Mode = CipherMode.CBC;
            provider.Padding = PaddingMode.PKCS7;
            provider.IV = encoder.GetBytes(IV);
            using (var encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <param name="IVString"></param>
    /// <returns></returns>
    public static string AesDecrypt(string str)
    {
        Encoding encoder = Encoding.UTF8;
        using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
        {
            aes.Key = encoder.GetBytes(KEY);
            aes.IV = encoder.GetBytes(IV);
            var enc = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                {
                    var bts = Convert.FromBase64String(str);
                    // UnityEngine.Debug.Log(bts.Length);
                    cs.Write(bts, 0, bts.Length);
                }
                return encoder.GetString(ms.ToArray());
            }
        }
    }

    public static string AesEncryptTBL(string str)
    {
        Encoding encoder = Encoding.UTF8;
        var toEncryptBytes = Encoding.UTF8.GetBytes(str);
        using (var provider = new AesCryptoServiceProvider())
        {
            provider.Key = encoder.GetBytes(TBLKEY);
            provider.Mode = CipherMode.CBC;
            provider.Padding = PaddingMode.PKCS7;
            provider.IV = encoder.GetBytes(TBLIV);
            using (var encryptor = provider.CreateEncryptor(provider.Key, provider.IV))
            {
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(toEncryptBytes, 0, toEncryptBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    public static string AesDecryptTBL(string str)
    {
        Encoding encoder = Encoding.UTF8;
        using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
        {
            aes.Key = encoder.GetBytes(TBLKEY);
            aes.IV = encoder.GetBytes(TBLIV);
            var enc = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                {
                    var bts = Convert.FromBase64String(str);
                    // UnityEngine.Debug.Log(bts.Length);
                    cs.Write(bts, 0, bts.Length);
                }
                return encoder.GetString(ms.ToArray());
            }
        }
    }

    public static byte[] FromBCDString(string buffer)
    {
        if (buffer == null) return null;
        int start = 0;
        int count = buffer.Length;
        bool inCase = false;
        byte cur = 0;
        int dataEnd = start + count;
        List<byte> lst = new List<byte>(count / 2);
        while (start < dataEnd)
        {
            byte num = (byte)buffer[start++];
            if (num == ' ' || num == '\r' || num == '\n' || num == '\t')
            {
                if (inCase)
                {
                    lst.Add((byte)(cur / 16));
                    inCase = false;
                }
                continue;
            }
            byte tmp = 0;
            if (num >= '0' && num <= '9')
                tmp = (byte)(num - '0');
            else if (num >= 'a' && num <= 'f')
                tmp = (byte)(num - 'a' + 10);
            else if (num >= 'A' && num <= 'F')
                tmp = (byte)(num - 'A' + 10);
            else
                throw new ArgumentException("需要传入一个正确的BCD字符串，BCD字符串中只能包含 0-9 A-F a-f 和空格，回车 制表符!");
            if (!inCase)
            {
                cur = (byte)(tmp * 16);
                inCase = true;
            }
            else
            {
                cur += tmp;
                inCase = false;
                lst.Add(cur);
            }
        }
        if (inCase)
        {
            lst.Add((byte)(cur / 16));
            inCase = false;
        }
        return lst.ToArray();
    }
    public static string ToBCDStringLower(byte[] buffer)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            sb.Append(buffer[i].ToString("x2"));
        }
        return sb.ToString();//result;
    }
}