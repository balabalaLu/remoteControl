namespace server
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.button1 = new System.Windows.Forms.Button();
			this.OnlineList = new System.Windows.Forms.ListView();
			this.IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.computerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.OS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.CPU = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Mem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label1 = new System.Windows.Forms.Label();
			this.ConnectionInfo = new System.Windows.Forms.ListView();
			this.IP_info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.details = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label2 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
			this.program_status = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
			this.clientNum = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.portText = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(838, 28);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// 文件ToolStripMenuItem
			// 
			this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出ToolStripMenuItem});
			this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
			this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
			this.文件ToolStripMenuItem.Text = "文件";
			// 
			// 退出ToolStripMenuItem
			// 
			this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
			this.退出ToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
			this.退出ToolStripMenuItem.Text = "退出";
			// 
			// 设置ToolStripMenuItem
			// 
			this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
			this.设置ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
			this.设置ToolStripMenuItem.Text = "设置";
			// 
			// 帮助ToolStripMenuItem
			// 
			this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
			this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
			this.帮助ToolStripMenuItem.Text = "帮助";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(6, 28);
			this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(81, 49);
			this.button1.TabIndex = 1;
			this.button1.Text = "文件管理";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// OnlineList
			// 
			this.OnlineList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IP,
            this.port,
            this.computerName,
            this.OS,
            this.CPU,
            this.Mem});
			this.OnlineList.FullRowSelect = true;
			this.OnlineList.GridLines = true;
			this.OnlineList.Location = new System.Drawing.Point(11, 113);
			this.OnlineList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.OnlineList.Name = "OnlineList";
			this.OnlineList.Size = new System.Drawing.Size(694, 220);
			this.OnlineList.TabIndex = 0;
			this.OnlineList.UseCompatibleStateImageBehavior = false;
			this.OnlineList.View = System.Windows.Forms.View.Details;
			this.OnlineList.DoubleClick += new System.EventHandler(this.OnlineList_DoubleClick);
			// 
			// IP
			// 
			this.IP.Text = "IP地址";
			this.IP.Width = 212;
			// 
			// port
			// 
			this.port.Text = "端口号";
			this.port.Width = 77;
			// 
			// computerName
			// 
			this.computerName.Text = "计算机名称";
			this.computerName.Width = 144;
			// 
			// OS
			// 
			this.OS.Text = "操作系统";
			this.OS.Width = 124;
			// 
			// CPU
			// 
			this.CPU.Text = "CPU";
			// 
			// Mem
			// 
			this.Mem.Text = "内存容量";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Window;
			this.label1.Location = new System.Drawing.Point(16, 96);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 15);
			this.label1.TabIndex = 3;
			this.label1.Text = "上线主机";
			// 
			// ConnectionInfo
			// 
			this.ConnectionInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IP_info,
            this.details});
			this.ConnectionInfo.Location = new System.Drawing.Point(12, 367);
			this.ConnectionInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ConnectionInfo.Name = "ConnectionInfo";
			this.ConnectionInfo.Size = new System.Drawing.Size(693, 106);
			this.ConnectionInfo.TabIndex = 4;
			this.ConnectionInfo.UseCompatibleStateImageBehavior = false;
			this.ConnectionInfo.View = System.Windows.Forms.View.Details;
			// 
			// IP_info
			// 
			this.IP_info.Text = "IP地址";
			this.IP_info.Width = 188;
			// 
			// details
			// 
			this.details.Text = "详细信息";
			this.details.Width = 707;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.SystemColors.Window;
			this.label2.Location = new System.Drawing.Point(16, 349);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(97, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "客户连接信息";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel4,
            this.program_status,
            this.toolStripStatusLabel3,
            this.clientNum});
			this.statusStrip1.Location = new System.Drawing.Point(0, 484);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
			this.statusStrip1.Size = new System.Drawing.Size(838, 25);
			this.statusStrip1.TabIndex = 0;
			// 
			// toolStripStatusLabel4
			// 
			this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
			this.toolStripStatusLabel4.Size = new System.Drawing.Size(84, 20);
			this.toolStripStatusLabel4.Text = "当前状态：";
			// 
			// program_status
			// 
			this.program_status.Name = "program_status";
			this.program_status.Size = new System.Drawing.Size(78, 20);
			this.program_status.Text = "未就绪......";
			// 
			// toolStripStatusLabel3
			// 
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			this.toolStripStatusLabel3.Size = new System.Drawing.Size(99, 20);
			this.toolStripStatusLabel3.Text = "累计上线主机";
			// 
			// clientNum
			// 
			this.clientNum.Name = "clientNum";
			this.clientNum.Size = new System.Drawing.Size(18, 20);
			this.clientNum.Text = "0";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(23, 23);
			// 
			// portText
			// 
			this.portText.Location = new System.Drawing.Point(612, 47);
			this.portText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.portText.Name = "portText";
			this.portText.Size = new System.Drawing.Size(72, 25);
			this.portText.TabIndex = 6;
			this.portText.Text = "9999";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(700, 44);
			this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(83, 28);
			this.button2.TabIndex = 7;
			this.button2.Text = "开始监听";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(838, 509);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.portText);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ConnectionInfo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.OnlineList);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Main_Form_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView OnlineList;
        private System.Windows.Forms.ColumnHeader IP;
        private System.Windows.Forms.ColumnHeader port;
        private System.Windows.Forms.ColumnHeader computerName;
        private System.Windows.Forms.ColumnHeader OS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView ConnectionInfo;
        private System.Windows.Forms.ColumnHeader IP_info;
        private System.Windows.Forms.ColumnHeader details;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel program_status;
        private System.Windows.Forms.TextBox portText;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel clientNum;
		private System.Windows.Forms.ColumnHeader CPU;
		private System.Windows.Forms.ColumnHeader Mem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
	}
}

