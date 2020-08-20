using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class VersionInfo
    {
        /// <summary>平台名称</summary>
        public string Platform;
        /// <summary>APP版本号</summary>
        public string AppVersion;
        /// <summary>资源版本号</summary>
        public int ResVersion;
        /// <summary>是否强制更新</summary>
        public bool IsForcedUpdate;
        /// <summary>是否为提审版本</summary>
        public bool IsCheckVer;
        /// <summary>资源地址</summary>
        public string ResURL;
        /// <summary>APP下载包地址</summary>
        public string AppDownloadURL;
    }
