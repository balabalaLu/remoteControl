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
		MainForm MF;
		String localDiskList = "";
		String FolderList = "";
		String FileList = "";
		TreeNode selNode;//被选中的左侧的某个treeNode
		TcpClient Client;            //用于连接被控端
		String clientIP;          //被控端IP
		NetworkStream Ns;
		String selectedFileName = "";//选中的某一个文件

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
					//在界面展示提示
					break;
				case "下载":
					MessageBox.Show(e.ClickedItem.Text);
					break;
				default:
					break;
			}
		}
		/// <summary>
		/// 右键文件上传
		/// </summary>
		/*public void UpLoadSelectedFile()
		{
			//发送文件之前 将文件名字和长度发送过去
			long fileLength = new FileInfo(fileFullPath).Length;
			string totalMsg = string.Format("{0}-{1}", fileName, fileLength);
			ClientSendMsg(totalMsg, 2);


			//发送文件
			byte[] buffer = new byte[SendBufferSize];

			using (FileStream fs = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
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

						socketClient.Send(firstBuffer, 0, readLength + 1, SocketFlags.None);

						firstRead = false;
						continue;
					}
					//之后发送的均为直接读取的字节流
					socketClient.Send(buffer, 0, readLength, SocketFlags.None);
				}
				fs.Close();
			}
		}*/
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
		}
	}
}
