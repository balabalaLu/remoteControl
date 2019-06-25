using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
	class Global
	{
		public static int Port = 9999;                      //默认上线端口
		public static String Host = "127.0.0.1";            //默认主控端地址
		public static bool isListenPort = true;           //是否监听端口
		public static int lisPort = 6666;                  //自身监听端口
		public static int FilePort = 7771;       //文件传输的端口s
		public static int remoteFilePort = 7777;//远程文件传输端口
	}
}
