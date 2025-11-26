namespace ZuZhuangUI.PeiZhi.Frm
{
    partial class TDShuJuFrm
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.commBoxE1 = new JieMianLei.UC.CommBoxE();
            this.label1 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ucFpanl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).BeginInit();
            this.ucPanL1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFpanl1
            // 
            this.ucFpanl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ucFpanl1.Size = new System.Drawing.Size(734, 35);
            // 
            // labFbiaoTi
            // 
            this.labFbiaoTi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labFbiaoTi.Size = new System.Drawing.Size(92, 27);
            this.labFbiaoTi.Text = "通道数据";
            // 
            // ucPanL1
            // 
            this.ucPanL1.Controls.Add(this.flowLayoutPanel1);
            this.ucPanL1.Controls.Add(this.panel2);
            this.ucPanL1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ucPanL1.Size = new System.Drawing.Size(734, 506);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 54);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(732, 450);
            this.flowLayoutPanel1.TabIndex = 48;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.commBoxE1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(732, 54);
            this.panel2.TabIndex = 49;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(274, 24);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(81, 17);
            this.checkBox1.TabIndex = 116;
            this.checkBox1.Text = "请求类型";
            this.checkBox1.UseVisualStyleBackColor = true;
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
            this.commBoxE1.Location = new System.Drawing.Point(88, 11);
            this.commBoxE1.Name = "commBoxE1";
            this.commBoxE1.Size = new System.Drawing.Size(156, 35);
            this.commBoxE1.TabIndex = 115;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 27);
            this.label1.TabIndex = 114;
            this.label1.Text = "通道ID";
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.Location = new System.Drawing.Point(392, 14);
            this.button5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(109, 32);
            this.button5.TabIndex = 45;
            this.button5.Text = "查询";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TDShuJuFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 541);
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.Name = "TDShuJuFrm";
            this.Text = "TDShuJuFrm";
            this.ucFpanl1.ResumeLayout(false);
            this.ucFpanl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFico)).EndInit();
            this.ucPanL1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox1;
        private JieMianLei.UC.CommBoxE commBoxE1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}