using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BoTaiKeTXLei.Modle;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using SSheBei.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BoTaiKeTXLei.Frm
{
    public partial class TiaoShiKJ : UserControl
    {

        private List<string> JiHeNameshuju = new List<string>();
        private PeiZhiLei PeiZhiLei;
        private bool ChongFa = false;
        private List<string> listNew = new List<string>();
        public TiaoShiKJ()
        {
            InitializeComponent();
           
          
        }

        public void SetCanShu(PeiZhiLei peiZhiLei)
        {
          
            PeiZhiLei = peiZhiLei;
           
            InitShuJu();
            this.comboBox1.TextUpdate += ComboBox1_TextUpdate;//
        }

      


        /// <summary>
        /// 初始化诊断数据
        /// </summary>
        private void InitShuJu()
        {

            this.comboBox1.Items.Clear();
            JiHeNameshuju.Clear();

            string payh = PeiZhiLei.JiLuPathName;
            //加载数据
            List<SendModel> list = PeiZhiLei.DataMoXing.GetJiLuData(payh);
            for (int i = 0; i < list.Count; i++)
            {
                JiHeNameshuju.Add(list[i].Name);
            }
            this.comboBox1.Items.AddRange(JiHeNameshuju.ToArray());

        }
        private void ComboBox1_TextUpdate(object sender, EventArgs e)
        {
            //清空combobox
            this.comboBox1.Items.Clear();
            //清空listNew
            listNew.Clear();
            //遍历全部备查数据
            foreach (var item in JiHeNameshuju)
            {
                if (item.Contains(this.comboBox1.Text))
                {
                    //符合，插入ListNew
                    listNew.Add(item);
                }
            }

            //combobox添加已经查到的关键词
            this.comboBox1.Items.AddRange(listNew.ToArray());
            //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
            this.comboBox1.SelectionStart = this.comboBox1.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动弹出下拉框
            this.comboBox1.DroppedDown = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ip = this.textBox3.Text;          
            SendModel yiQiModel = PeiZhiLei.DataMoXing.GetModel(CunType.XieOpenBoTaiKe);
            if (yiQiModel!=null)
            {
                yiQiModel.IP= ip;
                PeiZhiLei.XieJiDianQi(yiQiModel, 1);
            }
        }

        public void ShuaXin()
        {
            string ip = this.textBox3.Text;
            YiQiModel yiQiModel = PeiZhiLei.DataMoXing.GetSheBeiModel(ip);
            if (yiQiModel != null)
            {
                if (yiQiModel.TX)
                {
                    if (this.label1.BackColor != Color.Green)
                    {
                        this.label1.BackColor = Color.Green;
                    }
                }
                else
                {
                    if (this.label1.BackColor != Color.Red)
                    {
                        this.label1.BackColor = Color.Red;
                    }
                }
            }
        }
        //非labamba表达式
        private void Receiv(string msg)
        {
            this.label1.Invoke(new Action(() => {
                this.textBox1.AppendText(msg + "\r\n");
            }));
         
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string ip = this.textBox3.Text;

            YiQiModel yiQiModel = PeiZhiLei.DataMoXing.GetSheBeiModel(ip);
            if (yiQiModel != null)
            {
                //Receiv( PeiZhiLei.DataMoXing.RunAdbCommands(yiQiModel.QuanXianPath));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ChongFa)
            {
                MessageBox.Show("还没结束");
                return;
            }
            SendModel sendDateModel = new SendModel();
            sendDateModel.CMD = textBox8.Text.Trim();
            sendDateModel.Status = textBox9.Text.Trim();
            sendDateModel.Param = textBox10.Text;
            sendDateModel.IsABO = CunType.XieBoTaiKeFanHui8;
            sendDateModel.Name = textBox5.Text.Trim();         
            sendDateModel.IP=this.textBox3.Text;
          
            if (sendDateModel.CMD == "" && sendDateModel.Status == "" && sendDateModel.Param == "")
            {
                MessageBox.Show("命令、状态、参数不能为空");
                return;
            }
            Task.Factory.StartNew(() =>
            {
                ChongFa = true;
             
                PeiZhiLei.XieJiDianQi(sendDateModel);
                SendModel model = PeiZhiLei.DataMoXing.GetModel(sendDateModel.IsABO);
                if (model != null)
                {
                    Thread.Sleep(1000);
                    DateTime dateTime = DateTime.Now;
                    for (; ; )
                    {
                        if (model.IsZhengZaiCe == 1)
                        {
                            Receiv($"时间:{(DateTime.Now - dateTime).TotalMilliseconds}  \r\n{model.JiCunQiModel.Value.ToString()}\r\n{PeiZhiLei.DataMoXing.GetData()}");
                            break;
                        }
                        if ((DateTime.Now - dateTime).TotalMilliseconds >=30*1000)
                        {
                            Receiv($"超时时间:{(DateTime.Now - dateTime).TotalMilliseconds}  \r\n");
                            break;
                        }
                        Thread.Sleep(1);
                    }
                }
                ChongFa = false;
             

            });

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (ChongFa)
            {
                return;
            }
            if (checkBox2.Checked)
            {
                string ascii = textBox10.Text;
                Byte[] asczhi = System.Text.Encoding.ASCII.GetBytes(ascii);
                textBox10.Text = ChangYong.ByteOrString(asczhi, " ");
            }
            else
            {
                string ascii = textBox10.Text;
                Byte[] asczhi = ChangYong.HexStringToByte(ascii);
                if (asczhi != null)
                {
                    textBox10.Text = System.Text.Encoding.ASCII.GetString(asczhi);
                }
                else
                {
                    textBox10.Text = "";
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = this.comboBox1.Text;

            string payh = PeiZhiLei.JiLuPathName;
            //加载数据
            List<SendModel> list = PeiZhiLei.DataMoXing.GetJiLuData(payh);

            for (int i = 0; i < list.Count; i++)
            {
                if (name == list[i].Name)
                {
                    SendModel sendDateModel = list[i];
                    textBox8.Text = sendDateModel.CMD;
                    textBox9.Text = sendDateModel.Status;
                    textBox10.Text = sendDateModel.Param;
                    textBox5.Text = sendDateModel.Name;
                    textBox3.Text = sendDateModel.IP;
                    break;
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            InitShuJu();
        }
    }
}
