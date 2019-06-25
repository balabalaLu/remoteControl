using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace server
{
    /// <summary>
    /// 记录上线主机的基本信息，和基础套接字
    /// </summary>
    class ClientInfo
    {
        String ip;
        /// <summary>
        /// 被控端IP
        /// </summary>
        public String IP
        {
            get { return ip; }
            set { ip = value; }
        }

        String port;
        /// <summary>
        /// 被控端端口号
        /// </summary>
        public String Port
        {
            get { return port; }
            set { port = value; }
        }

        String computerName;
        /// <summary>
        /// 被控端主机名
        /// </summary>
        public String ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        String os;
        /// <summary>
        /// 被控制端的操作系统
        /// </summary>
        public String OS
        {
            get { return os; }
            set { os = value; }
        }

        String cpu;
        /// <summary>
        /// 被控制端的CPU频率
        /// </summary>
        public String CPU
        {
            get { return cpu; }
            set { cpu = value; }
        }

        String memory;
        /// <summary>
        /// 被控制端主机的内存容量
        /// </summary>
        public String Memory
        {
            get { return memory; }
            set { memory = value; }
        }

        Socket socket;
        /// <summary>
        /// 被控端主机的套接字
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
            set { socket = value; }
        }
    }
}
