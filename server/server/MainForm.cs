using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace server
{
	public partial class MainForm : Form
	{
		TcpListener Lis;
		Socket socket;
		String removeIP;//新下线的主机
		String addIP;//新上线的主机
		ClientInfo client = null;//主机信息结构体

		public delegate void myUI();//委托 
		public MainForm()
		{
			InitializeComponent();
		}
		/// <summary>
		/// 主窗体加载时的事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Main_Form_Load(object sender, EventArgs e)
		{
			//窗体加载的同时则开启端口监听 - [多线程]
			
		}

		/// <summary>
		/// 监听上线的端口
		/// </summary>
		public void listenPort()
		{
			IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			Lis = new TcpListener(ipAddress, Global.port);
			while (Global.isListenPort)
			{
				try
				{ 
					Lis.Start();//开始监听
					this.program_status.Text = Global.port + "端口监听成功";
				}
				catch
				{
					this.program_status.Text = Global.port + "端口监听失败";
				}
				try
				{
					this.socket = Lis.AcceptSocket();//客户端请求则创建套接字
													 //解析客户端请求
					while (true)
					{
						using (NetworkStream ns = new NetworkStream(this.socket))
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
				catch { }
			}
		}
		/// <summary>
		/// 解析客户端的命令
		/// </summary>
		public void orderCatcher(String[] orderSet)
		{
			//判断命令的第一个参数
			switch (orderSet[0])
			{
				case "$Online":
					this.onLine(orderSet);
					break;
				case "$rej":
					break;
				case "$Offline":
					this.offLine(orderSet[1]);
					break;
			}
		}
		/// <summary>
		/// 有客户端上线时解析基本信息
		/// </summary>
		public void onLine(String[] orderSet)
		{
			ClientInfo client = new ClientInfo();
			//获取IP地址和端口号
			client.IP = ((IPEndPoint)this.socket.RemoteEndPoint).Address.ToString();
			client.Port = ((IPEndPoint)this.socket.RemoteEndPoint).Port.ToString();
			//解析命令获取其他参数
			client.ComputerName = orderSet[1];
			client.OS = orderSet[2];
			client.CPU = orderSet[3];
			client.Memory = orderSet[4];
			client.Socket = this.socket;
			//添加到全局变量
			this.client = client;

			//向被控端发送上线成功反馈
			using (NetworkStream Ns = new NetworkStream(client.Socket))
			{
				Ns.Write(Encoding.Default.GetBytes("$Return||#Online_OK"), 0, Encoding.Default.GetBytes("$Return||#Online_OK").Length);
				Ns.Flush();
			}
			//判断是否是重复上线,不是则在数组和ListView中添加
			int flag = 0;
			for (int i = 0; i < this.OnlineList.Items.Count; i++)
			{
				if (this.OnlineList.Items[i].Text.Trim() == this.client.IP.Trim())
				{
					flag = 1;
					break;
				}
			}
			if (flag == 0)
			{
				//数组中添加
				Global.OnlineClientArr.Add(this.client);
				this.addIP = this.client.IP;

				//添加界面LISTVIEW中的主机
				this.BeginInvoke(new myUI(this.addComputer));
			}


		}

		/// <summary>
		/// 被控制端下线
		/// </summary>
		/// <param name="orderSet"></param>
		public void offLine(String ip)
		{
			//移除集合中的该主机
			for (int i = 0; i < Global.onlineNumber; i++)
			{
				if (ip == Global.OnlineClientArr[i].IP)
				{
					Global.OnlineClientArr.Remove(Global.OnlineClientArr[i]);
					break;
				}
				this.removeIP = ip;
				//删除界面LISTVIEW中的主机
				this.BeginInvoke(new myUI(this.removeComputer));
			}
		}
		/// <summary>
		/// 删除ListView中的指定主机
		/// </summary>
		public void removeComputer()
		{
			for (int i = 0; i < this.OnlineList.Items.Count; i++)
			{
				if (this.OnlineList.Items[i].Text.Trim() == this.removeIP.Trim())
				{
					//消除该项
					this.OnlineList.Items[i].Remove();
					break;
				}
			}
			Global.onlineNumber--;
			//在ConnectionInfo框显示下线信息
			this.ConnectionInfo.Items.Add(this.removeIP + "主机下线成功");
			this.clientNum.Text = Convert.ToString(Global.onlineNumber);
		}

		/// <summary>
		/// 在ListView中添加上线的主机
		/// </summary>
		public void addComputer()
		{
			//在界面的ListView中添加上线的主机信息
			/**ListViewItem item = new ListViewItem();
			item.SubItems.Add(this.client.IP);
			item.SubItems.Add(this.client.Port);
			item.SubItems.Add(this.client.ComputerName);
			item.SubItems.Add(this.client.OS);
			item.SubItems.Add(this.client.CPU);
			item.SubItems.Add(this.client.Memory);
			OnlineList.Items.Add(item);**/

			this.OnlineList.Items.Add(this.client.IP, this.client.IP, 0);
			this.OnlineList.Items[this.client.IP].SubItems.Add(this.client.Port);
			this.OnlineList.Items[this.client.IP].SubItems.Add(this.client.ComputerName);
			this.OnlineList.Items[this.client.IP].SubItems.Add(this.client.OS);
			this.OnlineList.Items[this.client.IP].SubItems.Add(this.client.CPU);
			this.OnlineList.Items[this.client.IP].SubItems.Add(this.client.Memory);


			//上线主机个数增加
			Global.onlineNumber++;
			//在ConnectionInfo框显示上线信息
			this.ConnectionInfo.Items.Add(this.removeIP + "主机上线成功");
			this.clientNum.Text = Convert.ToString(Global.onlineNumber);
		}

		/// <summary>
		/// 关闭主界面时
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(CancelEventArgs e)
		{
			DialogResult result = MessageBox.Show("是否确认关闭？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			e.Cancel = result != DialogResult.Yes;
			base.OnClosing(e);
		}

		/// <summary>
		/// 点击开始监听的按钮时
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			//Global.port = Convert.ToInt32(this.portText.Text);
			//Global.isListenPort = true;
			Thread thread = new Thread(new ThreadStart(this.listenPort));
			thread.Start();
		}


		/// <summary>
		/// 双击上线主机
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnlineList_DoubleClick(object sender, EventArgs e)
		{
			//如果选中了某一项
			if (this.OnlineList.SelectedItems.Count > 0)
			{
				String IP = this.OnlineList.SelectedItems[0].Text.Trim();
				//根据选中的IP对上线主机集合进行查找
				for (int i = 0; i < Global.OnlineClientArr.Count; i++)
				{
					if (IP == Global.OnlineClientArr[i].IP)
					{
						//传递IP，Socket，主窗体的句柄，打开文件管理
						FileManager FM = new FileManager(IP, Global.OnlineClientArr[i].Socket, this);
						FM.Show();
					}
				}
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			//如果选中了某一项
			if (this.OnlineList.SelectedItems.Count > 0)
			{
				String IP = this.OnlineList.SelectedItems[0].Text.Trim();
				//根据选中的IP对上线主机集合进行查找
				for (int i = 0; i < Global.OnlineClientArr.Count; i++)
				{
					if (IP == Global.OnlineClientArr[i].IP)
					{
						//传递IP，Socket，主窗体的句柄，打开文件管理
						FileManager FM = new FileManager(IP, Global.OnlineClientArr[i].Socket, this);
						FM.Show();
					}
				}
			}
		}

	}
}
