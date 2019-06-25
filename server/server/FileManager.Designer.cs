namespace server
{
	partial class FileManager
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.dirListTree = new System.Windows.Forms.TreeView();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.FileListView = new System.Windows.Forms.ListView();
			this.NodeRightMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.上传ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.下载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.NodeRightMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.dirListTree);
			this.groupBox1.Location = new System.Drawing.Point(9, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(293, 632);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "硬盘";
			// 
			// dirListTree
			// 
			this.dirListTree.Location = new System.Drawing.Point(7, 37);
			this.dirListTree.Name = "dirListTree";
			this.dirListTree.Size = new System.Drawing.Size(274, 589);
			this.dirListTree.TabIndex = 0;
			this.dirListTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.dirlistTreeNodeDoubleClick);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.FileListView);
			this.groupBox2.Location = new System.Drawing.Point(308, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(473, 632);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "文件";
			// 
			// FileListView
			// 
			this.FileListView.Location = new System.Drawing.Point(10, 39);
			this.FileListView.Name = "FileListView";
			this.FileListView.Size = new System.Drawing.Size(462, 587);
			this.FileListView.TabIndex = 0;
			this.FileListView.UseCompatibleStateImageBehavior = false;
			this.FileListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ItemRightClicked);
			// 
			// NodeRightMenu
			// 
			this.NodeRightMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.NodeRightMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上传ToolStripMenuItem,
            this.下载ToolStripMenuItem});
			this.NodeRightMenu.Name = "NodeRightMenu";
			this.NodeRightMenu.Size = new System.Drawing.Size(109, 52);
			this.NodeRightMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.NodeRightMenu_ItemClicked);
			// 
			// 上传ToolStripMenuItem
			// 
			this.上传ToolStripMenuItem.Name = "上传ToolStripMenuItem";
			this.上传ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
			this.上传ToolStripMenuItem.Text = "上传";
			// 
			// 下载ToolStripMenuItem
			// 
			this.下载ToolStripMenuItem.Name = "下载ToolStripMenuItem";
			this.下载ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
			this.下载ToolStripMenuItem.Text = "下载";
			// 
			// FileManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 656);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "FileManager";
			this.Text = "FileManager";
			this.Load += new System.EventHandler(this.File_Manager_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.NodeRightMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TreeView dirListTree;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListView FileListView;
		private System.Windows.Forms.ContextMenuStrip NodeRightMenu;
		private System.Windows.Forms.ToolStripMenuItem 上传ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 下载ToolStripMenuItem;
	}
}