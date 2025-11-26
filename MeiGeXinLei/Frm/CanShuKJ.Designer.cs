namespace MeiGeXinLei.Frm
{
    partial class CanShuKJ
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
            this.label11 = new System.Windows.Forms.Label();
            this.commBoxE1 = new JieMianLei.UC.CommBoxE();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.commBoxE2 = new JieMianLei.UC.CommBoxE();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 27);
            this.label11.TabIndex = 100;
            this.label11.Text = "填写参数";
            // 
            // commBoxE1
            // 
            this.commBoxE1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.commBoxE1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commBoxE1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commBoxE1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.commBoxE1.FormattingEnabled = true;
            this.commBoxE1.Location = new System.Drawing.Point(82, 12);
            this.commBoxE1.Margin = new System.Windows.Forms.Padding(4);
            this.commBoxE1.Name = "commBoxE1";
            this.commBoxE1.Size = new System.Drawing.Size(211, 35);
            this.commBoxE1.TabIndex = 114;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(18, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 27);
            this.label2.TabIndex = 113;
            this.label2.Text = "数据1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.commBoxE2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.commBoxE1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(319, 130);
            this.panel1.TabIndex = 115;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(18, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 27);
            this.label1.TabIndex = 115;
            this.label1.Text = "数据2";
            // 
            // commBoxE2
            // 
            this.commBoxE2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.commBoxE2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commBoxE2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commBoxE2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.commBoxE2.FormattingEnabled = true;
            this.commBoxE2.Location = new System.Drawing.Point(82, 72);
            this.commBoxE2.Margin = new System.Windows.Forms.Padding(4);
            this.commBoxE2.Name = "commBoxE2";
            this.commBoxE2.Size = new System.Drawing.Size(211, 35);
            this.commBoxE2.TabIndex = 116;
            // 
            // CanShuKJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label11);
            this.Name = "CanShuKJ";
            this.Size = new System.Drawing.Size(319, 157);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label11;
        private JieMianLei.UC.CommBoxE commBoxE1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private JieMianLei.UC.CommBoxE commBoxE2;
    }
}
