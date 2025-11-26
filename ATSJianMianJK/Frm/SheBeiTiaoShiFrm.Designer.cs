namespace ATSJianMianJK.Frm
{
    partial class SheBeiTiaoShiFrm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.chouTiKJ1 = new BaseUI.UC.ChouTiKJ();
            this.ucFpanl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).BeginInit();
            this.ucPanL1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFpanl1
            // 
            this.ucFpanl1.Size = new System.Drawing.Size(1369, 43);
            // 
            // labFbiaoTi
            // 
            this.labFbiaoTi.Size = new System.Drawing.Size(92, 27);
            this.labFbiaoTi.Text = "设备调试";
            // 
            // ucPanL1
            // 
            this.ucPanL1.Controls.Add(this.panel1);
            this.ucPanL1.Controls.Add(this.chouTiKJ1);
            this.ucPanL1.Size = new System.Drawing.Size(1369, 808);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(246, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1121, 806);
            this.panel1.TabIndex = 8;
            // 
            // chouTiKJ1
            // 
            this.chouTiKJ1.ButtonHeight = 30;
            this.chouTiKJ1.Dock = System.Windows.Forms.DockStyle.Left;
            this.chouTiKJ1.Location = new System.Drawing.Point(0, 0);
            this.chouTiKJ1.Margin = new System.Windows.Forms.Padding(4);
            this.chouTiKJ1.Name = "chouTiKJ1";
            this.chouTiKJ1.SelectedBand = 0;
            this.chouTiKJ1.Size = new System.Drawing.Size(246, 806);
            this.chouTiKJ1.TabIndex = 7;
            // 
            // SheBeiTiaoShiFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1369, 851);
            this.Name = "SheBeiTiaoShiFrm";
            this.Text = "SheBeiTiaoShiFrm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SheBeiTiaoShiFrm_Load);
            this.ucFpanl1.ResumeLayout(false);
            this.ucFpanl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).EndInit();
            this.ucPanL1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private BaseUI.UC.ChouTiKJ chouTiKJ1;
    }
}