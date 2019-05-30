using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Proj2new
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte funccode = 1;
        VarManage vm;

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.CheckedChanged += rbtnFunction_CheckedChanged;
            radioButton2.CheckedChanged += rbtnFunction_CheckedChanged;
            radioButton3.CheckedChanged += rbtnFunction_CheckedChanged;
            radioButton4.CheckedChanged += rbtnFunction_CheckedChanged;
            radioButton5.CheckedChanged += rbtnFunction_CheckedChanged;
            
            Optimization.layout = tbxOpt;

            vm = new VarManage();
            VarManage.lbx = listBox1;
        }

        private void rbtnFunction_CheckedChanged(object sender, EventArgs e)
        {
            funccode = byte.Parse(((RadioButton)sender).Tag.ToString());
        }

        private void btnIptclear_Click(object sender, EventArgs e)
        {
            tbxInvalX.Text = "";
            tbxInvalY.Text = "";
            tbxIpt.Text = "";
            tbxPt.Text = "";
        }

        private void btnOptclear_Click(object sender, EventArgs e)
        {
            tbxOpt.Text = "";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "文字檔(*.txt)|*.txt";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                vm.Load(openFileDialog1.FileName);
            }
        }

        private void btnVarClear_Click(object sender, EventArgs e)
        {
            vm.Clear();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBox1.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBox1.SelectedItem = listBox1.Items[index];
                    if (vm[index] != null)
                        tbxIpt.Text = "" + vm[index];
                }
            }
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            Formula fm = new Formula();
            try { fm = new Formula(tbxIpt.Text); }
            catch (Exception)
            {
                MessageBox.Show("函數表示有誤！");
                return;
            }
            //先解析函數，排除例外

            Vector inipoint = new Vector(2);
            try
            {
                if (fm.ftype == Formula.FormulaType.TwoVar)
                {
                    string[] tmp = tbxPt.Text.Split(',');
                    inipoint[0] = double.Parse(tmp[0]);
                    inipoint[1] = double.Parse(tmp[1]);
                }
                else if (fm.ftype == Formula.FormulaType.Single_X)
                    inipoint[0] = double.Parse(tbxPt.Text);
                else if (fm.ftype == Formula.FormulaType.Single_Y)
                    inipoint[1] = double.Parse(tbxPt.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("起始點設定表示有誤！");
                return;
            }
            //解析起始點，排除例外

            try
            {
                if (funccode == 3)
                {
                    Optimization.Newton(fm, inipoint);
                    return;
                }
                if (funccode == 4)
                {
                    Optimization.QuasiNewton(fm, inipoint);
                    return;
                }
            }
            catch (Exception exp)
            {
                tbxOpt.Text += exp.Message + "\r\n";
            }
            //Newton 不須輸入區間

            Vector intvalX = new Vector(2);
            Vector intvalY = new Vector(2);
            try
            {
                string[] tmp;
                if (fm.ftype == Formula.FormulaType.Single_X || fm.ftype == Formula.FormulaType.TwoVar)
                {
                    tmp = tbxInvalX.Text.Split(',');
                    intvalX[0] = double.Parse(tmp[0]);
                    intvalX[1] = double.Parse(tmp[1]);
                }
                if (fm.ftype == Formula.FormulaType.Single_Y || fm.ftype == Formula.FormulaType.TwoVar)
                {
                    tmp = tbxInvalY.Text.Split(',');
                    intvalY[0] = double.Parse(tmp[0]);
                    intvalY[1] = double.Parse(tmp[1]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("區間設定表示有誤！");
                return;
            }
            //解析區間，排除例外

            try
            {
                if (funccode == 1)
                    Optimization.Powell(fm, inipoint, intvalX, intvalY);
                else if (funccode == 2)
                    Optimization.SteepDescent(fm, inipoint, intvalX, intvalY);
                else if (funccode == 5)
                    Optimization.Conjugate(fm, inipoint, intvalX, intvalY);
            }
            catch (Exception exp)
            {
                tbxOpt.Text += exp.Message + "\r\n";
            }
            //執行function
        }
    }

    public class VarManage
    {
        private List<string> fmls;
        public static ListBox lbx;

        public VarManage()
        {
            fmls = new List<string>();
        }

        public string this[int index]
        {
            get
            {
                if (index >= 0 && index < fmls.Count)
                    return fmls[index];
                else
                    return null;
            }
        }

        public void Load(string path)
        {
            List<string> input = File.ReadAllLines(path).ToList();

            foreach(string str in input)
            {
                if (str == "") continue;
                else
                    fmls.Add(str);
                UpdateView();
            }
        }

        public void Clear()
        {
            fmls.Clear();
            UpdateView();
        }

        private void UpdateView()
        {
            lbx.Items.Clear();
            for (int i = 0; i < fmls.Count; i++) 
            {
                lbx.Items.Add(fmls[i]);
            }
        }
    }
}
