using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;  //加入WMI
using System.IO;
using System.Net;

namespace client
{
	public partial class MainForm : Form
	{
		TcpClient Client;
		TcpClient FileClient;
		TcpListener Lis;
		TcpListener LisFile;
		Socket socket;
		Socket fileSocket;
		NetworkStream stream;
		NetworkStream fileStream;
		Socket Lis_socket;
		//Socket Lis_fileSocket;
		String localDiskList = "$GetDir||";                     //电脑盘符命令，初始化命令头
		String onlineOrder = "$Online||";                     //上线命令，初始化命令头部
		String folderList = "$GetFolder||";                  //列举子文件夹命令，初始化命令头
		String fileList = "$GetFile||";                    //列举文件命令，初始化命令头
		String savePath = "";//本地的路径
		String receivedFileName = "";//需要接收的文件名
		public delegate void myUI();
		String HintFilename = "";
		String HintFilepath = "";

		public MainForm()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 窗体加载时默认连接主控端主机
		/// 如果连接成功则发送上线请求
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Main_Form_Load(object sender, EventArgs e)
		{
			
		}
		#region  上线请求
		/// <summary>
		/// 此方法通过Windows WMI 服务,查询此客户端的信息
		/// </summary>
		public void Get_ComputerInfo()
		{
			//查询计算机名
			this.onlineOrder += this.WMI_Searcher("SELECT * FROM Win32_ComputerSystem", "Caption") + "||";
			//查询操作系统
			this.onlineOrder += this.WMI_Searcher("SELECT * FROM Win32_OperatingSystem", "Caption") + "||";
			//查询CPU
			this.onlineOrder += this.WMI_Searcher("SELECT * FROM Win32_Processor", "Caption") + "||";
			//查询内存容量 - 单位: MB
			this.onlineOrder += (int.Parse(this.WMI_Searcher("SELECT * FROM Win32_OperatingSystem", "TotalVisibleMemorySize")) / 1024) + " MB||";
		}


		/// <summary>
		/// 此方法根据指定语句通过WMI查询用户指定内容
		/// 并且返回
		/// </summary>
		/// <param name="QueryString"></param>
		/// <param name="Item_Name"></param>
		/// <returns></returns>
		public String WMI_Searcher(String QueryString, String Item_Name)
		{
			String Result = "";
			ManagementObjectSearcher MOS = new ManagementObjectSearcher(QueryString);
			ManagementObjectCollection MOC = MOS.Get();
			foreach (ManagementObject MOB in MOC)
			{
				Result = MOB[Item_Name].ToString();
				break;
			}
			MOC.Dispose();
			MOS.Dispose();
			return Result;
		}

		/// <summary>
		/// 此方法用于向主控端发送上线请求 
		/// 命令原型 : $Online||计算机名||操作系统||CPU频率||内存容量
		/// </summary>
		public void postOnlineMessage()
		{
			this.Client = new TcpClient();
			this.FileClient = new TcpClient();
			//多次尝试连接
			while (Global.isListenPort)
			{
				try
				{
					this.Client.Connect(Global.Host, Global.Port);
				}
				catch
				{ }
				if (this.Client.Connected)
					break;
			}
			//如果连接上了
			if (this.Client.Connected)
			{
				//弹框点击确定
				if (this.bombInfo() == 1)
				{
					//得到套接字原型
					this.socket = this.Client.Client;
					this.stream = new NetworkStream(this.socket);
					
					//发送上线请求
					this.stream.Write(Encoding.Default.GetBytes(this.onlineOrder), 0, Encoding.Default.GetBytes(this.onlineOrder).Length);
					this.stream.Flush();
					//如果请求发出后套接字仍然处于连接状态
					//则单劈出一个线程，用于接收命令
					if (this.socket.Connected)
					{
						Thread thread = new Thread(new ThreadStart(this.Get_Server_Order));
						thread.Start();
						//新开一个线程，试图连接远程7777端口
						Thread thread1 = new Thread(new ThreadStart(this.ConnFilePort));
						thread1.Start();
					}
				}
				
			}
		}
		/// <summary>
		/// 多次试图连接远程的7777端口
		/// </summary>
		public void ConnFilePort()
		{
			//多次尝试连接
			while (true)
			{
				try
				{
					this.FileClient.Connect(Global.Host, Global.remoteFilePort);
				}
				catch
				{ }
				if (this.FileClient.Connected)
					break;
			}
			if (this.FileClient.Connected)
			{
				this.fileSocket = this.FileClient.Client;
				this.fileStream = new NetworkStream(this.fileSocket);
			}
			Thread thread = new Thread(new ThreadStart(this.Res_File));
			thread.Start();
		}
		public int bombInfo()
		{
			//跳出弹框
			//label1.Text = "";
			DialogResult MsgBoxResult;//设置对话框的返回值
			MsgBoxResult = MessageBox.Show("远程主机127.0.0.1想要对您进行远程管理", "提示",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Information,
			MessageBoxDefaultButton.Button2);
			if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
			{
				//this.label1.ForeColor = System.Drawing.Color.Red;
				//label1.Text = " 你选择了按下”Yes“的按钮！"
				return 1;

			}
			return 0;
		}
		#endregion

