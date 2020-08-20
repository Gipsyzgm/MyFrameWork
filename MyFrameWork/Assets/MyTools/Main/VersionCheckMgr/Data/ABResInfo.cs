using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class ABResFile
    {
        public string File;
        public string MD5;
        public ulong Size;
        public bool isStreaming = false;       //是否从Streaming目录中读取
        public int Version;                    //所属哪一批次的更新资源

        public string GetFileString()
        {
            return File + "|" + MD5 + "|" + Size + "|" + isStreaming + "|" + Version;
        }
    }
    public class ABResInfo
    {
        public int Version;
        public string VersionData;
        public Dictionary<string, ABResFile> dicFileInfo = new Dictionary<string, ABResFile>();
        /// <summary>
        /// 把ABFiles解析成ABResFile的格式
        /// </summary>
        /// <param name="fileTxt"></param>
        /// <param name="isStreaming"></param>
        public ABResInfo(string fileTxt,bool isStreaming = false)
        {
            string[] line = fileTxt.Split('\n');
            string[] infos = line[0].Split('|');
            Version = Convert.ToInt32(infos[0]);
            VersionData = infos[1];
            for (int i = 1; i < line.Length; i++)
                setFileInfo(line[i], isStreaming);
        }
        /// <summary>
        /// 解析第二行及以后的ABFiles文件
        /// </summary>
        /// <param name="infoStr"></param>
        /// <param name="isStreaming"></param>
        private void setFileInfo(string infoStr,bool isStreaming)
        {
            string[] infos = infoStr.Split('|');
            if (infos.Length < 3) return;
            ABResFile fileInfo = new ABResFile();
            fileInfo.File = infos[0];
            fileInfo.MD5 = infos[1];
            fileInfo.Size = Convert.ToUInt64(infos[2]);
            if (infos.Length == 5) //从persistent中读取的
            {
                fileInfo.isStreaming = Convert.ToBoolean(infos[3]);
                fileInfo.Version = Convert.ToInt32(infos[4]);
            }
            else
            {
                fileInfo.isStreaming = isStreaming;
                if (isStreaming)
                    fileInfo.Version = Version;
            }
            dicFileInfo.Add(fileInfo.File, fileInfo);
        }

        public void Dispose()
        {
            dicFileInfo.Clear();
        }
    }
