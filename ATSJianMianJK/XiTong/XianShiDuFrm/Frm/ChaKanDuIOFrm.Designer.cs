namespace ATSJianMianJK.XiTong.XianShiDuFrm.Frm
{
    partial class ChaKanDuIOFrm
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1 = new JieMianLei.FuFrom.KJ.FGenSui();
            this.panel1 = new System.Windows.Forms.Panel();
            this.commBoxE1 = new JieMianLei.UC.CommBoxE();
            this.label1 = new System.Windows.Forms.Label();
            this.ucFpanl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).BeginInit();
            this.ucPanL1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFpanl1
            // 
            this.ucFpanl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ucFpanl1.Size = new System.Drawing.Size(777, 35);
            // 
            // labFbiaoTi
            // 
            this.labFbiaoTi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labFbiaoTi.Size = new System.Drawing.Size(76, 21);
            this.labFbiaoTi.Text = "读IO数据";
            // 
            // ucPanL1
            // 
            this.ucPanL1.Controls.Add(this.flowLayoutPanel1);
            this.ucPanL1.Controls.Add(this.panel1);
            this.ucPanL1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ucPanL1.Size = new System.Drawing.Size(777, 551);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 52);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(775, 497);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.commBoxE1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 52);
            this.panel1.TabIndex = 9;
            // 
            // commBoxE1
            // 
            this.commBoxE1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.commBoxE1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commBoxE1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commBoxE1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.commBoxE1.FormattingEnabled = true;
            this.commBoxE1.Items.AddRange(new object[] {
            "与",
            "或"});
            this.commBoxE1.Location = new System.Drawing.Point(69, 7);
            this.commBoxE1.Name = "commBoxE1";
            this.commBoxE1.Size = new System.Drawing.Size(156, 29);
            this.commBoxE1.TabIndex = 113;
            this.commBoxE1.SelectedIndexChanged += new System.EventHandler(this.commBoxE1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 21);
            this.label1.TabIndex = 112;
            this.label1.Text = "通道ID";
            // 
            // ChaKanDuIOFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 586);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Name = "ChaKanDuIOFrm";
            this.Text = "ChaKanDuIOFrm";
            this.ucFpanl1.ResumeLayout(false);
            this.ucFpanl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).EndInit();
            this.ucPanL1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private JieMianLei.FuFrom.KJ.FGenSui flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private JieMianLei.UC.CommBoxE commBoxE1;
    }
}