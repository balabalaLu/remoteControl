using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace server
{
	public partial class FileManager : Form
	{
		public delegate void myUI();//委托
		bool isListeningSocket = true;//是否持续监听客户端
		String title = "";
		Socket clientSocket;
		Socket fileSocket;
		TcpClient Client;            //用于连接被控端
		TcpClient FileClient;
		TcpListener LisFile;
		MainForm MF;
		String localDiskList = "";
		String FolderList = "";
		String FileList = "";
		TreeNode selNode;//被选中的左侧的某个treeNode
		String clientIP;          //被控端IP
		NetworkStream Ns;
		NetworkStream fileNs;
		String SelectedFileListName = "";//选中的文件夹
		String selectedFileName = "";//选中的某一个文件
		String selectedFilePath = "";//选中的某一个文件的路径
		String upLoadFilePath = "";//本地选中的需要上传的文件
		String upLoadFileName = "";
		String savePath = "";//本地下载需要存储的文件路径
		String HintFilename = "";
		String HintFilepath = "";

		public FileManager(String Ip, Socket clientSocket, MainForm MF)
		{
			InitializeComponent();
			this.title = "文件管理-" + Ip;
			this.clientIP = Ip;
			this.clientSocket = clientSocket;
			this.MF = MF;
		}


		/// <summary>
		/// 加载窗体
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void File_Manager_Load(object sender, EventArgs e)
		{
			try
			{
				Client = new TcpClient();
				Client.Connect(this.clientIP, Global.clientPort);

				//如果连接上了
				if (Client.Client.Connected)
				{
					//重定向SOCKET 得到被控端套接字句柄
					this.clientSocket = this.Client.Client;
					//窗体加载时默认列举被控端电脑盘符
					this.Ns = new NetworkStream(this.clientSocket);
					//命令原型 ： $GetDir (没有参数1的情况下返回当前主机所有盘符)
					this.Ns.Write(Encoding.Default.GetBytes("$GetDir"), 0, Encoding.Default.GetBytes("$GetDir").Length);
					this.Ns.Flush();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("尝试连接被控端发生错误 : " + ex.Message);
			}
			try
			{
				Thread thread = new Thread(new ThreadStart(this.listenSocket));
				thread.Start();
				Thread thread1 = new Thread(new ThreadStart(this.listenFileSocket));
				thread1.Start();
			}
			catch { };
		}
		/// <summary>
		/// 持续监听客户端发送的请求
		/// </summary>
		public void listenSocket()
		{
			while (isListeningSocket)
			{
				//接收命令
				using (NetworkStream ns = new NetworkStream(this.clientSocket))
				{
					try
					{
						byte[] receive = new byte[1024];
						int resLen = ns.Read(receive, 0, receive.Length);
						String[] orderSet = Encoding.Default.GetString(receive, 0, resLen).Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
						this.orderCatcher(orderSet);
					}
					catch { }
				}
			}
		}
		/// <summary>
		/// 监听本地7777
		/// </summary>
		public void listenFileSocket()
		{
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			LisFile = new TcpListener(ipAddress, Global.FilePort);
			while (true)
			{
				try
				{
					LisFile.Start();//开始监听
					this.fileSocket = LisFile.AcceptSocket();//客户端请求则创建套接字
					this.fileNs = new NetworkStream(this.fileSocket);
					//创建线程
					Thread thread = new Thread(new ThreadStart(this.Res_File));
					thread.Start();
				}
				catch { }
			}
		}
		/// <summary>
		/// 解析命令,调用不同的方法
		/// </summary>
		public void orderCatcher(String[] orderSet)
		{
			switch (orderSet[0])
			{
				//硬盘驱动器集合
				case "$GetDir":
					this.localDiskList = orderSet[1];
					this.BeginInvoke(new myUI(this.GetLocalDistList));
					break;
				//子文件夹列表
				case "$GetFolder":
					this.FolderList = orderSet[1];
					this.BeginInvoke(new myUI(this.GetFolderList));
					break;
				//子文件列表
				case "$GetFile":
					this.FileList = orderSet[1];
					this.BeginInvoke(new myUI(this.GetFileList));
					break;

			}
		}
		/// <summary>
		/// 拿到磁盘的列表
		/// </summary>
		public void GetLocalDistList()
		{
			//得到硬盘盘符数组
			String[] localDiskList = this.localDiskList.Split(',');
			this.dirListTree.Nodes.Clear();
			for (int i = 0; i < localDiskList.Length; i++)
			{
				//分解名字列表
				String[] Names = localDiskList[i].Split('#');
				if (Names.Length > 1)
				{
					//1.结点名称2.结点显示的文本3.树结点中显示的图像索引
					this.dirListTree.Nodes.Add(Names[1] + "\\", Names[0] + " " + Names[1], 1);
					//给Tag赋值
					this.dirListTree.Nodes[Names[1] + "\\"].Tag = Names[1] + "\\";
				}
			}
		}
		/// <summary>
		/// 拿到文件夹的列表
		/// </summary>
		public void GetFolderList()
		{
			String[] FolderList = this.FolderList.Split(',');
			this.selNode.Nodes.Clear();
			this.FileListView.Items.Clear();
			for (int i = 0; i < FolderList.Length; i++)
			{
				if (FolderList[i] != "")
				{
					//拿到最后的文件夹名
					String Read_Name = FolderList[i].Substring(FolderList[i].LastIndexOf("\\") + 1, FolderList[i].Length - (FolderList[i].LastIndexOf("\\") + 1));
					//设置此节点的名称为Read_Name，设置状态为已经选中
					this.selNode.Nodes.Add(FolderList[i] + "\\", Read_Name, 2);
					//设置此节点的tag为全路径
					this.selNode.Nodes[FolderList[i] + "\\"].Tag = FolderList[i] + "\\";
					//加入右侧列表
					this.FileListView.Items.Add(FolderList[i] + "\\", Read_Name, 0);
				}
			}
		}
		/// <summary>
		/// 拿到文件的列表
		/// </summary>
		public void GetFileList()
		{
			//得到分离后的文件数组结构
			String[] FileList = this.FileList.Split(',');
			for (int i = 0; i < FileList.Length; i++)
			{
				if (FileList[i] != "")
				{
					String Read_Name = FileList[i].Substring(FileList[i].LastIndexOf("\\") + 1, FileList[i].Length - (FileList[i].LastIndexOf("\\") + 1));
					//打入文件列表
					this.FileListView.Items.Add(FileList[i], Read_Name, 1);
				}
			}
		}

		/// <summary>
		/// 双击
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dirlistTreeNodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			this.selNode = e.Node;
			//发送列目录命令请求
			//原型 : $GetFolder|[参数1]  (列举参数1的目录)
			this.Ns.Write(Encoding.Default.GetBytes("$GetFolder||" + e.Node.Tag), 0, Encoding.Default.GetBytes("$GetFolder||" + e.Node.Tag).Length);
			this.Ns.Flush();
			//发送列文件命令请求
			//原型 : $GetFile|[参数1]  (列举参数1的目录)
			this.Ns.Write(Encoding.Default.GetBytes("$GetFile||" + e.Node.Tag), 0, Encoding.Default.GetBytes("$GetFile||" + e.Node.Tag).Length);
			this.Ns.Flush();
			//设置当前选中的文件夹的名称
			this.SelectedFileListName = Convert.ToString(e.Node.Tag);
		}
		/// <summary>
		/// 右键菜单操作
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NodeRightMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			switch (e.ClickedItem.Text)
			{
				case "上传":
					OpenFileDialog ofDialog = new OpenFileDialog();
					if (ofDialog.ShowDialog(this) == DialogResult.OK)
					{
						this.upLoadFileName = ofDialog.SafeFileName; //文件名
						//txtFileName.Text = fileName;      
						this.upLoadFilePath = ofDialog.FileName;     //获取包含文件名的全路径
						this.UpLoadSelectedFile();
					}
					break;
				case "下载":
					FolderBrowserDialog path = new FolderBrowserDialog();
					path.ShowDialog();
					this.savePath = path.SelectedPath;
					//发送选中的文件全路径给被控制端
					this.Ns.Write(Encoding.Default.GetBytes("$DownLoadFile||" + this.selectedFilePath), 0, Encoding.Default.GetBytes("$DownLoadFile||" + this.selectedFilePath).Length);
					this.Ns.Flush();
					break;
				default:
					break;
			}
		}
		/// <summary>
		/// 右键文件上传
		/// </summary>
		public void UpLoadSelectedFile()
		{
			//发送文件之前 将和长度发送过去
			long fileLength = new FileInfo(this.upLoadFilePath).Length;
			string totalMsg =string.Format("{0}-{1}", Path.Combine(selectedFilePath,upLoadFileName), fileLength);
			//添加标识
			byte[] sendMsg=new Byte[1];
			sendMsg[0] = 2;
			byte[] byteMsg = Encoding.Default.GetBytes(totalMsg);
			sendMsg=sendMsg.Concat(byteMsg).ToArray();
			this.fileNs.Write(sendMsg, 0, sendMsg.Length);
			this.fileNs.Flush();
			//发送文件
			byte[] buffer = new byte[1024];
			using (FileStream fs = new FileStream(upLoadFilePath, FileMode.Open, FileAccess.Read))
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
		/// 右边的FileListView被右击的时候
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ItemRightClicked(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				this.NodeRightMenu.Show(FileListView, e.Location);//鼠标右键按下弹出菜单
			}
			//存储选中的文件名
			if (this.FileListView.SelectedItems.Count > 0)
			{
				this.selectedFileName = this.FileListView.SelectedItems[0].Text.Trim();
				this.selectedFilePath = Path.Combine(this.SelectedFileListName, this.selectedFileName);
			}
		}
		/// <summary>
		/// 文件下载时
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
					if (fileSocket != null)
						firstReceived = fileSocket.Receive(buffer);
					if (firstReceived > 0) //接受到的长度大于0 说明有信息或文件传来
					{
						if (buffer[0] == 2)//2为文件名字和长度
						{
							string revFileLength = Encoding.UTF8.GetString(buffer, 1, firstReceived - 1);
							fileLength = Convert.ToInt64(revFileLength);
						}
						if (buffer[0] == 1)//1为文件
						{

							int received = 0;
							long receivedTotalFilelength = 0;
							bool firstWrite = true;
							using (FileStream fs = new FileStream(Path.Combine(savePath,this.selectedFileName), FileMode.Create, FileAccess.Write))
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

							HintFilename = this.selectedFileName; //文件名 不带路径
							HintFilepath = savePath; //文件路径 不带文件名
							
						}

					}
				}
				catch (Exception ex)
				{
				}
			}
		}
	}
}
