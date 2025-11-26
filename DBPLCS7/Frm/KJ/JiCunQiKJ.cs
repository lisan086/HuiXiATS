using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUI.FuFrom.XinWeiHuFrm;
using CommLei.JiChuLei;
using DBPLCS7.Model;
using SSheBei.Model;

namespace DBPLCS7.Frm.KJ
{
    public partial class JiCunQiKJ : UserControl, IFUCKJ<PLCJiCunQiModel>
    {
        private PLCJiCunQiModel PLCJiCunQiModel=new PLCJiCunQiModel();
        public JiCunQiKJ()
        {
            InitializeComponent();
            this.comboBox1.Items.Clear();
            List<string> lis = ChangYong.MeiJuLisName(typeof(PLCDataType));
            for (int i = 0; i < lis.Count; i++)
            {
                this.comboBox1.Items.Add(lis[i]);
            }
            this.comboBox2.Items.Clear();
            List<string> lis2 = ChangYong.MeiJuLisName(typeof(XinHaoType));
            for (int i = 0; i < lis2.Count; i++)
            {
                this.comboBox2.Items.Add(lis2[i]);
            }

            this.comboBox3.Items.Clear();
            List<string> lis3 = ChangYong.MeiJuLisName(typeof(GongNengType));
            for (int i = 0; i < lis3.Count; i++)
            {
                this.comboBox3.Items.Add(lis3[i]);
            }
        }

        public void Clear()
        {
            this.textBox2.Clear();
            this.textBox3.Clear();
            this.textBox4.Clear();
            this.textBox5.Clear();
            this.textBox7.Clear();
        }

        public PLCJiCunQiModel GetModel()
        {
            PLCJiCunQiModel model = ChangYong.FuZhiShiTi(PLCJiCunQiModel);
            model.AdRm = ChangYong.TryInt(this.textBox4.Text,-1);
            model.Count = ChangYong.TryInt(this.textBox5.Text, -1);
            model.DBKuan = ChangYong.TryShort(this.textBox2.Text,0);
            model.PianYiLiang = ChangYong.TryShort(this.textBox3.Text, 0);
            model.PLCDataType = ChangYong.GetMeiJuZhi<PLCDataType>(this.comboBox1.Text);
            model.Name = this.textBox7.Text;
            model.XinHaoType= ChangYong.GetMeiJuZhi<XinHaoType>(this.comboBox2.Text);
            model.GongNengType= ChangYong.GetMeiJuZhi<GongNengType>(this.comboBox3.Text);
            model.IsIO = ChangYong.TryInt(this.textBox1.Text, -1);
            model.IOLuZhi= this.textBox6.Text;
            model.IORedZhi = this.textBox8.Text;
            return model;
        }

        public int GetSunXu()
        {
            return 1;
        }

        public void SetCanShu(PLCJiCunQiModel canshu)
        {
            PLCJiCunQiModel = ChangYong.FuZhiShiTi(canshu);           
            this.comboBox1.Text= canshu.PLCDataType.ToString();
            this.textBox2.Text = canshu.DBKuan.ToString();
            this.textBox3.Text = canshu.PianYiLiang.ToString();
            this.textBox4.Text = canshu.AdRm.ToString();
            this.textBox5.Text = canshu.Count.ToString();
            this.textBox7.Text = canshu.Name.ToString();
            this.comboBox2.Text= canshu.XinHaoType.ToString();
            this.comboBox3.Text = canshu.GongNengType.ToString();
            this.textBox1.Text = canshu.IsIO.ToString();
            this.textBox6.Text = canshu.IOLuZhi;
            this.textBox8.Text = canshu.IORedZhi;
        }

        public void SetSunXu(int sunxu)
        {
           
        }
    }
}
