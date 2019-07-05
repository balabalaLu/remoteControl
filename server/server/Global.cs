using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace server
{
    class Global
    {
        public static int onlineNumber = 0;//上线主机数量
        public static int port = 9999;     //管理端的端口号
        public static int clientPort = 6666;//被控制端的端口号
        public static bool isListenPort = true; //是否正在监听
		public static int FilePort = 7777;//本地文件的传输端口
		public static int remoteFilePort = 7771;//被控制端文件传输端口
        public static List<ClientInfo> OnlineClientArr = new List<ClientInfo>();//保存上线主机列表
    }
}
