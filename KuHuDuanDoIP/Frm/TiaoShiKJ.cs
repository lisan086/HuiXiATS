using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUI.FuZhuLei;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.UC;
using KuHuDuanDoIP.Model;


namespace KuHuDuanDoIP.Frm
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
        public void SetCanShu(List<string> IPS,PeiZhiLei peiZhiLei)
        {
            this.comboBox3.Items.Clear();
            List<string> meijus = ChangYong.MeiJuLisName(typeof(ZhiLingType));
            for (int i = 0; i < meijus.Count; i++)
            {
                if (meijus[i].Contains("Open")|| meijus[i].Contains("GuanBi") || meijus[i].Contains("GetZi"))
                {
                    continue;
                }
                this.comboBox3.Items.Add(meijus[i]);
            }
            if (this.comboBox3.Items.Count > 0)
            {
                this.comboBox3.SelectedIndex = 0;
            }
            PeiZhiLei =peiZhiLei;
            this.comboBox2.Items.Clear();
            for (int i = 0; i < IPS.Count; i++)
            {
                this.comboBox2.Items.Add(IPS[i]);
            }
            if (this.comboBox2.Items.Count>0)
            { 
                this.comboBox2.SelectedIndex = 0;
            }
            InitShuJu();
            this.comboBox1.TextUpdate += ComboBox1_TextUpdate;//
        }

        public void ShuaXin()
        {
            string ip=this.comboBox2.Text;
            SheBeiModel model = PeiZhiLei.MoXing.GetSheBeiModel(ip);
            if (model!=null)
            {
                Color color=model.TX?Color.Green:Color.Red;
                if (this.label1.BackColor!= color)
                { 
                    this.label1.BackColor=color;
                }
            }
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
            List<JiLuZhiLingModel> list =PeiZhiLei.MoXing.GetJiLuData(payh) ;
            for (int i = 0; i < list.Count; i++)
            {           
                JiHeNameshuju.Add(list[i].ZhiLingName);
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
        }
        //非labamba表达式
        private void Receiv(string msg)
        {
            this.label1.Invoke(new Action(() => {
                this.textBox1.AppendText(msg + "\r\n");
            }));
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = this.comboBox1.Text;

            string payh = PeiZhiLei.JiLuPathName;
            //加载数据
            List<JiLuZhiLingModel> list = PeiZhiLei.MoXing.GetJiLuData(payh);

            for (int i = 0; i < list.Count; i++)
            {
                if (name == list[i].ZhiLingName)
                {
                    JiLuZhiLingModel sendDateModel = list[i];
                    textBox8.Text = sendDateModel.TaDiZhi;
                    textBox9.Text = sendDateModel.FuZaiLeiXing;
                    textBox10.Text = sendDateModel.ZhiLingShuJu;
                    textBox5.Text = sendDateModel.ZhiLingName;
                    this.comboBox3.Text = sendDateModel.ZhiLingType.ToString();
                    this.comboBox2.Text = sendDateModel.IP;
                    break;
                }
            }
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ChongFa)
            {
                MessageBox.Show("还没结束");
                return;
            }
            ZhiLingModel sendDateModel = new ZhiLingModel();
            sendDateModel.FuZaiLeiXing = textBox9.Text.Trim();
            sendDateModel.TaDiZhi = textBox8.Text.Trim();
            sendDateModel.IP = this.comboBox2.Text;
            sendDateModel.ZhiLingName = textBox5.Text.Trim();
            sendDateModel.ZhiLingShuJu = textBox10.Text.Trim();
        
            sendDateModel.ZhiLingType = ChangYong.GetMeiJuZhi<ZhiLingType>(this.comboBox3.Text);
            if (sendDateModel.FuZaiLeiXing == "" && sendDateModel.TaDiZhi == "" && sendDateModel.ZhiLingShuJu == "")
            {
                MessageBox.Show("命令、状态、参数不能为空");
                return;
            }
            Task.Factory.StartNew(() =>
            {
                ChongFa = true;
                PeiZhiLei.ChuFaXieJiLu(sendDateModel);
                ZhiLingModel model = PeiZhiLei.MoXing.GetModel(sendDateModel.ZhiLingType,false);
                if (model != null)
                {
                    Thread.Sleep(500);
                    DateTime dateTime = DateTime.Now;
                    for (; ; )
                    {
                        if (model.IsXieWan == 1)
                        {
                            Receiv($"时间:{(DateTime.Now - dateTime).TotalMilliseconds}  \r\n{model.JiCunQiModel.Value.ToString()}");
                            break;
                        }
                        if ((DateTime.Now - dateTime).TotalMilliseconds >= 30 * 1000)
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

        private void button1_Click(object sender, EventArgs e)
        {
            ZhiLingModel sendDateModel = new ZhiLingModel();
            sendDateModel.FuZaiLeiXing = textBox9.Text.Trim();
            sendDateModel.TaDiZhi = textBox8.Text.Trim();
            sendDateModel.IP = this.comboBox2.Text;
            sendDateModel.ZhiLingName = textBox5.Text.Trim();
            sendDateModel.ZhiLingShuJu = textBox10.Text.Trim();
            sendDateModel.ZhiLingType = ZhiLingType.XieDoipOpenTCP;
            PeiZhiLei.ChuFaXieJiLu(sendDateModel);
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            InitShuJu();
        }
    }
}
