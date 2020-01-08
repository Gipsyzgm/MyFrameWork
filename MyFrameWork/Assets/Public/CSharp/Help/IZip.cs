using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * 
 * 
 */
public class IZip : MonoBehaviour {


    //压缩文件夹
    public static void ZipDirectory(string dir, string dirTo,string zipName = "", string cipher = "")
    {
        if (zipName == string.Empty)
        {
            zipName = new DirectoryInfo(dir).Name;
        }
        string ZipFileName = dirTo + "/" + zipName + ".zip";

        using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipFileName))
        {
            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
            {
                if(!cipher.Equals(string.Empty))
                {
                    s.Password = cipher;
                }
                ZipSetp(dir, s, zipName);
            }
        }
    }
    private static void ZipSetp(string dir, ZipOutputStream s, string parentPath)
    {
        Crc32 crc = new Crc32();

        DirectoryInfo direction = new DirectoryInfo(dir);
        FileSystemInfo[] files = direction.GetFileSystemInfos();

        for (int i=0;i< files.Length;i++)
        {
            string fullName = files[i].FullName.Replace(@"\", "/");
            string name = files[i].Name;

            if (files[i] is DirectoryInfo)
            {
                ZipSetp(files[i].FullName,s,parentPath + "/" + name);
            }
            else
            {
                using (FileStream fs = File.OpenRead(fullName))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    string fileName = parentPath + "/" + name;
                    ZipEntry entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }

    //解压包
    public static void UnZip(string zipFile, string zipToDir, ref float progress, Action<bool> unZipAction, string password = "")
    {
        long bytesLength = new FileInfo(zipFile).Length;
        long curLength = 0;
        ZipEntry theEntry;
        try
        {
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipFile)))
            {
                if (password != string.Empty)
                    zipStream.Password = password;
                //theEntry.Name                        所有压缩包里面物体的相对压缩包的路径
                //Path.GetDirectoryName(theEntry.Name) 物体所在的文件夹的名称
                //Path.GetFileName(theEntry.Name)      物体的名称
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    curLength += zipStream.Length;
                    progress = curLength / (bytesLength + 0.0f);

                    string dirFile = zipToDir + Path.GetDirectoryName(theEntry.Name) + "/";
                    if (!Directory.Exists(dirFile))
                        Directory.CreateDirectory(dirFile);

                    string fileName = Path.GetFileName(theEntry.Name);

                    using (FileStream streamWriter = File.Create(dirFile + fileName))
                    {
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = zipStream.Read(data, 0, data.Length);
                            if (size > 0)
                                streamWriter.Write(data, 0, size);
                            else
                                break;
                        }
                        streamWriter.Close();
                    }
                }
                zipStream.Close();
                if (unZipAction != null)
                    unZipAction(true);
            }
        }
        catch(Exception ex)
        {
            if (unZipAction != null)
                unZipAction(false);
            Debug.Log("解压出错:" + ex);
        }      
    }
}
