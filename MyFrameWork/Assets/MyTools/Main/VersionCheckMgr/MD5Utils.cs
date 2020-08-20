using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
/// <summary>
/// https://blog.csdn.net/CatchMe_439/article/details/78404195?utm_medium=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.channel_param&depth_1-utm_source=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.channel_param
/// </summary>
public class MD5Utils
{
    /// <summary>
    /// 获取文件的MD5值
    /// </summary>
    /// <param name="fileName">文件全路径</param>
    /// <returns></returns>
    public static string MD5File(string fileName, out long size)
    {
        try
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            size = file.Length;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    /// <param name="filepath">文件全路径</param>
    /// <returns></returns>
    public static string MD5ByteFile(byte[] data)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(data);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        return sb.ToString();
    }
    /// <summary>
    /// 计算字符串的MD5值
    /// </summary>
    /// <param name="sDataIn">字符串</param>
    /// <returns></returns>
    public static string GetMD5(string sDataIn)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bytValue, bytHash;
        bytValue = Encoding.UTF8.GetBytes(sDataIn);
        bytHash = md5.ComputeHash(bytValue);
        md5.Clear();
        string sTemp = "";
        for (int i = 0; i < bytHash.Length; i++)
        {
            sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
        }
        return sTemp.ToLower();
    }

}