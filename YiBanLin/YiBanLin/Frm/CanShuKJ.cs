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
using YiBanLin.Model;


namespace Pcan.Frm
{
    public partial class CanShuKJ : UserControl,KJPeiZhiJK
    {
        private CunType CunType=CunType.XieKaiLin;
        public CanShuKJ()
        {
            InitializeComponent();
        }

        public void SetData(CunType model)
        {
           
            CunType = model;
            if (CunType == CunType.XieKaiLin)
            {
                this.panel1.Visible = false;
                this.label11.Text = "不需要参数";
            }
            else if (CunType == CunType.XieGuanLin)
            {
                this.panel1.Visible = false;
                this.label11.Text = "不需要参数";
            }
            else if (CunType == CunType.XieFanHuiLin1)
            {
               
               
            }
            else if (CunType == CunType.XieFanHuiLin0)
            {
              
            }
          
          
        }
        public string GetCanShu()
        {

            if (CunType == CunType.XieKaiLin)
            {
                return "";
            }
            else if (CunType == CunType.XieGuanLin)
            {
                return "";
            }
            else if (CunType == CunType.XieFanHuiLin1)
            {
                List<string> list = new List<string>();
                list.Add(this.textBox5.Text);
                list.Add(this.textBox4.Text);
                list.Add(this.textBox1.Text);
                return ChangYong.FenGeDaBao(list, ",");

            }
            else if (CunType == CunType.XieFanHuiLin0)
            {
                List<string> list = new List<string>();
                list.Add(this.textBox5.Text);
                list.Add(this.textBox4.Text);
                list.Add(this.textBox1.Text);
                return ChangYong.FenGeDaBao(list,",");
            }
            return "";
        }

        public Control GetPeiZhiKJ(string jicunqibiaoshi)
        {
            return this;
        }

        public void SetCanShu(string canshu)
        {

            if (CunType == CunType.XieKaiLin)
            {
               
            }
            else if (CunType == CunType.XieGuanLin)
            {
                //return "";
            }
            else if (CunType == CunType.XieFanHuiLin0)
            {
                List<string> list = ChangYong.JieGeStr(canshu, ',');
                if (list.Count > 2)
                {
                    this.textBox5.Text = list[0];
                    this.textBox4.Text = list[1];
                    this.textBox1.Text = list[2];
                }


            }
            else if (CunType == CunType.XieFanHuiLin1)
            {
                List<string> list = ChangYong.JieGeStr(canshu,',');
                if (list.Count > 2)
                {
                    this.textBox5.Text=list[0];
                    this.textBox4.Text=list[1];
                    this.textBox1.Text = list[2];
                }
                
            }
        }
    }
}
