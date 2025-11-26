using AdvancedDataGridView;
using ATSJianCeXianTi.Lei;
using ATSJianCeXianTi.Model;
using ATSJianMianJK.ZiDianYiKJ;
using CommLei.DataChuLi;
using CommLei.JiChuLei;
using JieMianLei.FuFrom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ATSJianCeXianTi.PeiFangFrm
{
    public partial class PFPeiZhiFrm : BaseFuFrom
    {
        public bool IsShuaXin = false;
        private PeiFangLei PeiFangLei;
        private List<ZhongJianModel> FuZhiModel = new List<ZhongJianModel>();
        private List<PeiFangShouWeiModel> LisShouWeis = new List<PeiFangShouWeiModel>();
        private List<string> JiaJuHao = new List<string>();
        private List<QieHuanPeiFangModel> Mas = new List<QieHuanPeiFangModel>();
        public PFPeiZhiFrm()
        {
            InitializeComponent();
            PeiFangLei = new PeiFangLei();
            JiaZaiLiaoHao();
        }

        /// <summary>
        /// 加载料号
        /// </summary>
        /// <param name="iskaishi"></param>
        private void JiaZaiLiaoHao(bool iskaishi=true)
        {
            if (iskaishi)
            {
                ZongClear(false);
            }
            
            this.comboBox1.Items.Clear();
            List<string> shuju = PeiFangLei.GetPeiFangNames();
            foreach (var item in shuju)
            {
                this.comboBox1.Items.Add(item);
            }
            
        }

        public void SetCanShu(string peifangming)
        {
            this.comboBox1.Enabled = false;
            this.comboBox1.Text= peifangming;
        }
        private void PFPeiZhiFrm_Load(object sender, EventArgs e)
        {           
              
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsShuaXin)
            {
                return;
            }
            string name = this.comboBox1.Text;
            if (string.IsNullOrEmpty(name))
            {
                this.QiDongTiShiKuang("请先选择配方名称");
                return;
            }
            Mas.Clear();
            JiaJuHao.Clear();
            ZongClear(true);
            ZongTestModel model= PeiFangLei.JiaZaiPeiFang(name);
            if (model!=null)
            {
                LisShouWeis =ChangYong.FuZhiShiTi( model.ShouWeiChuLi);

                Mas = model.PeiFangDuiYingMa;
                JiaJuHao = model.JiaJuNames;
                for (int i = 0; i < model.ZhongJianModels.Count; i++)
                {
                    int index = this.jiHeDataGrid1.Rows.Add();
                    FuZhiData(model.ZhongJianModels[i], this.jiHeDataGrid1.Rows[index]);
                  
                }
               
            }
           
        }

        private void ZongClear(bool isquanbuqingchu)
        {
            if (isquanbuqingchu)
            {
                this.jiHeDataGrid1.Rows.Clear();
                
            }
            else
            {
                this.jiHeDataGrid1.Rows.Clear();
            }
        }

       
        private void FuZhiData(ZhongJianModel zongmodel, DataGridViewRow row)
        {
            FuZhi(zongmodel.TestModel, row, zongmodel);
        }

        private void FuZhi(TestModel model, DataGridViewRow row, ZhongJianModel zongmodel)
        {          
            row.Cells[0].Value = model.ItemName;
            row.Cells[1].Value = model.GongNengType.ToString();
            row.Cells[2].Value = string.Format("{0}:{1}", model.SheBeiID, model.SheBeiName);
            row.Cells[3].Value = model.CMDSend;
            row.Cells[4].Value = model.CMDCanShu;
            row.Cells[5].Value = model.LowStr;
            row.Cells[6].Value = model.UpStr;
            row.Cells[7].Value = model.DanWei;
            row.Cells[8].Value = model.BiJiaoType.ToString();
            row.Cells[9].Value = model.IsTest;
            row.Cells[10].Value = model.IsMes;
            row.Cells[11].Value = model.TaskNo;
            row.Cells[12].Value = model.XuHaoID;
            row.Cells[13].Value = model.FuJianWeiZhi;
            row.Cells[14].Value = model.FuJianCiShu;
            row.Cells[15].Value = model.ShiYingTDID;
            row.Tag = zongmodel;
            row.Height = 32;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int index = this.jiHeDataGrid1.Rows.Add();
            FuZhiData(new ZhongJianModel() { TestModel = new TestModel() { XuHaoID = GetXuHao() } }, this.jiHeDataGrid1.Rows[index]);
        }




        private ZhongJianModel GetZhongJianModel(DataGridViewRow node)
        {

            if (node.Tag is ZhongJianModel)
            {
                ZhongJianModel model = ChangYong.FuZhiShiTi(node.Tag as ZhongJianModel);
                model.TestModel.BiJiaoType = node.Cells[8].Value.ToString();
                model.TestModel.CMDCanShu = ChangYong.TryStr(node.Cells[4].Value, "");
                model.TestModel.CMDSend = ChangYong.TryStr(node.Cells[3].Value, "");
                model.TestModel.DanWei = ChangYong.TryStr(node.Cells[7].Value, "");
                model.TestModel.GongNengType = node.Cells[1].Value.ToString();
                model.TestModel.IsMes = ChangYong.TryStr(node.Cells[10].Value, "").Contains("u");
                model.TestModel.IsTest = ChangYong.TryStr(node.Cells[9].Value, "").Contains("u");
                model.TestModel.ItemName = ChangYong.TryStr(node.Cells[0].Value, "");
                model.TestModel.LowStr = ChangYong.TryStr(node.Cells[5].Value, "");
                string[] shuds = ChangYong.TryStr(node.Cells[2].Value, "").Split(':');
                model.TestModel.SheBeiID = ChangYong.TryInt(shuds[0], -1);
                model.TestModel.TaskNo= ChangYong.TryStr(node.Cells[11].Value, "");
                if (shuds.Length >= 2)
                {
                    model.TestModel.SheBeiName = shuds[1];
                }
                model.TestModel.UpStr = ChangYong.TryStr(node.Cells[6].Value, "");
                model.TestModel.FuJianCiShu = ChangYong.TryInt(node.Cells[14].Value, -1);
                model.TestModel.FuJianCiShuF = model.TestModel.FuJianCiShu;
                model.TestModel.FuJianWeiZhi = ChangYong.TryStr(node.Cells[13].Value, "");
                model.TestModel.IsFuJian = 0;
                model.TestModel.ShiYingTDID = ChangYong.TryStr(node.Cells[15].Value, "");
                if (string.IsNullOrEmpty(model.TestModel.FuJianWeiZhi)==false&& model.TestModel.FuJianCiShu>0)
                {
                    if (model.TestModel.FuJianWeiZhi.Contains(",")==false)
                    {
                        model.TestModel.IsFuJian = 1;
                    }
                }
                return model;
            }

            return null;
        }
       
        private string GetXuHao()
        {
            int count = this.jiHeDataGrid1.Rows.Count;
            return $"{count + 1}";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.jiHeDataGrid1.SelectedRows.Count>0)
            {
                DataGridViewRow node = this.jiHeDataGrid1.SelectedRows[0];
                ShanChu(node);
               
            }
            else
            {
                this.QiDongTiShiKuang("没有选中要删除的项");
            }
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.jiHeDataGrid1.SelectedRows.Count>0)
            {
              
                for (int i = 0; i < this.jiHeDataGrid1.SelectedRows.Count; i++)
                {
                    DataGridViewRow node = this.jiHeDataGrid1.SelectedRows[i];
                    YiDong(false, node);
                }
               
              
            }
            else
            {
                this.QiDongTiShiKuang("没有选中要删除的项");
            }
        }

        /// <summary>
        /// 移动数据
        /// </summary>
        /// <param name="isxiangxia"></param>
        /// <param name="node"></param>
        private void YiDong(bool isxiangxia, DataGridViewRow node)
        {
            int sdhu = this.jiHeDataGrid1.Rows.Count;
            int index = -1;
           
            for (int i = 0; i < sdhu; i++)
            {
                if (this.jiHeDataGrid1.Rows[i].Equals(node))
                {
                    index = i;
                    if (isxiangxia == false)
                    {
                        if (index == 0)
                        {
                            index = -1;
                        }
                        else
                        {                        
                            this.jiHeDataGrid1.Rows.Remove(node);
                        }
                    }
                    else
                    {
                        if (index == sdhu - 1)
                        {
                            index += 1;
                        }
                        else
                        {

                            this.jiHeDataGrid1.Rows.Remove(node);

                        }
                    }

                    break;
                }
            }
            if (isxiangxia == false)
            {
                index -= 1;
                if (index >= 0)
                {

                    this.jiHeDataGrid1.Rows.Insert(index, node);                   
                    node.Selected = true;
                }

            }
            else
            {
                index += 1;
                if (index < sdhu)
                {
                    this.jiHeDataGrid1.Rows.Insert(index, node);                 
                    node.Selected = true;
                }
            }
        }

        private void ShanChu(DataGridViewRow node)
        {
            this.jiHeDataGrid1.Rows.Remove(node);
        }

        private void FuZhi(List< DataGridViewRow> node)
        {
            FuZhiModel.Clear();
            for (int i = 0; i < node.Count; i++)
            {
                ZhongJianModel zhongJianModel = GetZhongJianModel(node[i]);
                if (zhongJianModel != null)
                {
                    FuZhiModel.Add(zhongJianModel);
                }
            }
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.jiHeDataGrid1.SelectedRows.Count>0)
            {
                for (int i = 0; i < this.jiHeDataGrid1.SelectedRows.Count; i++)
                {
                    DataGridViewRow node = this.jiHeDataGrid1.SelectedRows[i];
                    YiDong(true, node);
                }
                
            }
            else
            {
                this.QiDongTiShiKuang("没有选中要删除的项");
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is List<DataGridViewRow>)
            {
                List<DataGridViewRow> lis = this.contextMenuStrip1.Tag as List<DataGridViewRow>;
                for (int i = 0; i < lis.Count; i++)
                {
                    ShanChu(lis[i]);
                }              
            }
        }


        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is List<DataGridViewRow>)
            {
                List<DataGridViewRow> lis = this.contextMenuStrip1.Tag as List<DataGridViewRow>;
                FuZhi(lis);
            }
        }

        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStrip1.Tag is List<DataGridViewRow>)
            {
                if (FuZhiModel!=null)
                {
                    List< DataGridViewRow> node = this.contextMenuStrip1.Tag as List<DataGridViewRow>;
                    if (node.Count > 0)
                    {
                        int xinindex = node[0].Index + 1;
                        for (int i = 0; i < FuZhiModel.Count; i++)
                        {
                            this.jiHeDataGrid1.Rows.Insert(xinindex, new DataGridViewRow());
                            FuZhiData(FuZhiModel[i], this.jiHeDataGrid1.Rows[xinindex]);
                            xinindex++;
                        }
                       
                    }
                }
                         
            }
            else
            {
                if (FuZhiModel != null)
                {
                    for (int i = 0; i < FuZhiModel.Count; i++)
                    {
                        int index = this.jiHeDataGrid1.Rows.Add();
                       
                        FuZhiData(FuZhiModel[i], this.jiHeDataGrid1.Rows[index]);
                      
                    }
                   
                   
                }

            }
        }

       

       

        private void button1_Click(object sender, EventArgs e)
        {
            string wenjianname = this.comboBox1.Text;
            if (string.IsNullOrEmpty(wenjianname))
            {
                this.QiDongTiShiKuang("请填写名称");
                return;
            }
            if (this.jiHeDataGrid1.Rows.Count==0)
            {
                this.QiDongTiShiKuang("还没有填写内容");
                return;
            }
            int jishuqi = 0;
            int weizhi = 1;
            List<ZhongJianModel> lismodels = new List<ZhongJianModel>();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                ZhongJianModel modes = GetZhongJianModel(this.jiHeDataGrid1.Rows[i]);
                if (modes != null)
                {
                    modes.TestModel.WeiZhi = weizhi;
                    modes.TestModel.XuHaoID = $"{weizhi}";
                    weizhi++;                 
                    jishuqi++;
                    lismodels.Add(modes);
                }
            }
            if (lismodels.Count==0)
            {
                this.QiDongTiShiKuang("内容填写不对");
                return;
            }
            ZongTestModel xinmodel = new ZongTestModel();
            xinmodel.PeiFangDuiYingMa.Clear();
            xinmodel.ShouWeiChuLi = ChangYong.FuZhiShiTi(LisShouWeis);
            xinmodel.PeiFangDuiYingMa.AddRange(Mas.ToArray());
            xinmodel.JiaJuNames.AddRange(JiaJuHao.ToArray());
            xinmodel.Name = wenjianname;
            xinmodel.TestCount = jishuqi;
            xinmodel.ZhongJianModels = ChangYong.FuZhiShiTi(lismodels);
            PeiFangLei.BaoCun(xinmodel, wenjianname);
            this.QiDongTiShiKuang("保存成功");
            
            IsShuaXin = true;
            JiaZaiLiaoHao(false);
            this.comboBox1.Text = wenjianname;
            IsShuaXin = false;
        }

        private void jiHeDataGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.jiHeDataGrid1.SelectedRows.Count > 0)
                {
                    this.删除ToolStripMenuItem.Visible = true;
                    this.复制ToolStripMenuItem.Visible = true;
                    this.插入ToolStripMenuItem.Visible = false;
                    if (FuZhiModel.Count>0)
                    {
                        this.插入ToolStripMenuItem.Visible = true;
                    }
                    List<DataGridViewRow> lis = new List<DataGridViewRow>();
                  
                    foreach (var item in this.jiHeDataGrid1.SelectedRows)
                    {
                        lis.Add(item as DataGridViewRow);
                    }
                    lis.Sort((x, y) =>
                    {
                        if (x.Index > y.Index)
                        {
                            return 1;
                        }
                        else
                        {
                            return -1;
                        }
                    });
                 
                    this.contextMenuStrip1.Tag = lis;
                    this.contextMenuStrip1.Show(this.jiHeDataGrid1, e.X, e.Y);
                }
                else
                {
                    if (FuZhiModel.Count>0)
                    {
                        this.删除ToolStripMenuItem.Visible = false;
                        this.复制ToolStripMenuItem.Visible = false;
                        this.插入ToolStripMenuItem.Visible = true;
                        this.contextMenuStrip1.Tag = null;
                        this.contextMenuStrip1.Show(this.jiHeDataGrid1, e.X, e.Y);
                    }
                }
            }
        }

        private void jiHeDataGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    string gongnengma = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                   
                    XuanZseFrm fem = new XuanZseFrm();
                    fem.SetCanShu(PeiFangLei.JianCeDui.GetGongNeng(), gongnengma);
                    if (fem.ShowDialog(this) == DialogResult.OK)
                    {
                        this.jiHeDataGrid1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = fem.JieGuo;
                    }
                }
                else if (e.ColumnIndex == 2)
                {
                    string gongnengma = this.jiHeDataGrid1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    if (string.IsNullOrEmpty(gongnengma))
                    {
                        this.QiDongTiShiKuang("没有选择功能");
                        return;
                    }
                    DataGridViewRow row = this.jiHeDataGrid1.Rows[e.RowIndex];
                    string shebei = row.Cells[e.ColumnIndex].Value.ToString();
                    ZhongJianModel model = ChangYong.FuZhiShiTi(GetZhongJianModel(row));                  
                    XuanZeJiCunPeiZhiFrm fem = new XuanZeJiCunPeiZhiFrm(PeiFangLei);
                    fem.SetCanShu(model,"1");
                    if (fem.ShowDialog(this) == DialogResult.OK)
                    {
                        model = ChangYong.FuZhiShiTi(fem.ZhongJianModel);
                        FuZhi(model.TestModel, row, model);
                    }
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.xlsx|*.xlsx";

            saveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-dd");
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                String wenjianming = saveFileDialog.FileName;
                DuRuExclWenDan duRuExclWenDan = new DuRuExclWenDan();
                Dictionary<String, string> dic = new Dictionary<String, string>();

                TestModel model = new TestModel();
               
                duRuExclWenDan.DaoChuExc(wenjianming, GetDaoChuNeiRong(model.GetData()));
            }
        }
        public Dictionary<string, List<object>> GetDaoChuNeiRong(List<string> lies)
        {
            Dictionary<string, List<object>> BiaoGe = new Dictionary<string, List<object>>();
            List<TestModel> LieModels = new List<TestModel>();
            for (int i = 0; i < this.jiHeDataGrid1.Rows.Count; i++)
            {
                ZhongJianModel modes = GetZhongJianModel(this.jiHeDataGrid1.Rows[i]);
                if (modes != null)
                {

                    LieModels.Add(modes.TestModel);
                }
            }
          
            if (LieModels is IList)
            {


                foreach (var item in LieModels)
                {

                    for (int i = 0; i < lies.Count; i++)
                    {

                        object zhi = ConvertEnumerationItem(item, lies[i]);
                        if (BiaoGe.ContainsKey(lies[i]) == false)
                        {
                            BiaoGe.Add(lies[i], new List<object>());
                        }
                        BiaoGe[lies[i]].Add(zhi);

                    }

                }
            }
            return BiaoGe;
        }
        private object ConvertEnumerationItem(object item, string fieldName)
        {
            DataRow row = item as DataRow;
            if (row != null)
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    if (row.Table.Columns.Contains(fieldName))
                        return row[fieldName];
                }
                return row[0];
            }
            else
            {
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(item).Find(fieldName, true);
                if (descriptor != null)
                    return (descriptor.GetValue(item) ?? null);
            }
            return null;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String wenjianming = openFileDialog.FileName;
                DuRuExclWenDan duRuExclWenDan = new DuRuExclWenDan();
                Dictionary<String, string> dic = new Dictionary<String, string>();

                TestModel model = new TestModel();
                List<string> list = model.GetData();
                for (int i = 0; i < list.Count; i++)
                {
                    if (dic.ContainsKey(list[i])==false)
                    {
                        dic.Add(list[i], list[i]);
                    }
                }
                List<TestModel> Plc_list = duRuExclWenDan.ShuChuExcelOrLis<TestModel>(wenjianming, dic);


                for (int i = 0; i < Plc_list.Count; i++)
                {
                    Plc_list[i].SheBeiID = SheBeiJiHe.Cerate().GetSheBeiID(Plc_list[i].SheBeiName);
                }
                this.jiHeDataGrid1.Rows.Clear();
                for (int i = 0; i < Plc_list.Count; i++)
                {
                    ZhongJianModel zongTestModel = new ZhongJianModel();
                    zongTestModel.TestModel = Plc_list[i];
                    int index = this.jiHeDataGrid1.Rows.Add();
                    FuZhiData(zongTestModel, this.jiHeDataGrid1.Rows[index]);

                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PeiZhiShouWeiFrm peiZhiShouWeiFrm = new PeiZhiShouWeiFrm();
            peiZhiShouWeiFrm.SetCanShu(LisShouWeis);
            if (peiZhiShouWeiFrm.ShowDialog(this)==DialogResult.OK)
            {
                LisShouWeis =ChangYong.FuZhiShiTi( peiZhiShouWeiFrm.GetCanShu());
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            QieHuanPeiFangFrm duiYingMaFrm = new QieHuanPeiFangFrm();
            duiYingMaFrm.SetCanShu(Mas);
            if (duiYingMaFrm.ShowDialog(this) == DialogResult.OK)
            {
                Mas = ChangYong.FuZhiShiTi(duiYingMaFrm.MaS);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DuiYingMaFrm duiYingMaFrm = new DuiYingMaFrm();
            duiYingMaFrm.SetCanShu(JiaJuHao);
            if (duiYingMaFrm.ShowDialog(this)==DialogResult.OK)
            {
                JiaJuHao = ChangYong.FuZhiShiTi(duiYingMaFrm.MaS);
            }
        }

        
    }
}
