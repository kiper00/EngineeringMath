using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Windows;

namespace Proj3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int func = 1;
        bool apendInv = false;
        DataManager dm;

        private void Form1_Load(object sender, EventArgs e)
        {
            dm = new DataManager();

            radioButton1.CheckedChanged += rbtn_CheckedChanged;
            radioButton3.CheckedChanged += rbtn_CheckedChanged;
            radioButton6.CheckedChanged += rbtn_CheckedChanged;

            SetLayout();
        }

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            func = int.Parse(((RadioButton)sender).Tag.ToString());
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            dm.FilterD = ((TrackBar)sender).Value / 100.0;
            lblRdi.Text = "Radius  " + dm.FilterD;

            if (func == 1 || dm.Img_Fre == null) return;
            if (func == 2)
                Fourier.PassFilter(false, dm.M, dm.N, dm.FilterD, dm.FilterN, ref dm.Freq, out dm.Aftf);
            else
                Fourier.PassFilter(true, dm.M, dm.N, dm.FilterD, dm.FilterN, ref dm.Freq, out dm.Aftf);

            if (apendInv)
            {
                Fourier.FFT(false, dm.M, dm.N, ref dm.Aftf, out dm.Invr);
                SetImage(dm.M, dm.N, ref dm.Invr, out dm.Img_Fnl);
            }

            SetImage(dm.M, dm.N, ref dm.Aftf, out dm.Img_Aft);
            SetLayout();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            dm.FilterN = ((TrackBar)sender).Value;
            lblOrd.Text = "Order  " + dm.FilterN;

            if (func == 1) return;
            if (func == 2)
                Fourier.PassFilter(false, dm.M, dm.N, dm.FilterD, dm.FilterN, ref dm.Freq, out dm.Aftf);
            else
                Fourier.PassFilter(true, dm.M, dm.N, dm.FilterD, dm.FilterN, ref dm.Freq, out dm.Aftf);

            if (apendInv)
            {
                Fourier.FFT(false, dm.M, dm.N, ref dm.Aftf, out dm.Invr);
                SetImage(dm.M, dm.N, ref dm.Invr, out dm.Img_Fnl);
            }

            SetImage(dm.M, dm.N, ref dm.Aftf, out dm.Img_Aft);
            SetLayout();
        }
        
        private void btnRead_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "所有圖片檔案(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|圖檔(*.jpg)|*.jpg|點陣圖檔(*.bmp)|*.bmp|透明圖檔(*.png)|*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                dm.SetInput(new Bitmap(Image.FromFile(openFileDialog1.FileName)));
                SetLayout();
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            apendInv = !apendInv;

            btn3.BackColor = apendInv ? Color.FromArgb(224, 16, 96) : Color.FromArgb(232, 232, 232);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            panel1.Size = new Size(this.ClientSize.Width - 30, this.ClientSize.Height - 318);

            int newsize = pbx1.Width * 2;
            if (newsize > 1024) newsize = 256;

            pbx1.Size = pbx2.Size = pbx3.Size = pbx4.Size = new Size(newsize, newsize);
            pbx2.Location = new Point(pbx1.Location.X + newsize + 20, pbx1.Location.Y);
            pbx3.Location = new Point(pbx2.Location.X + newsize + 20, pbx1.Location.Y);
            pbx4.Location = new Point(pbx3.Location.X + newsize + 20, pbx1.Location.Y);
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            if (dm.Img_Ori == null)
            {
                MessageBox.Show("未設定輸入！");
                return;
            }

            //step1:FFT
            Fourier.FFT(true, dm.M, dm.N, ref dm.Orig, out dm.Freq);
            
            //step2:
            if (func == 1)//FFT
            {
                dm.Aftf = new List<Complex>(dm.Freq);
                dm.Img_Aft = dm.Img_Fre;
            }
            else if (func == 2)//LPF
                Fourier.PassFilter(false, dm.M, dm.N, dm.FilterD, dm.FilterN, ref dm.Freq, out dm.Aftf);
            else if (func == 3)//HPF
                Fourier.PassFilter(true, dm.M, dm.N, dm.FilterD, dm.FilterN, ref dm.Freq, out dm.Aftf);
            
            //step3:IFFT
            Fourier.FFT(false, dm.M, dm.N, ref dm.Aftf, out dm.Invr);


            //step4:set image imformation
            SetImage(dm.M, dm.N, ref dm.Freq, out dm.Img_Fre);
            if (func != 1)
                SetImage(dm.M, dm.N, ref dm.Aftf, out dm.Img_Aft);
            else
                dm.Img_Aft = dm.Img_Fre;
            SetImage(dm.M, dm.N, ref dm.Invr, out dm.Img_Fnl);

            //step5:set image to viewport
            SetLayout();
        }



        
        private void SetLayout()
        {
            pbx1.Image = dm.Img_Ori;
            pbx2.Image = dm.Img_Fre; 
            pbx3.Image = dm.Img_Aft;
            pbx4.Image = dm.Img_Fnl;
        }

        private void SetImage(int h, int w, ref List<Complex> ls, out Bitmap Img)
        {
            Img = new Bitmap(w, h);
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    int val = (int)ls[i * w + j].Magnitude;
                    if (val > 255) val = 255;
                    if (val < 0) val = 0;

                    Img.SetPixel(j, i, Color.FromArgb(val, val, val));
                }
        }
    }

    public class DataManager
    {
        /// <summary>
        /// 原圖。
        /// </summary>
        public Bitmap Img_Ori;
        /// <summary>
        /// 頻域圖。
        /// </summary>
        public Bitmap Img_Fre;
        /// <summary>
        /// 頻域處理圖。
        /// </summary>
        public Bitmap Img_Aft;
        /// <summary>
        /// 結果圖。
        /// </summary>
        public Bitmap Img_Fnl;

        public int M, N;


        public double FilterD = 0.3;
        public int FilterN = 2;

        /// <summary>
        /// 原輸入陣列。Original Input
        /// </summary>
        public List<Complex> Orig;
        /// <summary>
        /// 頻率域陣列。Transform Output
        /// </summary>
        public List<Complex> Freq;
        /// <summary>
        /// 處理後頻率域陣列。After Operation Output
        /// </summary>
        public List<Complex> Aftf;
        /// <summary>
        /// 反轉換後陣列。Inverse Transform Output
        /// </summary>
        public List<Complex> Invr;

        public DataManager()
        {
            FilterD = 0.3;
            FilterN = 2;

            M = N = 0;

            Orig = Freq = Aftf = Invr = new List<Complex>();
        }

        public void Clear()
        {
            Freq = Aftf = Invr = new List<Complex>();

            Img_Fre = Img_Aft = Img_Fnl = null;
        }

        public void SetInput(Bitmap Ipt)
        {
            Img_Ori = Ipt;
            M = Ipt.Height;
            N = Ipt.Width;

            Orig = new List<Complex>();
            for (int i = 0; i < Img_Ori.Height; i++)
            {
                for (int j = 0; j < Img_Ori.Width; j++)
                {
                    Color cr = Img_Ori.GetPixel(j, i);
                    Orig.Add(new Complex((int)(cr.R * 0.299 + cr.G * 0.587 + cr.B * 0.114) * (((i + j) & 1) == 0 ? 1 : -1), 0));
                }
            }

            Clear();
        }
    }

    public class Fourier
    {
        /*
        public static double FilterD = 0.3;
        public static int FilterN = 2;

        
        /// <summary>
        /// 原輸入陣列。Original Input
        /// </summary>
        private static List<Complex> X = new List<Complex>();

        /// <summary>
        /// 轉換後陣列。Transform Output
        /// </summary>
        public static List<Complex> F = new List<Complex>();

        /// <summary>
        /// 反轉換後陣列。Inverse Transform Output
        /// </summary>
        private static List<Complex> iF = new List<Complex>();
        */
        
        public Fourier()
        {

        }

        private static void BitReversal(int N, List<Complex> ipt, ref List<Complex> opt)
        {
            //List<int> order = new List<int>();

            opt.Clear();
            opt = Enumerable.Repeat((Complex)0, N).ToList();

            int log2N = (int)(Math.Log10(N) / Math.Log10(2));

            for (int i = 0; i < N; i++)
            {
                int val = 0;
                for (int num = i, j = log2N; j > 0; j--)
                {
                    val |= (num & 1);

                    if (j == 1) break;
                    num >>= 1;
                    val <<= 1;
                }

                opt[i] = ipt[val];
                //order.Add(val);
            }
            //return order;
        }

        private static void FFT_Workshop(bool forward, int N, List<Complex> X, out List<Complex> F)
        {
            F = new List<Complex>();
            BitReversal(N, X, ref F);

            for (int k = 2; k <= N; k *= 2)
            {
                Complex eit = Complex.Exp(Complex.ImaginaryOne * ((forward ? -2.0 : 2.0) * Math.PI / k));

                for (int j = 0; j < N; j += k)
                {
                    Complex td = new Complex(1, 0);
                    for (int i = j; i < j + k / 2; i++)
                    {
                        Complex a = F[i];
                        Complex b = F[i + k / 2] * td;
                        F[i] = a + b;
                        F[i + k / 2] = a - b;
                        td *= eit;
                    }
                }
            }
            F = F.Select(c => c / new Complex(Math.Sqrt(N), 0)).ToList();
        }


        /// <summary>
        /// 實作Discrete Fourier Transform。
        /// </summary>
        /// <param name="forward">是正向轉換或反轉換。</param>
        /// <param name="M">處裡影像的高度。</param>
        /// <param name="N">處理影像的寬度。</param>
        /// <param name="X">輸入複數陣列。</param>
        /// <param name="F">輸出複數陣列。</param>
        public static void DFT(bool forward, int M, int N, ref List<Complex> X, out List<Complex> F)
        {   
            F = Enumerable.Repeat(new Complex(0, 0), M * N).ToList();

            double w = (forward ? -2.0 : 2.0) * Math.PI / (M * N);
            for (int i = 0; i < M * N; i++) 
            {
                for (int j = 0; j < M * N; j++)
                    F[i] += X[j] * Complex.Exp(Complex.ImaginaryOne * w * ((i % N) * (j % N) + (i / N) * (j / N)) * M);
                F[i] /= new Complex(N, 0);
            }
        }

        /// <summary>
        /// 實作Fast Fourier Transform。
        /// </summary>
        /// <param name="forward">是正向轉換或反轉換。</param>
        /// <param name="M">處裡影像的高度。</param>
        /// <param name="N">處理影像的寬度。</param>
        /// <param name="X">輸入複數陣列。</param>
        /// <param name="F">輸出複數陣列。</param>
        public static void FFT(bool forward, int M, int N, ref List<Complex> X, out List<Complex> F)
        {   
            F = new List<Complex>(X);
            
            List<Complex> result;
            List<List<Complex>> temp = new List<List<Complex>>();
            for (int i = 0; i < M; i++) temp.Add(new List<Complex>());
            for (int i = 0; i < M * N; i++) temp[i / N].Add(F[i]);
            
            for (int i = 0; i < M; i++)
            {
                FFT_Workshop(forward, N, temp[i], out result);
                for (int j = 0; j < N; j++)
                    F[i * N + j] = result[j];
            }


            temp = new List<List<Complex>>();
            for (int i = 0; i < N; i++) temp.Add(new List<Complex>());
            for (int i = 0; i < M * N; i++) temp[i % N].Add(F[i]);

            for (int i = 0; i < N; i++)
            {
                FFT_Workshop(forward, M, temp[i], out result);
                for (int j = 0; j < M; j++)
                    F[j * N + i] = result[j];
            }
/*
            FFT_Workshop(forward, M * N, X, out F);*/
        }

        /// <summary>
        /// 實作Pass Filter。
        /// </summary>
        /// <param name="HighP">是高通濾波器或低通濾波器。</param>
        /// <param name="M">處理影像的高度。</param>
        /// <param name="N">處理影像的寬度。</param>
        /// <param name="D">濾波器的截止點。scaler between 0, 1</param>
        /// <param name="n">濾波器的階數。at least 1</param>
        /// <param name="F">輸入複數陣列。</param>
        /// <param name="aF">輸出複數陣列。</param>
        public static void PassFilter(bool HighP, int M, int N, double D, int n, ref List<Complex> F, out List<Complex> aF)
        {
            aF = new List<Complex>(F);
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++) 
                {
                    double dist = Math.Sqrt(Math.Pow((j - N / 2), 2) + Math.Pow((i - M / 2), 2)) / (M / 2);
                    double rate = 1 / (1 + Math.Pow((dist / D), 2 * n));
                    if (HighP) rate = 1 - rate;
                    aF[i * N + j] *= rate;
                }
        }
    }
}
