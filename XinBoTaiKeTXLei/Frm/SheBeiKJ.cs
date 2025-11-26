using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoTaiKeTXLei.Modle;
using CommLei.JiChuLei;

namespace BoTaiKeTXLei.Frm
{
    public partial class SheBeiKJ : UserControl
    {
        private YiQiModel SaoMaModel;
        public SheBeiKJ()
        {
            InitializeComponent();
        }

        public void SetCanShu(YiQiModel model)
        {
            SaoMaModel = ChangYong.FuZhiShiTi(model);
            this.txbMoShi.Text = model.IP;
            this.textBox1.Text = model.DuanKou.ToString();

            this.textBox4.Text = model.SheBeiID.ToString();
            this.textBox5.Text = model.DuZiFuShu.ToString();
            this.textBox6.Text = model.QuanXianPath.ToString();
            this.textBox2.Text = model.ChaoTime.ToString();
            this.textBox3.Text = model.XieZiFuShu.ToString();
        }
        public YiQiModel GetSaoMaModel()
        {
            YiQiModel model = ChangYong.FuZhiShiTi(SaoMaModel);
            model.IP = this.txbMoShi.Text;
            model.DuanKou = ChangYong.TryInt(this.textBox1.Text, 0);
            model.SheBeiID = ChangYong.TryInt(this.textBox4.Text, 0);
            model.DuZiFuShu = ChangYong.TryInt(this.textBox5.Text, 0);
            model.XieZiFuShu = ChangYong.TryInt(this.textBox3.Text, 0);
            model.QuanXianPath = this.textBox6.Text;
            model.ChaoTime = ChangYong.TryInt(this.textBox2.Text, 30000);
            return model;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            // 设置对话框属性（可选）
            folderBrowserDialog.Description = "请选择ADB工具所在文件夹";  // 对话框描述
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;  // 根文件夹
            folderBrowserDialog.ShowNewFolderButton = true;  // 是否显示"新建文件夹"按钮

            // 显示对话框
            DialogResult result = folderBrowserDialog.ShowDialog();

            // 处理用户选择
            if (result == DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.SelectedPath;
               this.textBox6.Text = selectedFolder;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog = new OpenFileDialog();
            // 设置对话框属性（可选）
        

            // 显示对话框
            DialogResult result = folderBrowserDialog.ShowDialog();

            // 处理用户选择
            if (result == DialogResult.OK)
            {
                string selectedFolder = folderBrowserDialog.FileName;
                this.textBox2.Text = selectedFolder;
            }
        }
    }
}
