namespace ATSUI.KJ
{
    partial class TXKJ
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.sheBeiDengKJ1 = new BaseUI.UC.SheBeiDengKJ();
            this.SuspendLayout();
            // 
            // sheBeiDengKJ1
            // 
            this.sheBeiDengKJ1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sheBeiDengKJ1.ColorYanSe = System.Drawing.Color.Red;
            this.sheBeiDengKJ1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheBeiDengKJ1.Location = new System.Drawing.Point(0, 0);
            this.sheBeiDengKJ1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sheBeiDengKJ1.Name = "sheBeiDengKJ1";
            this.sheBeiDengKJ1.Size = new System.Drawing.Size(438, 104);
            this.sheBeiDengKJ1.StrName = "设备状态";
            this.sheBeiDengKJ1.TabIndex = 9;
            this.sheBeiDengKJ1.ZiTiDaXiao = 12;
            this.sheBeiDengKJ1.ZiTiYanSe = System.Drawing.Color.Black;
            // 
            // TXKJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.sheBeiDengKJ1);
            this.Name = "TXKJ";
            this.Size = new System.Drawing.Size(438, 104);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseUI.UC.SheBeiDengKJ sheBeiDengKJ1;
    }
}