		#region  监听端口，处理命令
		/// <summary>
		/// 此方法用于监听接收信息的端口
		/// </summary>
		public void Listen_Port()
		{
			while (Global.isListenPort)
			{
				this.Lis_socket = Lis.AcceptSocket();  //如果有服务端请求则创建套接字
				Thread thread = new Thread(new ThreadStart(this.Res_Message));
				thread.Start();
			}
		}
		/// <summary>
		/// 文件传输端口
		/// </summary>
		/*public void Listen_FilePort()
		{
			while (Global.isListenPort)
			{
				this.Lis_fileSocket = LisFile.AcceptSocket();  //如果有服务端请求则创建套接字
				Thread thread = new Thread(new ThreadStart(this.Res_File));
				thread.Start();
			}
		}*/
		/// <summary>
		/// 此方法用于得到主控端发来的命令集合
		/// </summary>
		public void Get_Server_Order()
		{
			while (true)
			{
				try
				{
					byte[] bb = new byte[1024];
					//接收命令
					int Order_Len = this.stream.Read(bb, 0, bb.Length);
					//得到主控端发来的命令集合
					String[] Order_Set = Encoding.Default.GetString(bb, 0, Order_Len).Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
					this.Order_Catcher(Order_Set);
				}
				catch (Exception ex)
				{ };
			}
		}

		/// <summary>
		/// 此方法负责接收主控端命令
		/// 并且传递到处理方法种
		/// </summary>
		public void Res_Message()
		{
			while (true)
			{
				try
				{
					using (NetworkStream ns = new NetworkStream(this.Lis_socket))
					{
						try
						{
							byte[] bb = new byte[1024];
							//得到命令
							int Res_Len = ns.Read(bb, 0, bb.Length);
							//得到完整命令分割后的数组结构
							String[] Order_Set = Encoding.Default.GetString(bb, 0, Res_Len).Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
							//调用判断命令函数
							//MessageBox.Show(Order_Set[0]);
							this.Order_Catcher(Order_Set);
						}
						catch (Exception ex) { };
					}
				}
				catch (Exception ex)
				{ };
			}
		}
		/// <summary>
		/// 持续监听，接收上传的文件
		/// </summary>
		public void Res_File()
		{
			long fileLength = 0;
			while (true)
			{
				int firstReceived = 0;
				byte[] buffer = new byte[1024];
				try
				{
					//获取接收的数据,并存入内存缓冲区  返回一个字节数组的长度
					if (fileSocket!= null)
						firstReceived = fileSocket.Receive(buffer);
					if (firstReceived > 0) //接受到的长度大于0 说明有信息或文件传来
					{
						if (buffer[0] == 2)//2为文件名字和长度
						{
							string fileNameWithLength = Encoding.UTF8.GetString(buffer, 1, firstReceived - 1);
							savePath = fileNameWithLength.Split('-').First(); //文件保存的路径
							fileLength = Convert.ToInt64(fileNameWithLength.Split('-').Last());//文件长度
						}
						if (buffer[0] == 1)//1为文件
						{

							int received = 0;
							long receivedTotalFilelength = 0;
							bool firstWrite = true;
							using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
							{
								while (receivedTotalFilelength < fileLength) //之后收到的文件字节数组
								{
									if (firstWrite)
									{
										fs.Write(buffer, 1, firstReceived - 1); //第一次收到的文件字节数组 需要移除标识符1 后写入文件
										fs.Flush();
										receivedTotalFilelength += firstReceived - 1;
										firstWrite = false;
										continue;
									}
									received = fileSocket.Receive(buffer); //之后每次收到的文件字节数组 可以直接写入文件
									fs.Write(buffer, 0, received);
									fs.Flush();
									receivedTotalFilelength += received;
								}
								fs.Close();
							}

							HintFilename = savePath.Substring(savePath.LastIndexOf("\\") + 1); //文件名 不带路径
							HintFilepath = savePath.Substring(0, savePath.LastIndexOf("\\")); //文件路径 不带文件名
							this.BeginInvoke(new myUI(this.MsgHint));
						}

					}
				}
				catch (Exception ex)
				{
					
				}
			}
		}
		/// <summary>
		/// 消息提示
		/// </summary>
		public void MsgHint()
		{
			this.listView1.Items.Add(DateTime.Now.ToLongTimeString().ToString() + "\r\n您成功接收了文件" + HintFilename + "\r\n保存路径为:" + HintFilepath + "\r\n");
		}

