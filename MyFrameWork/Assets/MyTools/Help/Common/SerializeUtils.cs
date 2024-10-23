using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SerializeUtils
{
    public static object DeSerializeBinaryFile(string fileName)
    {
        byte[] bytes = File.ReadAllBytes(fileName);
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            return DeSerializeBinary(stream);
        }
    }

    /// <summary>
    /// 反序列化二进制字节流
    /// </summary>
    /// <returns>The serialize binary.</returns>
    /// <param name="stream">Stream.</param>
    public static object DeSerializeBinary(Stream stream)
    {
        object result = null;
        if (stream != null)
        {
            try
            {
                //设置类型转化代理
                //Unity一些特定类无法直接序列化，如：Vector3，Color

                BinaryFormatter deserializer = new BinaryFormatter();
                result = deserializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                Debug.LogError("反序列化失败(Binary)" + e.Message);
            }
        }
        return result;
    }


    /// <summary>
    /// 二进制序列化(需要给类标注[Serializable])
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static byte[] SerializeBinary(object obj)
    {
        MemoryStream stream = null;
        byte[] binary = null;
        try
        {
            stream = new MemoryStream();

            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(stream, obj);

            stream.Position = 0;

            int length = (int)stream.Length;
            binary = new byte[length];
            stream.Read(binary, 0, length);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
        return binary;
    }
}
