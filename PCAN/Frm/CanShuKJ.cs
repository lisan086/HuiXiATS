using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CommLei.JiChuLei;
using SSheBei.ABSSheBei;
using YiBanSaoMaQi.Model;

namespace Pcan.Frm
{
    public partial class CanShuKJ : UserControl,KJPeiZhiJK
    {
        private CunType CunType=CunType.XieGuanCAN;
        public CanShuKJ()
        {
            InitializeComponent();
        }

        public void SetData(CunType model)
        {
           
            CunType = model;
            if (CunType == CunType.XieGuanCAN)
            {
                this.panel1.Visible = false;
                this.label11.Text = "不需要参数";
            }
            else if (CunType == CunType.XieLianJieCAN)
            {
                this.panel1.Visible = false;
                this.label11.Text = "不需要参数";
            }
            else if (CunType == CunType.XieCANWuZhiRec)
            {
                this.label1.Visible = false;
                this.textBox1.Visible = false;

            }
            else if (CunType == CunType.XieCANYouZhiRec)
            {
              
            }
            else if (CunType == CunType.XieCANZhiDu)
            {
                this.label7.Visible = false;
                this.label8.Visible = false;

                this.textBox4.Visible = false;
                this.textBox5.Visible = false;
            }

        }
        public string GetCanShu()
        {

            if (CunType == CunType.XieGuanCAN)
            {
                return "";
            }
            else if (CunType == CunType.XieLianJieCAN)
            {
                return "";
            }
            else if (CunType == CunType.XieCANWuZhiRec)
            {
                List<string> list = new List<string>();
                list.Add(this.textBox5.Text);
                list.Add(this.textBox4.Text);
              
                return ChangYong.FenGeDaBao(list, ",");

            }
            else if (CunType == CunType.XieCANYouZhiRec)
            {
                List<string> list = new List<string>();
                list.Add(this.textBox5.Text);
                list.Add(this.textBox4.Text);
                list.Add(this.textBox1.Text);
                return ChangYong.FenGeDaBao(list,",");
            }
            else if (CunType == CunType.XieCANZhiDu)
            {
                return this.textBox1.Text;
            }
            return "";
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {

            if (CunType == CunType.XieGuanCAN)
            {
               
            }
            else if (CunType == CunType.XieLianJieCAN)
            {
                //return "";
            }
            else if (CunType == CunType.XieCANZhiDu)
            {
                 this.textBox1.Text=canshu;

            }
            else if (CunType == CunType.XieCANYouZhiRec)
            {
                List<string> list = ChangYong.JieGeStr(canshu,',');
                if (list.Count > 2)
                {
                    this.textBox5.Text=list[0];
                    this.textBox4.Text=list[1];
                    this.textBox1.Text = list[2];
                }
                
            }
            else if (CunType == CunType.XieCANWuZhiRec)
            {
                List<string> list = ChangYong.JieGeStr(canshu, ',');
                if (list.Count >= 2)
                {
                    this.textBox5.Text = list[0];
                    this.textBox4.Text = list[1];
                  
                }

            }
        }
    }
}
