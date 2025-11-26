namespace ATSUI
{
    partial class MianFrm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设备配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设备调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.切换用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日记管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日志配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.流程配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.流程调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读IO显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读参数显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.写调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mES配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sheBeiDengKJ1 = new BaseUI.UC.SheBeiDengKJ();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ucFpanl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).BeginInit();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFpanl1
            // 
            this.ucFpanl1.Controls.Add(this.sheBeiDengKJ1);
            this.ucFpanl1.Controls.Add(this.panel1);
            this.ucFpanl1.Size = new System.Drawing.Size(1187, 43);
            this.ucFpanl1.Controls.SetChildIndex(this.picFico, 0);
            this.ucFpanl1.Controls.SetChildIndex(this.labFbiaoTi, 0);
            this.ucFpanl1.Controls.SetChildIndex(this.panel1, 0);
            this.ucFpanl1.Controls.SetChildIndex(this.sheBeiDengKJ1, 0);
            // 
            // picFico
            // 
            this.picFico.Image = global::ATSUI.Properties.Resources.ats__终稿_256x256;
            this.picFico.Size = new System.Drawing.Size(48, 41);
            // 
            // labFbiaoTi
            // 
            this.labFbiaoTi.AutoSize = false;
            this.labFbiaoTi.Dock = System.Windows.Forms.DockStyle.Left;
            this.labFbiaoTi.Location = new System.Drawing.Point(48, 0);
            this.labFbiaoTi.Size = new System.Drawing.Size(330, 41);
            this.labFbiaoTi.Text = "主界面";
            // 
            // ucPanL1
            // 
            this.ucPanL1.Size = new System.Drawing.Size(1187, 799);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(378, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panel1.Size = new System.Drawing.Size(466, 41);
            this.panel1.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设备ToolStripMenuItem,
            this.登录ToolStripMenuItem,
            this.日记管理ToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(3, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(460, 41);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设备ToolStripMenuItem
            // 
            this.设备ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设备配置ToolStripMenuItem,
            this.设备调试ToolStripMenuItem});
            this.设备ToolStripMenuItem.Name = "设备ToolStripMenuItem";
            this.设备ToolStripMenuItem.Size = new System.Drawing.Size(66, 37);
            this.设备ToolStripMenuItem.Text = "设备";
            // 
            // 设备配置ToolStripMenuItem
            // 
            this.设备配置ToolStripMenuItem.Name = "设备配置ToolStripMenuItem";
            this.设备配置ToolStripMenuItem.Size = new System.Drawing.Size(178, 32);
            this.设备配置ToolStripMenuItem.Text = "设备配置";
            this.设备配置ToolStripMenuItem.Click += new System.EventHandler(this.设备配置ToolStripMenuItem_Click);
            // 
            // 设备调试ToolStripMenuItem
            // 
            this.设备调试ToolStripMenuItem.Name = "设备调试ToolStripMenuItem";
            this.设备调试ToolStripMenuItem.Size = new System.Drawing.Size(178, 32);
            this.设备调试ToolStripMenuItem.Text = "设备调试";
            this.设备调试ToolStripMenuItem.Click += new System.EventHandler(this.设备调试ToolStripMenuItem_Click);
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.登录配置ToolStripMenuItem,
            this.切换用户ToolStripMenuItem});
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            this.登录ToolStripMenuItem.Size = new System.Drawing.Size(66, 37);
            this.登录ToolStripMenuItem.Text = "登录";
        
            // 
            // 登录配置ToolStripMenuItem
            // 
            this.登录配置ToolStripMenuItem.Name = "登录配置ToolStripMenuItem";
            this.登录配置ToolStripMenuItem.Size = new System.Drawing.Size(224, 32);
            this.登录配置ToolStripMenuItem.Text = "登录配置";
            this.登录配置ToolStripMenuItem.Click += new System.EventHandler(this.登录配置ToolStripMenuItem_Click);
            // 
            // 切换用户ToolStripMenuItem
            // 
            this.切换用户ToolStripMenuItem.Name = "切换用户ToolStripMenuItem";
            this.切换用户ToolStripMenuItem.Size = new System.Drawing.Size(224, 32);
            this.切换用户ToolStripMenuItem.Text = "切换用户";
            this.切换用户ToolStripMenuItem.Click += new System.EventHandler(this.切换用户ToolStripMenuItem_Click);
            // 
            // 日记管理ToolStripMenuItem
            // 
            this.日记管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.日志配置ToolStripMenuItem});
            this.日记管理ToolStripMenuItem.Name = "日记管理ToolStripMenuItem";
            this.日记管理ToolStripMenuItem.Size = new System.Drawing.Size(106, 37);
            this.日记管理ToolStripMenuItem.Text = "日志管理";
            // 
            // 日志配置ToolStripMenuItem
            // 
            this.日志配置ToolStripMenuItem.Name = "日志配置ToolStripMenuItem";
            this.日志配置ToolStripMenuItem.Size = new System.Drawing.Size(178, 32);
            this.日志配置ToolStripMenuItem.Text = "日志配置";
            this.日志配置ToolStripMenuItem.Click += new System.EventHandler(this.日志配置ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.流程配置ToolStripMenuItem,
            this.流程调试ToolStripMenuItem,
            this.mES配置ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(66, 37);
            this.toolStripMenuItem1.Text = "流程";
            // 
            // 流程配置ToolStripMenuItem
            // 
            this.流程配置ToolStripMenuItem.Name = "流程配置ToolStripMenuItem";
            this.流程配置ToolStripMenuItem.Size = new System.Drawing.Size(181, 32);
            this.流程配置ToolStripMenuItem.Text = "流程配置";
            this.流程配置ToolStripMenuItem.Click += new System.EventHandler(this.流程配置ToolStripMenuItem_Click);
            // 
            // 流程调试ToolStripMenuItem
            // 
            this.流程调试ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.读IO显示ToolStripMenuItem,
            this.读参数显示ToolStripMenuItem,
            this.写调试ToolStripMenuItem,
            this.读缓存ToolStripMenuItem});
            this.流程调试ToolStripMenuItem.Name = "流程调试ToolStripMenuItem";
            this.流程调试ToolStripMenuItem.Size = new System.Drawing.Size(181, 32);
            this.流程调试ToolStripMenuItem.Text = "流程调试";
            // 
            // 读IO显示ToolStripMenuItem
            // 
            this.读IO显示ToolStripMenuItem.Name = "读IO显示ToolStripMenuItem";
            this.读IO显示ToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.读IO显示ToolStripMenuItem.Text = "读IO显示";
            this.读IO显示ToolStripMenuItem.Click += new System.EventHandler(this.读IO显示ToolStripMenuItem_Click);
            // 
            // 读参数显示ToolStripMenuItem
            // 
            this.读参数显示ToolStripMenuItem.Name = "读参数显示ToolStripMenuItem";
            this.读参数显示ToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.读参数显示ToolStripMenuItem.Text = "读参数显示";
            this.读参数显示ToolStripMenuItem.Click += new System.EventHandler(this.读参数显示ToolStripMenuItem_Click);
            // 
            // 写调试ToolStripMenuItem
            // 
            this.写调试ToolStripMenuItem.Name = "写调试ToolStripMenuItem";
            this.写调试ToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.写调试ToolStripMenuItem.Text = "写调试";
            this.写调试ToolStripMenuItem.Click += new System.EventHandler(this.写调试ToolStripMenuItem_Click);
            // 
            // 读缓存ToolStripMenuItem
            // 
            this.读缓存ToolStripMenuItem.Name = "读缓存ToolStripMenuItem";
            this.读缓存ToolStripMenuItem.Size = new System.Drawing.Size(198, 32);
            this.读缓存ToolStripMenuItem.Text = "查看缓存";
            this.读缓存ToolStripMenuItem.Click += new System.EventHandler(this.读缓存ToolStripMenuItem_Click);
            // 
            // mES配置ToolStripMenuItem
            // 
            this.mES配置ToolStripMenuItem.Name = "mES配置ToolStripMenuItem";
            this.mES配置ToolStripMenuItem.Size = new System.Drawing.Size(181, 32);
            this.mES配置ToolStripMenuItem.Text = "MES配置";
            this.mES配置ToolStripMenuItem.Click += new System.EventHandler(this.mES配置ToolStripMenuItem_Click);
            // 
            // sheBeiDengKJ1
            // 
            this.sheBeiDengKJ1.ColorYanSe = System.Drawing.Color.Red;
            this.sheBeiDengKJ1.Dock = System.Windows.Forms.DockStyle.Left;
            this.sheBeiDengKJ1.Location = new System.Drawing.Point(844, 0);
            this.sheBeiDengKJ1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sheBeiDengKJ1.Name = "sheBeiDengKJ1";
            this.sheBeiDengKJ1.Size = new System.Drawing.Size(184, 41);
            this.sheBeiDengKJ1.StrName = "设备状态";
            this.sheBeiDengKJ1.TabIndex = 8;
            this.sheBeiDengKJ1.ZiTiDaXiao = 12;
            this.sheBeiDengKJ1.ZiTiYanSe = System.Drawing.Color.Black;
            this.sheBeiDengKJ1.Click += new System.EventHandler(this.sheBeiDengKJ1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 150;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MianFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1187, 842);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MianFrm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MianFrm_Load);
            this.ucFpanl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设备配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设备调试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 切换用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 日记管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 日志配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 流程配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 流程调试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读IO显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读参数显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 写调试ToolStripMenuItem;
        private BaseUI.UC.SheBeiDengKJ sheBeiDengKJ1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 读缓存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mES配置ToolStripMenuItem;
    }
}

