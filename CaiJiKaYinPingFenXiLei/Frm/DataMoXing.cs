using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommLei.JiChuLei;
using SSheBei.CRCJiaoYan;
using SSheBei.Model;
using YiBanSaoMaQi.Model;
using CommLei.DataChuLi;
using System.IO;
using System.Drawing;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using BaseUI.UC;



namespace YiBanSaoMaQi.Frm
{
    /// <summary>
    /// 模型数据
    /// </summary>
    public class DataMoXing
    {
        /// <summary>
        /// 设备id
        /// </summary>
        public int SheBeiID { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string SheBeiName { get; set; } = "";
        /// <summary>
        /// 读寄存器
        /// </summary>
        public List<JiCunQiModel> LisDu = new List<JiCunQiModel>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisXie = new List<JiCunQiModel>();
        /// <summary>
        /// 写寄存器
        /// </summary>
        public List<JiCunQiModel> LisDuXie = new List<JiCunQiModel>();

        /// <summary>
        /// 设备
        /// </summary>
        public List<SaoMaModel> LisSheBei = new List<SaoMaModel>();

        /// <summary>
        /// 写标识的对应 key表示寄存器的唯一表示
        /// </summary>
        public Dictionary<string, CunModel> JiLu = new Dictionary<string, CunModel>();

        private List<string> KeyS = new List<string>();

        /// <summary>
        /// 收集值
        /// </summary>
        public  List<double> YuanShiDian = new List<double>();

        /// <summary>
        /// 收集值
        /// </summary>
        public List<double> ZaoYing = new List<double>();
        /// <summary>
        /// 收集值
        /// </summary>
        public List<double> LvBoDianDian = new List<double>();

        /// <summary>
        /// 收集值
        /// </summary>
        public List<PointF> PingLvDian = new List<PointF>();

        /// <summary>
        /// 用于初始化
        /// </summary>
        public void IniData(string lujing)
        {
            LisDu.Clear();
            LisXie.Clear();
            JiLu.Clear();
            LisDuXie.Clear();
            JosnOrSModel JosnOrSModel = new JosnOrSModel(lujing);
            LisSheBei = JosnOrSModel.GetLisTModel<SaoMaModel>();
            if (LisSheBei == null)
            {
                LisSheBei = new List<SaoMaModel>();
            }
            foreach (SaoMaModel item in LisSheBei)
            {
                List<string> jicunqimodels = ChangYong.MeiJuLisName(typeof(CunType));
                for (int c = 0; c < jicunqimodels.Count; c++)
                {
                    CunType cuntype= ChangYong.GetMeiJuZhi<CunType>(jicunqimodels[c]);
                    if (jicunqimodels[c].ToLower().StartsWith("du"))
                    {
                        CunModel cunmodel = new CunModel();
                        cunmodel.ZongSheBeiId = item.SheBeiID;
                        cunmodel.IsDu = cuntype;
                       
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        model.WeiYiBiaoShi = $"{item.Name}-{jicunqimodels[c]}";
                        model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                        model.DuXie = 1;
                        LisDu.Add(model);
                        LisDuXie.Add(model);
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            cunmodel.ZongSheBeiId = item.SheBeiID;
                            cunmodel.JiCunQi = model;
                            JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                        }
                    }
                    else
                    {
                        CunModel cunmodel = new CunModel();
                        cunmodel.ZongSheBeiId = item.SheBeiID;
                        cunmodel.IsDu = cuntype;
                     
                        JiCunQiModel model = new JiCunQiModel();
                        model.SheBeiID = SheBeiID;
                        model.WeiYiBiaoShi = $"{item.Name}-{jicunqimodels[c]}";
                        model.MiaoSu = ChangYong.GetEnumDescription(cunmodel.IsDu);
                        model.DuXie = 2;
                        LisXie.Add(model);
                        LisDuXie.Add(model);
                        if (JiLu.ContainsKey(model.WeiYiBiaoShi) == false)
                        {
                            cunmodel.ZongSheBeiId = item.SheBeiID;
                            cunmodel.JiCunQi = model;
                            JiLu.Add(model.WeiYiBiaoShi, cunmodel);
                        }
                    }
                
                   
                }
            }
           
            KeyS = JiLu.Keys.ToList();
        }

   
        public void SetHeGe(int zongid,bool zhuangtai)
        {
        
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == zongid)
                {
                    LisSheBei[i].TX = zhuangtai;
                    for (int c = 0; c < KeyS.Count; c++)
                    {
                        if (JiLu[KeyS[c]].ZongSheBeiId== zongid)
                        {
                            JiLu[KeyS[c]].JiCunQi.IsKeKao = zhuangtai;
                        }
                    }
                    break;
                }
            }
        }

        public void SetJiCunQiValue(string weiyibiaoshi, string shuju)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];            
                cunModel.JiCunQi.Value = shuju;           
            }
        
        }
        public void SetJiCunQiValue(CunModel model, double[] shuju)
        {
            SaoMaModel shebeimodel = GetSheBeiModel(model);
            if (JiLu.ContainsKey(model.JiCunQi.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.JiCunQi.WeiYiBiaoShi];

                if (cunModel.IsDu==CunType.XieKaiShiCaiJi)
                {
                    cunModel.JiCunQi.Value = "OK";
                    cunModel.JiCunQi.IsKeKao = true;
                    double pinlu = 0;
                    double fuzhi = 0;
                    double snr = 0;
                    double linmindu = 0;
                    PingLvDian=  FourierTransform.PinLvYuFuZhi(shuju, shebeimodel.CaiYangLv,shebeimodel.FuZhi,out pinlu,out fuzhi);
                    FourierTransform.QiuSNROrLinMinDu(shuju, ZaoYing.ToArray(),shebeimodel.ZaoYing,shebeimodel.CaiYangLv,shebeimodel.LinMinDuadVoltage,shebeimodel.LinMinDuFaDaZengYi,shebeimodel.CaiJiMiaoSu,shebeimodel.linMinDuDaQiYa,out snr,out linmindu);
                    for (int c = 0; c < KeyS.Count; c++)
                    {
                        if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId)
                        {
                            if (JiLu[KeyS[c]].IsDu == CunType.DuShuJuPingLv)
                            {

                                JiLu[KeyS[c]].JiCunQi.IsKeKao = true;
                                JiLu[KeyS[c]].JiCunQi.Value = pinlu;

                            }
                            else if (JiLu[KeyS[c]].IsDu == CunType.DuShuJuFuZhi)
                            {
                                JiLu[KeyS[c]].JiCunQi.IsKeKao = true;
                                JiLu[KeyS[c]].JiCunQi.Value = fuzhi;
                            }
                            else if (JiLu[KeyS[c]].IsDu == CunType.DuShuJuLinMinDu)
                            {
                                JiLu[KeyS[c]].JiCunQi.IsKeKao = true;
                                JiLu[KeyS[c]].JiCunQi.Value = linmindu;
                            }
                            else if (JiLu[KeyS[c]].IsDu == CunType.DuShuJuXinZaoBi)
                            {
                                JiLu[KeyS[c]].JiCunQi.IsKeKao = true;
                                JiLu[KeyS[c]].JiCunQi.Value = snr;
                            }
                        }
                    }

                }
            
                       
            }
          

        }
        public void SetZhengZaiValue(string weiyibiaoshi,int sate)
        {
            if (JiLu.ContainsKey(weiyibiaoshi))
            {
                CunModel cunModel = JiLu[weiyibiaoshi];              
                cunModel.IsZhengZaiCe = sate;
                if (cunModel.IsZhengZaiCe == 0)
                {
                    cunModel.JiCunQi.Value = "";
                    YuanShiDian.Clear();
                    LvBoDianDian.Clear();
                    PingLvDian.Clear(); 
                    if (cunModel.IsDu == CunType.XieKaiShiCaiJi)
                    {
                    
                        for (int c = 0; c < KeyS.Count; c++)
                        {
                            if (JiLu[KeyS[c]].ZongSheBeiId == cunModel.ZongSheBeiId)
                            {
                                if (JiLu[KeyS[c]].IsDu.ToString().ToLower().StartsWith("du"))
                                {
                                    JiLu[KeyS[c]].JiCunQi.Value = "";
                                    JiLu[KeyS[c]].IsZhengZaiCe = 1;
                                }

                            }
                        }
                    }
                }           
            }

        }

     
        public CunModel GetModel(JiCunQiModel model)
        {
            if (JiLu.ContainsKey(model.WeiYiBiaoShi))
            {
                CunModel cunModel = JiLu[model.WeiYiBiaoShi];
                return cunModel;
            }
            return null;
        }

   

        public SaoMaModel GetSheBeiModel(CunModel model)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID==model.ZongSheBeiId)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }

        public SaoMaModel GetSheBeiModel(int shebeiid)
        {
            for (int i = 0; i < LisSheBei.Count; i++)
            {
                if (LisSheBei[i].SheBeiID == shebeiid)
                {
                    return LisSheBei[i];
                }
            }
            return null;
        }


    }




    public static class FourierTransform
    {
        /// <summary>
        /// 计算信号的均方根（RMS，代表功率）
        /// </summary>
        private static double CalculateRMS(double[] signal)
        {
            if (signal.Length == 0) return 0;
            double sumOfSquares = signal.Sum(x => x * x);
            return Math.Sqrt(sumOfSquares / signal.Length);
        }

        /// <summary>
        /// 计算信噪比（SNR）
        /// </summary>
        /// <param name="signalSamples">信号段采样数据</param>
        /// <param name="noiseSamples">噪声段采样数据</param>
        /// <returns>SNR（dB）</returns>
        private static double CalculateSNR(double[] signalSamples, double[] noiseSamples,double zaoyin)
        {
            double signalRms = CalculateRMS(signalSamples);
            double noiseRms = zaoyin*zaoyin;

            if (noiseRms == 0)
            {
                return 0;
            }
            if (signalRms == 0)
            {
                return -9999999;
            }

            // SNR = 10 * log10(信号功率 / 噪声功率)，功率与RMS²成正比
            return 10 * Math.Log10(Math.Pow(signalRms, 2) / Math.Pow(noiseRms, 2));
        }

        /// <summary>
        /// 计算麦克风灵敏度（dBV/Pa）
        /// </summary>
        /// <param name="referenceSamples">参考声压（1Pa）下的采样数据</param>
        /// <param name="adVoltageRange">AD转换器满量程电压（如5V）</param>
        /// <param name="gainDb">放大器增益（dB，如20dB）</param>
        /// <returns>灵敏度（dBV/Pa）</returns>
        private static double CalculateSensitivity(double[] referenceSamples, double adVoltageRange, double gainDb, double daqiya)
        {
            // 1. 计算参考信号的RMS（归一化值）
            double rmsNormalized = CalculateRMS(referenceSamples);

            // 2. 将归一化值转换为实际电压（考虑AD量程和增益）
            // 归一化值范围[-1,1]，对应AD满量程电压范围[-V/2, V/2]
            double voltage = rmsNormalized * (adVoltageRange / 2);

            // 3. 扣除放大器增益（增益使电压放大，灵敏度需还原）
            double gainLinear = Math.Pow(10, gainDb / 20); // dB转线性增益
            double voltageWithoutGain = voltage / gainLinear;

            // 4. 计算灵敏度（dBV/Pa）：20*log10(电压(V)/1Pa)
            if (voltageWithoutGain <= 0)
            {
                return -9999999;
            }
            return 20 * Math.Log10(voltageWithoutGain / daqiya); // 1.0对应1Pa
        }
        /// <summary>
        /// 频域滤波：去除特定频率（如50Hz工频噪声）
        /// </summary>
        public static double[] RemoveSpecificFrequency(double[] allSamples, int caiyanglv, double targetFreq, double bandwidth = 10)
        {
            int n = allSamples.Length;
            // 补零至2的幂次（优化FFT效率）
            int fftSize = 1;
            while (fftSize < n) fftSize <<= 1;
            double[] padded = new double[fftSize];
            Array.Copy(allSamples, padded, n);

            // 转换为复数数组（虚部为0）
            var complex = Array.ConvertAll(padded, x => new Complex32((float)x, 0));

            // 正向FFT
            Fourier.Forward(complex, FourierOptions.Matlab);

            // 构建频率掩码：衰减目标频率附近的分量
            for (int i = 0; i < fftSize; i++)
            {
                // 计算频率（Hz）
                double freq = i * (double)caiyanglv / fftSize;
                // 对称处理（FFT结果对称，需同时处理负频率）
                if (Math.Abs(freq - targetFreq) < bandwidth || Math.Abs(freq - (caiyanglv - targetFreq)) < bandwidth)
                {
                    complex[i] = new Complex32(0, 0); // 衰减目标频率
                }
            }

            // 逆FFT转换回时域
            Fourier.Inverse(complex, FourierOptions.Matlab);

            // 提取结果（去除补零部分，取实部）
            double[] output = new double[n];
            for (int i = 0; i < n; i++)
            {
                output[i] = complex[i].Real;
            }
            return output;
        }



        public static void QiuSNROrLinMinDu(double[] allSamples, double[] zaoyin,double zaoyingw, double caiyanglv, double adVoltage, double fadazengyi, double miaosu, double daqiya, out double Snr, out double linmindu)
        {

            //// 手动分割信号段和噪声段（示例：前1秒为噪声，中间2秒为信号）
            //int samplesPerSecond = (int)caiyanglv;
            //int noiseStart = 0;
            //int noiseLength = (int)(samplesPerSecond * (miaosu / 2d)); // 1秒噪声
            //int signalStart = noiseLength;
            //int signalLength = (int)(samplesPerSecond * (miaosu - miaosu / 2d)); // 2秒信号

            double[] noiseSamples = zaoyin;
            double[] signalSamples = allSamples;

            // 计算信噪比
            Snr = CalculateSNR(signalSamples, noiseSamples, zaoyingw);


            // 计算灵敏度（假设已知参考条件：1Pa声压下的信号）
            // 硬件参数示例：AD满量程5V，放大器增益20dB

            double gain = fadazengyi;
            linmindu = CalculateSensitivity(signalSamples, adVoltage, gain, daqiya); // 此处假设signalSamples是1Pa下的信号

        }
        public static List<PointF> PinLvYuFuZhi(double[] samples, double caiyanglv,double zhenfuzhi, out double zhupinlv, out double fuzhi)
        {
            List<PointF> dians = new List<PointF>();
            {
                //int n = samples.Length;
                //double peakAmp = (samples.Max() - samples.Min()) / 2; // 峰值（含直流偏移修正）
                //// 1. 补零至2的幂次（优化FFT效率，非必需但推荐）
                //int fftSize = 1;
                //while (fftSize < n) fftSize <<= 1; // 取大于等于n的最小2的幂次
                //double[] paddedData = new double[fftSize];
                //Array.Copy(samples, paddedData, n);

                //// 2. 转换为复数数组（FFT输入要求，虚部为0）
                //var complexData = Array.ConvertAll(paddedData, x => new Complex32((float)x, 0f));

                //// 3. 执行FFT（正向变换）
                //Fourier.Forward(complexData, FourierOptions.Matlab);

                //// 4. 计算幅值谱（仅取前半段，因FFT结果对称，后半段为负频率）
                //int halfSize = fftSize / 2;
                //double[] magnitudeSpectrum = new double[halfSize];
                //for (int i = 0; i < halfSize; i++)
                //{
                //    magnitudeSpectrum[i] = complexData[i].Magnitude; // 幅值 = 复数模长
                //}

                //// 5. 找到幅值最大的索引，对应主频
                //int maxIndex = 1;
                //double maxMagnitude = magnitudeSpectrum[1];
                //for (int i = 1; i < halfSize; i++) // 从1开始，排除直流分量（0Hz）
                //{
                //    if (magnitudeSpectrum[i] > maxMagnitude)
                //    {
                //        maxMagnitude = magnitudeSpectrum[i];
                //        maxIndex = i;
                //    }
                //    PointF dianx = new PointF();
                //    dianx.X = (float)(((double)i) * (caiyanglv / fftSize));
                //    dianx.Y = (float)(magnitudeSpectrum[i]);
                //    dians.Add(dianx);
                //}

                //// 7. 计算幅值（转换为实际电压）
                //// 步骤：原始幅值归一化 → 转换为电压
                //double normalizedAmp = magnitudeSpectrum[maxIndex] / (fftSize / 2); // FFT幅值归一化
                //double actualAmp = normalizedAmp * peakAmp; // 还原为实际电压（[-V/2, V/2]对应[-1,1]）

                //zhupinlv = ((double)maxIndex) * (caiyanglv / fftSize);
                //fuzhi = actualAmp;
                //return dians;

            }

            {
                //int n = samples.Length;

                //// 步骤1：FFT预处理（补零至2的幂次，确保FFT效率）
                //int fftSize = n;
                //if ((fftSize & (fftSize - 1)) != 0) // 若采样点数不是2的幂次
                //{
                //    fftSize = 1;
                //    while (fftSize < n) fftSize <<= 1; // 取最小的2的幂次
                //}
                //double[] paddedData = new double[fftSize];
                //Array.Copy(samples, paddedData, n);

                //// 步骤2：转换为复数数组（FFT输入要求）
                //var complexData = Array.ConvertAll(paddedData, x => new Complex32((float)x, 0f));

                //// 步骤3：执行FFT（正向变换）
                //Fourier.Forward(complexData, FourierOptions.Matlab);

                //// 步骤4：计算频域幅值谱（仅取前半段，实信号FFT结果对称）
                //int halfSize = fftSize / 2;
                //double[] magnitudes = new double[halfSize];
                //for (int i = 0; i < halfSize; i++)
                //{
                //    magnitudes[i] = complexData[i].Magnitude; // 复数模长（原始幅值）
                //}

                //// 步骤5：找到主频（排除直流分量i=0）
                //int peakIndex = 0;
                //double maxMagnitude = 0;
                //for (int i = 2; i < halfSize; i++)
                //{
                //    if (magnitudes[i] > maxMagnitude)
                //    {
                //        maxMagnitude = magnitudes[i];
                //        peakIndex = i;
                //    }
                //    PointF dianx = new PointF();
                //    dianx.X = (float)(((double)i) * (caiyanglv / fftSize));
                //    dianx.Y = (float)(magnitudes[i]);
                //    dians.Add(dianx);
                //}
                //zhupinlv = (double)peakIndex * caiyanglv / fftSize;

                //// 步骤6：计算电压幅值（归一化FFT结果）
                //// 归一化：消除采样点数影响（实信号FFT幅值需除以fftSize/2）
                //fuzhi = maxMagnitude / (fftSize / 2);

                //return dians;

            }
            {
                int n = samples.Length;

                // 1. 补零至2的幂次（优化FFT效率，非必需但推荐）
                int fftSize = n;
                // while (fftSize < n) fftSize <<= 1; // 取大于等于n的最小2的幂次
                double[] paddedData = new double[fftSize];
                Array.Copy(samples, paddedData, n);

                // 2. 转换为复数数组（FFT输入要求，虚部为0）
                var complexData = Array.ConvertAll(paddedData, x => new Complex32((float)x, 0f));

                // 3. 执行FFT（正向变换）
                Fourier.Forward(complexData, FourierOptions.Matlab);

                // 4. 计算幅值谱（仅取前半段，因FFT结果对称，后半段为负频率）
                int halfSize = fftSize / 2;
                double[] magnitudeSpectrum = new double[halfSize];
                for (int i = 0; i < halfSize; i++)
                {
                    magnitudeSpectrum[i] = complexData[i].Magnitude; // 幅值 = 复数模长
                }

                // 5. 找到幅值最大的索引，对应主频
                int maxIndex = 0;
                double maxMagnitude = 0;
                for (int i = 2; i < halfSize; i++) // 从1开始，排除直流分量（0Hz）
                {
                    if (magnitudeSpectrum[i] > maxMagnitude)
                    {
                        maxMagnitude = magnitudeSpectrum[i];
                        maxIndex = i;
                    }
                    PointF dianx = new PointF();
                    dianx.X = (float)(((double)i) * (caiyanglv / fftSize));
                    dianx.Y = (float)(magnitudeSpectrum[i]);
                    dians.Add(dianx);
                }

                // 6. 计算频率：频率 = 索引 × (采样频率 / FFT大小)


                fuzhi = (maxMagnitude / (fftSize))*zhenfuzhi;
                zhupinlv = (double)maxIndex * caiyanglv / fftSize;
                return dians;
            }
            {
                //计算 FFT
                //var fft = new MathNet.Numerics.IntegralTransforms.Fourier(signal.Length);
                //Complex32[] spectrum = fft.Transform(signal.Select(x => new Complex32((float)x, 0)).ToArray());

                //// 找到最大幅值对应的频率
                //int maxIndex = 0;
                //float maxMagnitude = 0;
                //for (int i = 1; i < spectrum.Length / 2; i++) // 仅取前一半（对称）
                //{
                //    float magnitude = spectrum[i].Magnitude;
                //    if (magnitude > maxMagnitude)
                //    {
                //        maxMagnitude = magnitude;
                //        maxIndex = i;
                //    }
                //    PointF dianx = new PointF();
                //    dianx.X = (float)(((double)i) * (caiyanglv / fftSize));
                //    dianx.Y = (float)(magnitude);
                //    dians.Add(dianx);
                //}

                //// 计算频率（Hz）
                //double frequency = maxIndex * sampleRate / signal.Length;

                //// 幅值（峰值）
                //double amplitude = maxMagnitude * 2 / signal.Length; // 归一化

                //return (frequency, amplitude);
            }
        }

    }
}
