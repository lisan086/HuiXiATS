namespace BoTaiKeTXLei.Frm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.commBoxE1 = new JieMianLei.UC.CommBoxE();
            this.commBoxE2 = new JieMianLei.UC.CommBoxE();
            this.tianCanShuKJ1 = new BaseUI.UC.TianCanShuKJ();
            this.tianCanShuKJ2 = new BaseUI.UC.TianCanShuKJ();
            this.tianCanShuKJ3 = new BaseUI.UC.TianCanShuKJ();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(66, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 27);
            this.label1.TabIndex = 130;
            this.label1.Text = "ip";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(44, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 27);
            this.label2.TabIndex = 123;
            this.label2.Text = "名称";
            // 
            // commBoxE1
            // 
            this.commBoxE1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.commBoxE1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commBoxE1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.commBoxE1.FormattingEnabled = true;
            this.commBoxE1.Items.AddRange(new object[] {
            "CMD",
            "JJ"});
            this.commBoxE1.Location = new System.Drawing.Point(105, 17);
            this.commBoxE1.Margin = new System.Windows.Forms.Padding(4);
            this.commBoxE1.Name = "commBoxE1";
            this.commBoxE1.Size = new System.Drawing.Size(211, 35);
            this.commBoxE1.TabIndex = 124;
            // 
            // commBoxE2
            // 
            this.commBoxE2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.commBoxE2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commBoxE2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commBoxE2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.commBoxE2.FormattingEnabled = true;
            this.commBoxE2.Items.AddRange(new object[] {
            "CMD",
            "JJ"});
            this.commBoxE2.Location = new System.Drawing.Point(106, 67);
            this.commBoxE2.Margin = new System.Windows.Forms.Padding(4);
            this.commBoxE2.Name = "commBoxE2";
            this.commBoxE2.Size = new System.Drawing.Size(211, 35);
            this.commBoxE2.TabIndex = 131;
            // 
            // tianCanShuKJ1
            // 
            this.tianCanShuKJ1.BiaoTi = "CMD  ";
            this.tianCanShuKJ1.Location = new System.Drawing.Point(49, 123);
            this.tianCanShuKJ1.Name = "tianCanShuKJ1";
            this.tianCanShuKJ1.Size = new System.Drawing.Size(267, 37);
            this.tianCanShuKJ1.TabIndex = 132;
            // 
            // tianCanShuKJ2
            // 
            this.tianCanShuKJ2.BiaoTi = "Param";
            this.tianCanShuKJ2.Location = new System.Drawing.Point(49, 166);
            this.tianCanShuKJ2.Name = "tianCanShuKJ2";
            this.tianCanShuKJ2.Size = new System.Drawing.Size(267, 37);
            this.tianCanShuKJ2.TabIndex = 133;
            // 
            // tianCanShuKJ3
            // 
            this.tianCanShuKJ3.BiaoTi = "第几个";
            this.tianCanShuKJ3.Location = new System.Drawing.Point(49, 215);
            this.tianCanShuKJ3.Name = "tianCanShuKJ3";
            this.tianCanShuKJ3.Size = new System.Drawing.Size(267, 37);
            this.tianCanShuKJ3.TabIndex = 134;
            // 
            // CanShuKJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tianCanShuKJ3);
            this.Controls.Add(this.tianCanShuKJ2);
            this.Controls.Add(this.tianCanShuKJ1);
            this.Controls.Add(this.commBoxE2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.commBoxE1);
            this.Name = "CanShuKJ";
            this.Size = new System.Drawing.Size(343, 255);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private JieMianLei.UC.CommBoxE commBoxE1;
        private JieMianLei.UC.CommBoxE commBoxE2;
        private BaseUI.UC.TianCanShuKJ tianCanShuKJ1;
        private BaseUI.UC.TianCanShuKJ tianCanShuKJ2;
        private BaseUI.UC.TianCanShuKJ tianCanShuKJ3;
    }
}
