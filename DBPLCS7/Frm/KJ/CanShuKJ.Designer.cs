namespace DBPLCS7.Frm.KJ
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
            this.commBoxE1 = new JieMianLei.UC.CommBoxE();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 27);
            this.label1.TabIndex = 126;
            this.label1.Text = "参数";
            // 
            // commBoxE1
            // 
            this.commBoxE1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.commBoxE1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commBoxE1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commBoxE1.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.commBoxE1.FormattingEnabled = true;
            this.commBoxE1.Location = new System.Drawing.Point(84, 22);
            this.commBoxE1.Margin = new System.Windows.Forms.Padding(4);
            this.commBoxE1.Name = "commBoxE1";
            this.commBoxE1.Size = new System.Drawing.Size(217, 35);
            this.commBoxE1.TabIndex = 127;
            // 
            // CanShuKJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.commBoxE1);
            this.Name = "CanShuKJ";
            this.Size = new System.Drawing.Size(316, 162);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private JieMianLei.UC.CommBoxE commBoxE1;
    }
}
