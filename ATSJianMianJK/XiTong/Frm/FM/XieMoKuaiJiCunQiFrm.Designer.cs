namespace ATSJianMianJK.XiTong.Frm.FM
{
    partial class XieMoKuaiJiCunQiFrm
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.jiChuXieKJ1 = new ATSJianMianJK.XiTong.Frm.KJ.XieQKJ.JiChuXieKJ();
            this.ucFpanl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).BeginInit();
            this.ucPanL1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFpanl1
            // 
            this.ucFpanl1.Size = new System.Drawing.Size(916, 43);
            // 
            // labFbiaoTi
            // 
            this.labFbiaoTi.Size = new System.Drawing.Size(72, 27);
            this.labFbiaoTi.Text = "写配置";
            // 
            // ucPanL1
            // 
            this.ucPanL1.Controls.Add(this.flowLayoutPanel1);
            this.ucPanL1.Size = new System.Drawing.Size(916, 654);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.jiChuXieKJ1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(914, 652);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // jiChuXieKJ1
            // 
            this.jiChuXieKJ1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jiChuXieKJ1.Location = new System.Drawing.Point(3, 3);
            this.jiChuXieKJ1.Name = "jiChuXieKJ1";
            this.jiChuXieKJ1.Size = new System.Drawing.Size(407, 243);
            this.jiChuXieKJ1.TabIndex = 1;
            // 
            // XieMoKuaiJiCunQiFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 697);
            this.Name = "XieMoKuaiJiCunQiFrm";
            this.Text = "XieMoKuaiJiCunQiFrm";
            this.ucFpanl1.ResumeLayout(false);
            this.ucFpanl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).EndInit();
            this.ucPanL1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private KJ.XieQKJ.JiChuXieKJ jiChuXieKJ1;
    }
}