		/// <summary>
		/// 此方法用于判断命令结构
		/// 根据不同的命令调用不同的方法进行处理
		/// </summary>
		/// <param name="Order_Set"></param>
		public void Order_Catcher(String[] Order_Set)
		{
			switch (Order_Set[0])
			{
				//此命令头表示客户端状态结果返回
				case "$Return":
					switch (Order_Set[1])
					{
						//如果是上线成功
						case "#Online_OK":
							this.Online_OK();
							break;
					}
					break;
				//此命令头表示客户端请求本机所有盘符
				case "$GetDir":
					this.getLocalDisk();
					break;
				//此命令头表示客户端请求本机指定目录下的所有文件夹
				case "$GetFolder":
					this.getFoloder(Order_Set[1]);
					break;
				//此命令头表示客户端请求本机指定目录下的所有文件
				case "$GetFile":
					this.getFile(Order_Set[1]);
					break;
				//文件下载
				case "$DownLoadFile":
					this.sendFile(Order_Set[1]);
					break;
			}
		}
		#endregion

		/// <summary>
		/// 上线成功后的用户提示
		/// </summary>
		public void Online_OK()
		{
			//弹框
		}
		

		#region  文件管理
		/// <summary>
		/// 此方法调用Windows WMI
		/// 列举当前电脑所有盘符
		/// </summary>
		public void getLocalDisk()
		{
			this.localDiskList = "$GetDir||";
			ManagementObjectSearcher MOS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
			ManagementObjectCollection MOC = MOS.Get();
			foreach (ManagementObject MOB in MOC)
			{
				this.localDiskList += MOB["Description"].ToString() + "#" + MOB["Caption"].ToString() + ",";
			}
			MOC.Dispose();
			MOS.Dispose();

			try
			{
				//得到硬盘分区列表后，尝试发送
				using (NetworkStream Ns = new NetworkStream(this.Lis_socket))
				{
					Ns.Write(Encoding.Default.GetBytes(this.localDiskList), 0, Encoding.Default.GetBytes(this.localDiskList).Length);
					Ns.Flush();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("尝试发送硬盘分区列表失败 : " + ex.Message);
			}
		}

		/// <summary>
		/// 此方法用于根据指定盘符列举子文件夹
		/// </summary>
		/// <param name="Path"></param>
		public void getFoloder(String Path)
		{
			this.folderList = "$GetFolder||";
			//得到指定盘符的所有子文件夹
			String[] Folder = Directory.GetDirectories(Path);
			for (int i = 0; i < Folder.Length; i++)
			{
				this.folderList += Folder[i] + ",";
			}

			try
			{
				//得到文件夹列表后，尝试发送
				using (NetworkStream Ns = new NetworkStream(this.Lis_socket))
				{
					Ns.Write(Encoding.Default.GetBytes(this.folderList), 0, Encoding.Default.GetBytes(this.folderList).Length);
					Ns.Flush();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("尝试发送文件夹列表失败 : " + ex.Message);
			}
		}
		public void sendFile(String Path)
		{
			//发送文件之前 将长度发送过去
			long fileLength = new FileInfo(Path).Length;
			//添加标识
			byte[] sendMsg = new Byte[1];
			sendMsg[0] = 2;
			byte[] byteMsg = Encoding.Default.GetBytes(fileLength.ToString());
			sendMsg = sendMsg.Concat(byteMsg).ToArray();

			this.fileStream.Write(sendMsg, 0, sendMsg.Length);
			this.fileStream.Flush();
			//发送文件
			byte[] buffer = new byte[1024];
			using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
			{
				int readLength = 0;
				bool firstRead = true;
				long sentFileLength = 0;
				while ((readLength = fs.Read(buffer, 0, buffer.Length)) > 0 && sentFileLength < fileLength)
				{
					sentFileLength += readLength;
					//在第一次发送的字节流上加个前缀1
					if (firstRead)
					{
						byte[] firstBuffer = new byte[readLength + 1];
						firstBuffer[0] = 1; //告诉机器该发送的字节数组为文件
						Buffer.BlockCopy(buffer, 0, firstBuffer, 1, readLength);
						fileSocket.Send(firstBuffer, 0, readLength + 1, SocketFlags.None);
						firstRead = false;
						continue;
					}
					//之后发送的均为直接读取的字节流
					fileSocket.Send(buffer, 0, readLength, SocketFlags.None);
				}
				fs.Close();
			}
		}
		/// <summary>
		/// 此方法用于根据指定盘符列举子所有文件
		/// </summary>
		/// <param name="Path"></param>
		public void getFile(String Path)
		{
			this.fileList = "$GetFile||";
			//得到文件目标文件夹文件数组
			String[] Result_List = Directory.GetFiles(Path);
			//通过拆分得到结果字符串
			for (int i = 0; i < Result_List.Length; i++)
			{
				this.fileList += Result_List[i] + ",";
			}

			try
			{
				//得到文件列表后，尝试发送
				using (NetworkStream Ns = new NetworkStream(this.Lis_socket))
				{
					Ns.Write(Encoding.Default.GetBytes(this.fileList), 0, Encoding.Default.GetBytes(this.fileList).Length);
					Ns.Flush();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("尝试发送文件夹列表失败 : " + ex.Message);
			}
		}
		
		#endregion

		/// <summary>
		/// 窗体关闭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			//下线命令 原型 ： $OffLine||
			String Order = "$OffLine||";
			try
			{
				//尝试发送下线请求
				this.stream.Write(Encoding.Default.GetBytes(Order + ((IPEndPoint)this.socket.LocalEndPoint).Address.ToString()), 0, Encoding.Default.GetBytes(Order + ((IPEndPoint)this.socket.LocalEndPoint).Address.ToString()).Length);
				this.stream.Flush();
			}
			catch (Exception ex)
			{ };
			Environment.Exit(0);
		}

		/// <summary>
		/// 连接主控端主机
		/// 如果连接成功则发送上线请求
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			//调用WMI收集系统信息
			this.Get_ComputerInfo();
			//发送上线请求 - [多线程]
			Thread thread = new Thread(new ThreadStart(this.postOnlineMessage));
			thread.Start();
			//自身监听端口,用于接收信息
			Lis = new TcpListener(Global.lisPort);
			Lis.Start();  //一直监听
			LisFile = new TcpListener(Global.FilePort);
			LisFile.Start();  //一直监听
			Thread thread_Lis_MySelf = new Thread(new ThreadStart(this.Listen_Port));
			thread_Lis_MySelf.Start();
			//Thread thread_Lis_MineFile = new Thread(new ThreadStart(this.Listen_FilePort));
			//thread_Lis_MineFile.Start();
		}
	}

}