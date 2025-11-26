namespace ATSJianCeXianTi.JKKJ.PeiZhiKJ
{
    partial class TDUIKJ
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
            this.tdBiaoGeKJ1 = new ATSJianCeXianTi.JKKJ.XianShi.TDBiaoGeKJ();
            this.gzkj1 = new ATSJianCeXianTi.JKKJ.PeiZhiKJ.GZKJ();
            this.SuspendLayout();
            // 
            // tdBiaoGeKJ1
            // 
            this.tdBiaoGeKJ1.Location = new System.Drawing.Point(47, 3);
            this.tdBiaoGeKJ1.Name = "tdBiaoGeKJ1";
            this.tdBiaoGeKJ1.Size = new System.Drawing.Size(955, 391);
            this.tdBiaoGeKJ1.TabIndex = 34;
            // 
            // gzkj1
            // 
            this.gzkj1.Location = new System.Drawing.Point(93, 424);
            this.gzkj1.Name = "gzkj1";
            this.gzkj1.Size = new System.Drawing.Size(872, 178);
            this.gzkj1.TabIndex = 33;
            // 
            // TDUIKJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tdBiaoGeKJ1);
            this.Controls.Add(this.gzkj1);
            this.Name = "TDUIKJ";
            this.Size = new System.Drawing.Size(1149, 582);
            this.ResumeLayout(false);

        }

        #endregion
        private GZKJ gzkj1;
        private XianShi.TDBiaoGeKJ tdBiaoGeKJ1;
    }
}
