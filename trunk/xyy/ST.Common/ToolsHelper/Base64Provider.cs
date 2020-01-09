using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ST.Common.ToolsHelper
{
    /// <summary>
    /// 实现Base64编码与其它编码转换的类
    /// </summary>
    public class Base64Provider
    {
        /// <summary>
        /// 加密密钥。
        /// </summary>
        public const string KEY = "st2018speakingPC";

        /// <summary>
        /// 加密初始化向量。
        /// </summary>
        public const string IV = "st2018speakingPC";

        private Base64Provider()
        {
        }
        /// <summary>
        /// 将其它编码的字符串转换成Base64编码的字符串
        /// </summary>
        /// <param name="source">要转换的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64String(string source)
        {
            //如果字符串为空或者长度为0则抛出异常
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source", "不能为空。");
            }
            else
            {
                //将字符串转换成UTF-8编码的字节数组
                byte[] buffer = Encoding.UTF8.GetBytes(source);
                //将UTF-8编码的字节数组转换成Base64编码的字符串
                string result = Convert.ToBase64String(buffer);
                return result;
            }
        }
        /// <summary>
        /// 将Base64编码的字符串转换成其它编码的字符串
        /// </summary>
        /// <param name="result">要转换的Base64编码的字符串</param>
        /// <returns></returns>
        public static string DecodeBase64String(string result)
        {
            //如果字符串为空或者长度为0则抛出异常
            if (string.IsNullOrEmpty(result))
            {
                throw new ArgumentNullException("result", "不能为空。");
            }
            else
            {
                //将字符串转换成Base64编码的字节数组
                byte[] buffer = Convert.FromBase64String(result);
                //将字节数组转换成UTF-8编码的字符串
                string source = Encoding.UTF8.GetString(buffer);
                return source;
            }
        }

        /// <summary>
        /// 解密 Base64 加密串。
        /// </summary>
        /// <param name="decryptString">需要解密的源字符串。</param>
        /// <param name="decryptKey">加密 Key。</param>
        /// <param name="decryptVector">偏移量。</param>
        /// <returns></returns>
        public static string Decrypt(string decryptString, string decryptKey, string decryptVector)
        {
            if (string.IsNullOrEmpty(decryptString))
                return string.Empty;

            Encoding byteEncoder = Encoding.UTF8;

            byte[] rijnKey = byteEncoder.GetBytes(decryptKey);
            byte[] rijnIV = byteEncoder.GetBytes(decryptVector);

            string decryption = "";

            RijndaelManaged rijn = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(decryptString)))
            {
                using (ICryptoTransform decryptor = rijn.CreateDecryptor(rijnKey, rijnIV))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                            decryption = swDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            rijn.Clear();

            decryption = decryption.Trim();

            //清除解密后乱码
            if (decryption.Last() == '}')
            {
                return decryption;
            }
            else
            {
                int lastIndex = decryption.LastIndexOf('}');
                decryption = decryption.Remove(lastIndex + 1);
                return decryption;
            }
        }
        /// <summary>
        /// 将指定的 AES 加密字符串进行解密。
        /// </summary>
        /// <param name="str">加密字符串。</param>
        /// <param name="key">加密的密钥。</param>
        /// <param name="iv">密钥。</param>
        /// <returns>解密后的字符串。</returns>
        public static string AESDecrypt(string str, string key, string iv)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            Encoding byteEncoder = Encoding.UTF8;

            byte[] rijndaelKey = byteEncoder.GetBytes(key);
            byte[] rijndaelIV = byteEncoder.GetBytes(iv);

            string decryption = String.Empty;

            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str)))
            {
                using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(rijndaelKey, rijndaelIV))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            decryption = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            rijndaelManaged.Clear();

            //decryption = decryption.Trim();

            // 清除解密后乱码
            if (decryption.IndexOf("\0") > -1)
            {
                return decryption.Replace("\0","").Trim();
            }
            else
            {
                //int lastIndex = decryption.LastIndexOf('}');
                //decryption = decryption.Remove(lastIndex + 1);
                return decryption.Trim();
            }

            //return decryption;
        }

        /// <summary>
        /// 将指定的字符串进行 AES 加密。
        /// </summary>
        /// <param name="str">加密字符串。</param>
        /// <param name="key">加密的密钥。</param>
        /// <param name="iv">密钥。</param>
        /// <returns>加密后的字符串。</returns>
        public static string AESEncrypt(string str, string key, string iv)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (key == null || iv == null || key.Length < 1 || iv.Length < 1)
            {
                throw new ArgumentException("Key/Iv is null.");
            }

            Encoding byteEncoder = Encoding.UTF8;

            byte[] rgbKey = byteEncoder.GetBytes(key);
            byte[] rgbIV = byteEncoder.GetBytes(iv);

            using (var rijndaelManaged = new RijndaelManaged()
            {
                Key = rgbKey, // 密钥，长度可为128， 196，256比特位
                IV = rgbIV,  //初始化向量(Initialization vector), 用于CBC模式初始化
                //KeySize = 256,//接受的密钥长度
                //BlockSize = 128,//加密时的块大小，应该与iv长度相同
                Mode = CipherMode.CBC,//加密模式
                Padding = PaddingMode.Zeros
            }) //填白模式，对于AES, C# 框架中的 PKCS　＃７等同与Java框架中 PKCS #5
            {
                using (var transform = rijndaelManaged.CreateEncryptor(rgbKey, rgbIV))
                {
                    var inputBytes = Encoding.UTF8.GetBytes(str);//字节编码， 将有特等含义的字符串转化为字节流
                    var encryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);//加密
                    return Convert.ToBase64String(encryptedBytes);//将加密后的字节流转化为字符串，以便网络传输与储存。
                }
            }
        }

        #region 废弃...

        ///// <summary>
        ///// AES 加密。
        ///// </summary>
        ///// <param name="str">加密字符。</param>
        ///// <param name="password">加密的密码。</param>
        ///// <param name="iv">密钥。</param>
        ///// <returns></returns>
        //public static string Encrypt(string str, string password, string iv)
        //{
        //    if (string.IsNullOrEmpty(str))
        //        return string.Empty;

        //    Encoding byteEncoder = Encoding.UTF8;

        //    byte[] rgbKey = byteEncoder.GetBytes(password);
        //    byte[] rgbIV = byteEncoder.GetBytes(iv);

        //    RijndaelManaged rijndaelManaged = new RijndaelManaged
        //    {
        //        Mode = CipherMode.CBC,
        //        Padding = PaddingMode.Zeros
        //    };

        //    string encryption = String.Empty;

        //    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
        //    {
        //        using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rgbKey, rgbIV))
        //        {
        //            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader streamReader = new StreamReader(cryptoStream))
        //                {
        //                    encryption = streamReader.ReadToEnd();
        //                }
        //            }
        //        }
        //    }
        //    rijndaelManaged.Clear();
        //    encryption = encryption.Trim();

        //    //清除解密后乱码
        //    if (encryption.Last() == '}')
        //    {
        //        return encryption;
        //    }
        //    else
        //    {
        //        int lastIndex = encryption.LastIndexOf('}');
        //        encryption = encryption.Remove(lastIndex + 1);
        //        return encryption;
        //    }
        //    //return encryption;
        //}

        ///// <summary>
        ///// AES加密 
        ///// </summary>
        ///// <param name="str">加密字符</param>
        ///// <param name="password">加密的密码</param>
        ///// <param name="iv">密钥</param>
        ///// <returns></returns>
        //public static string AESEncode(string str, string password, string iv)
        //{
        //    RijndaelManaged rijndaelCipher = new RijndaelManaged
        //    {
        //        Mode = CipherMode.CBC,
        //        Padding = PaddingMode.Zeros,
        //        KeySize = 128,
        //        BlockSize = 128
        //    };

        //    byte[] pwdBytes = Encoding.UTF8.GetBytes(password);

        //    byte[] keyBytes = new byte[16];

        //    int len = pwdBytes.Length;

        //    if (len > keyBytes.Length) len = keyBytes.Length;

        //    Array.Copy(pwdBytes, keyBytes, len);

        //    rijndaelCipher.Key = keyBytes;


        //    byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        //    rijndaelCipher.IV = new byte[16];

        //    ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        //    byte[] plainText = Encoding.UTF8.GetBytes(str);

        //    byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

        //    return Convert.ToBase64String(cipherBytes);

        //}
        ///// <summary>
        ///// AES 解密。
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="password"></param>
        ///// <param name="iv"></param>
        ///// <returns></returns>
        //public static string AESDecode(string str, string password, string iv)
        //{
        //    RijndaelManaged rijndaelManaged = new RijndaelManaged
        //    {
        //        Mode = CipherMode.CBC,
        //        Padding = PaddingMode.Zeros,
        //        KeySize = 128,
        //        BlockSize = 128
        //    };

        //    byte[] encryptedData = Convert.FromBase64String(str);
        //    byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
        //    byte[] keyBytes = new byte[16];

        //    int len = pwdBytes.Length;

        //    if (len > keyBytes.Length) len = keyBytes.Length;

        //    Array.Copy(pwdBytes, keyBytes, len);
        //    rijndaelManaged.Key = keyBytes;

        //    byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        //    rijndaelManaged.IV = ivBytes;
        //    ICryptoTransform transform = rijndaelManaged.CreateDecryptor();
        //    byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        //    return Encoding.UTF8.GetString(plainText);

        //}

        #endregion
    }
}
