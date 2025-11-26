namespace ATSUI
{
    partial class SheBeiStateFrm
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
            this.chouTiKJ1 = new BaseUI.UC.ChouTiKJ();
            this.fGenSui1 = new JieMianLei.FuFrom.KJ.FGenSui();
            this.ucFpanl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).BeginInit();
            this.ucPanL1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFpanl1
            // 
            this.ucFpanl1.Size = new System.Drawing.Size(888, 43);
            // 
            // labFbiaoTi
            // 
            this.labFbiaoTi.Size = new System.Drawing.Size(132, 27);
            this.labFbiaoTi.Text = "设备运行状态";
            // 
            // ucPanL1
            // 
            this.ucPanL1.Controls.Add(this.fGenSui1);
            this.ucPanL1.Controls.Add(this.chouTiKJ1);
            this.ucPanL1.Size = new System.Drawing.Size(888, 622);
            // 
            // chouTiKJ1
            // 
            this.chouTiKJ1.ButtonHeight = 30;
            this.chouTiKJ1.Dock = System.Windows.Forms.DockStyle.Left;
            this.chouTiKJ1.Location = new System.Drawing.Point(0, 0);
            this.chouTiKJ1.Margin = new System.Windows.Forms.Padding(4);
            this.chouTiKJ1.Name = "chouTiKJ1";
            this.chouTiKJ1.SelectedBand = 0;
            this.chouTiKJ1.Size = new System.Drawing.Size(246, 620);
            this.chouTiKJ1.TabIndex = 8;
            // 
            // fGenSui1
            // 
            this.fGenSui1.AutoScroll = true;
            this.fGenSui1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fGenSui1.Location = new System.Drawing.Point(246, 0);
            this.fGenSui1.Name = "fGenSui1";
            this.fGenSui1.Size = new System.Drawing.Size(640, 620);
            this.fGenSui1.TabIndex = 9;
            // 
            // SheBeiStateFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 665);
            this.Name = "SheBeiStateFrm";
            this.Text = "SheBeiStateFrm";
            this.ucFpanl1.ResumeLayout(false);
            this.ucFpanl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).EndInit();
            this.ucPanL1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseUI.UC.ChouTiKJ chouTiKJ1;
        private JieMianLei.FuFrom.KJ.FGenSui fGenSui1;
    }
}