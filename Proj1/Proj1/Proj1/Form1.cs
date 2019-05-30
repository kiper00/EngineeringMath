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

namespace Proj1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public const double zerodelta = 0.0000000001;
        public static bool isZero(double value)
        {
            return Math.Abs(value) < zerodelta;
        }
        public static bool doubleEqual(double value1, double value2)
        {
            return Math.Abs(value1 - value2) < zerodelta;
        }

        VarManager vm;
        
        //參數緩衝陣列
        List<Vector> voperand = new List<Vector>();
        List<Matrix> moperand = new List<Matrix>();

        private void Form1_Load(object sender, EventArgs e)
        {
            vm = new VarManager(ref lbxVar);
        }

        //讀檔按鈕
        private void btnRead_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.Multiselect = true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < openFileDialog1.FileNames.Length; i++)
                    vm.ReadFile(openFileDialog1.FileNames[i]);
            }
        }

        //寫檔按鈕
        private void btnWrite_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(saveFileDialog1.FileName, tbxOpt.Text);
        }
        
        //清除資料視窗
        private void btnClear_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("是否要清除資料清單？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                vm.Clear();
            }
        }

        //清除輸出視窗
        private void btnClearO_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否要清除輸出視窗？", "確認", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                tbxOpt.Text = "";
            }
        }

        //說明按鈕
        private void btnGuide_Click(object sender, EventArgs e)
        {
            string layout = "功能說明：\r\n" +
                "選擇命令後，輸入所需的參數，按下enter後程式將輸出結果在輸出視窗。\r\n\r\n" +
                "指令列表：\r\n" +
                "選擇想要執行的指令\r\n" +
                "特別指令－c27 c28，向量與矩陣的算式指令，選取此兩指令，\r\n" +
                "輸入需在第一個輸入一個運算式，其內以單一英文字母字元（a、b、c、...、z）\r\n" +
                "當作變數，在於其後接續輸入足夠數量的參數，範例如下\r\n" +
                "a+(b-c)-(d+e) v1 v11 v8 v7 v3\r\n\r\n" +
                "資料列表：\r\n" +
                "滑鼠雙擊可顯示資料詳細資訊\r\n" +
                "滑鼠右鍵點擊可自動鍵入該資料名稱至輸入視窗\r\n" +
                "清除按鈕可以將資料列表中資料清除\r\n" +
                "讀檔按鈕選擇檔案讀入資料\r\n\r\n" +
                "輸入視窗：\r\n" +
                "可以手動輸入資料名稱或以資料列表自動鍵入，按下enter鍵執行命令\r\n\r\n" +
                "輸出視窗\r\n" +
                "結果會顯示在輸出視窗\r\n" +
                "清除按鈕可以將輸出視窗的文字清除\r\n" +
                "寫檔按鈕可以將輸出視窗的文字寫入指定的檔案";
            MessageBox.Show(layout, "說明");
        }

        //雙擊資料列表，顯示資料詳細狀態
        private void lbxVar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbxVar.SelectedIndex == -1) return;
            MessageBox.Show("" + vm.DataText(lbxVar.SelectedIndex));
        }

        //右鍵點擊資料列表，將該資料名稱自動鍵入輸入視窗
        private void lbxVar_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                int index = lbxVar.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    lbxVar.SelectedItem = lbxVar.Items[index];
                    tbxIpt.Text += "" + vm.DataName(lbxVar.SelectedIndex) + " ";
                }
                tbxIpt.Focus();
                tbxIpt.Select(tbxIpt.TextLength, 1);
            }
        }
        
        //輸入視窗按鍵
        private void tbxIpt_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)//輸入命令
            {
                /*
                 定義命令格式：
                    var1  ,  var2
                    參數依需求輸入(參數數量根據命令不同)
                    參數之間以空格分格
                    暫定參數只接受單一資料(之後再新增多數基本運算)
                 */

                //清除緩衝
                voperand.Clear();
                moperand.Clear();


                //暫存
                Vector vtemp = new Vector();
                Matrix mtemp = new Matrix();
                try
                {

                    tbxIpt.Text = tbxIpt.Text.Remove(tbxIpt.Text.Length - 2);//移除後方\r\n
                    //tbxIpt.Text = tbxIpt.Text.Replace(" ", "");//去空格                
                    List<string> fullcmd = tbxIpt.Text.Split(' ').ToList();//以空格切割，取得命令與參數名稱
                    fullcmd.RemoveAll(x => x == "");//去掉連續空格間的空字串


                    ///對應相對的命令，其所需的參數數量
                    List<int> PaNumOfCmd = new List<int>() {
                        2,2,2,1,1,2,2,2,2,2,2,2,2,10000,10000,2,2,1,1,2,1,1,1,1,1,2,1,1
                    };

                    int cmdNum = lbxCmd.SelectedIndex;
                    if ((cmdNum >= 2 && cmdNum <= 16) || (cmdNum >= 21 && cmdNum <= 33))
                    {
                        cmdNum -= (cmdNum > 16 ? 6 : 2);

                        //以迴圈方式讀入參數
                        for (int i = 0; i < PaNumOfCmd[cmdNum]; i++)
                        {
                            //尚未讀完參數但已無參數可讀
                            if (i >= fullcmd.Count)
                            {
                                //c14與c15參數為任意，因此到底後就跳出迴圈正常執行
                                if (cmdNum == 13 || cmdNum == 14)
                                    break;
                                else//其餘命令則擲例外
                                    throw new Exception("參數數目不足！");
                            }

                            if (cmdNum <= 14)//vector command
                            {
                                vm.Find(fullcmd[i], ref vtemp);
                                voperand.Add(vtemp);
                            }
                            else//matrix command
                            {
                                vm.Find(fullcmd[i], ref mtemp);
                                moperand.Add(mtemp);
                            }
                        }
                        
                        switch (cmdNum)
                        {
                            case 0:
                                tbxOpt.Text += "" + (voperand[0] * voperand[1]) + "\r\n";
                                break;
                            case 1:
                                tbxOpt.Text += "" + (voperand[0] + voperand[1]).ToSimpleString() + "\r\n";
                                break;
                            case 2:
                                tbxOpt.Text += "" + (voperand[0] * voperand[1][0]).ToSimpleString() + "\r\n";
                                break;
                            case 3:
                                tbxOpt.Text += "" + voperand[0].magnitude + "\r\n";
                                break;
                            case 4:
                                tbxOpt.Text += "" + Vector.Normalized(voperand[0]).ToSimpleString() + "\r\n";
                                break;
                            case 5:
                                tbxOpt.Text += "" + Vector.crossProduct(voperand[0], voperand[1]).ToSimpleString() + "\r\n";
                                break;
                            case 6:
                                tbxOpt.Text += "" + Vector.Component(voperand[0], voperand[1]) + "\r\n";
                                break;
                            case 7:
                                tbxOpt.Text += "" + Vector.projection(voperand[0], voperand[1]).ToSimpleString() + "\r\n";
                                break;
                            case 8:
                                tbxOpt.Text += "" + Vector.TriangleArea(voperand[0], voperand[1]) + "\r\n";
                                break;
                            case 9:
                                tbxOpt.Text += "" + (Vector.isParalle(voperand[0], voperand[1]) ? "Yes" : "No") + "\r\n";
                                break;
                            case 10:
                                tbxOpt.Text += "" + (Vector.isOrthogonal(voperand[0], voperand[1]) ? "Yes" : "No") + "\r\n";
                                break;
                            case 11:
                                tbxOpt.Text += "" + Vector.clipAngle(voperand[0], voperand[1]) + "\r\n";
                                break;
                            case 12:
                                tbxOpt.Text += "" + Vector.PlaneNormal(voperand[0], voperand[1]).ToSimpleString() + "\r\n";
                                break;
                            case 13:
                                tbxOpt.Text += "" + (Vector.isLinearIndependence(voperand) ? "Yes" : "No") + "\r\n";
                                break;
                            case 14:
                                List<Vector> layout = Vector.getOrthonormalBasis(voperand);
                                tbxOpt.Text += "normal " + voperand.Count + "\r\n";
                                for (int i = 0; i < layout.Count; i++)
                                    tbxOpt.Text += "" + layout[i].ToSimpleString() + "\r\n";
                                layout.Clear();
                                break;

                            //Matrix operation
                            case 15:
                                tbxOpt.Text += "" + (moperand[0] + moperand[1]).ToSimpleString() + "\r\n";
                                break;
                            case 16:
                                tbxOpt.Text += "" + (moperand[0] * moperand[1]).ToSimpleString() + "\r\n";
                                break;
                            case 17:
                                tbxOpt.Text += "" + moperand[0].Rank + "\r\n";
                                break;
                            case 18:
                                tbxOpt.Text += "" + Matrix.Transposed(moperand[0]).ToSimpleString() + "\r\n";
                                break;
                            case 19:
                                List<Vector> solution = Matrix.SolveLS(moperand[0]
                                    , new Vector(Matrix.Transposed(moperand[1])[0]));
                                if (solution.Count == 0)
                                    throw new Exception("解線性系統無解。");
                                else if(solution.Count == 1)
                                {//單一解
                                    tbxOpt.Text += "解線性系統得單一解：\r\n";
                                    for (int i = 0; i < solution[0].dimention; i++)
                                        tbxOpt.Text += "" + solution[0][i] + "\r\n";
                                }
                                else//參數解
                                {
                                    tbxOpt.Text += "解線性系統得參數解，參數{Pi | 1 <= i <= " 
                                        + (solution.Count - 1) + "}：\r\n";
                                    tbxOpt.Text += "X = (i = 1 ~ " + (solution.Count - 1)
                                        + ")sum(Pi * Vi) + V" + solution.Count + "\r\n";

                                    for (int k = 0; k < solution.Count; k++)
                                        tbxOpt.Text += "V" + k + (k == solution.Count - 1 ? "\r\n" : "\t");
                                    for (int i = 0; i < solution[0].dimention; i++)
                                        for (int j = 0; j < solution.Count; j++)
                                            tbxOpt.Text += "" + solution[j][i]
                                            + (j == solution.Count - 1 ? "\r\n" : "\t");
                                }
                                break;
                            case 20:
                                tbxOpt.Text += "" + moperand[0].Determinant + "\r\n";
                                break;
                            case 21:
                                tbxOpt.Text += "" + moperand[0].Inverse.ToSimpleString() + "\r\n";
                                break;
                            case 22:
                                tbxOpt.Text += "" + moperand[0].Adjoint.ToSimpleString() + "\r\n";
                                break;
                            case 23:
                                List<double> evalue = new List<double>();
                                List<Vector> evect = new List<Vector>();
                                Matrix.Eigen(moperand[0], ref evalue, ref evect);

                                tbxOpt.Text += "eigen(a):\r\n" +
                                                "v =\r\n";
                                for (int i = 0; i < evect[0].dimention; i++)
                                {
                                    for (int j = 0; j < evect.Count; j++)
                                        tbxOpt.Text += "" + evect[j][i]
                                            + (j == evect.Count - 1 ? "\r\n" : "\t");
                                }

                                tbxOpt.Text += "d =\r\n";
                                for (int i = 0; i < evalue.Count; i++)
                                    tbxOpt.Text += "" + evalue[i] + "\r\n";
                                break;
                            case 24://power method

                                List<double> evalue1 = new List<double>();
                                List<Vector> evect1 = new List<Vector>();
                                Matrix.PowerMethod(moperand[0], ref evalue1, ref evect1);

                                tbxOpt.Text += "eigen(a):\r\n" +
                                                "v =\r\n";
                                for (int i = 0; i < evect1[0].dimention; i++)
                                {
                                    for (int j = 0; j < evect1.Count; j++)
                                        tbxOpt.Text += "" + evect1[j][i]
                                            + (j == evect1.Count - 1 ? "\r\n" : "\t");
                                }

                                tbxOpt.Text += "d =\r\n";
                                for (int i = 0; i < evalue1.Count; i++)
                                    tbxOpt.Text += "" + evalue1[i] + "\r\n";
                                break;
                            case 25:
                                vtemp = Matrix.LeastSquare(moperand[0]
                                    , new Vector(Matrix.Transposed(moperand[1])[0])) ?? new Vector();
                                if (vtemp == new Vector())
                                    throw new Exception("error");
                                for (int i = 0; i < vtemp.dimention; i++)
                                    tbxOpt.Text += "" + vtemp[i] + "\r\n";
                                break;
                            case 26:
                                tbxOpt.Text += "" + Matrix.rowReduce(moperand[0]).ToSimpleString() + "\r\n";
                                break;
                            case 27:
                                Matrix lower = moperand[0];
                                lower = Matrix.Transposed(lower);
                                lower = Matrix.rowReduce(lower);
                                lower = Matrix.Transposed(lower);

                                for (int i = 0; i < lower.column; i++)
                                    for (int j = lower.row - 1; j >= i; j--)
                                        lower[j][i] /= lower[i][i];
                                tbxOpt.Text += "" + lower.ToSimpleString() + "\r\n";
                                break;
                            default:
                                tbxOpt.Text += "未知的指令！\r\n";
                                break;
                        }
                    }
                    else if(cmdNum == 38 || cmdNum == 39)
                    {
                        string formula = fullcmd[0];
                        fullcmd.RemoveAt(0);

                        if (cmdNum == 38)
                            tbxOpt.Text += FormulaCalculateV(formula, fullcmd).ToSimpleString() + "\r\n";
                        else
                            tbxOpt.Text += FormulaCalculateM(formula, fullcmd).ToSimpleString() + "\r\n";
                    }
                    else
                    {
                        tbxOpt.Text += "未知的指令！\r\n";
                    }
                }
                catch(Exception ex)
                {
                    if (ex.Message == "var doesnt exist")
                        tbxOpt.Text += "未知的參數！\r\n";
                    else
                        tbxOpt.Text += ex.Message + "\r\n";
                }


                //把輸出視窗卷軸拉到最下方
                tbxOpt.Select(tbxOpt.TextLength - 1, 1);
                tbxOpt.ScrollToCaret();
                tbxIpt.Focus();
                tbxIpt.Text = "";//清除輸入視窗
            }
        }


        void inToPostfix(string infix, ref string postfix)
        {
            List<char> stack = new List<char>();
            stack.Add('\0');
            int i, j, top;
            for (i = 0, top = 0; i < infix.Length; i++)
                switch (infix[i])
                {
                    case '(':              // 運算子堆疊 
                        stack.Add(infix[i]);
                        top++;
                        break;
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        while (priority(stack[top]) >= priority(infix[i]))
                        {
                            postfix += stack[top--];
                            stack.RemoveAt(stack.Count - 1);
                        }
                        stack.Add(infix[i]);
                        top++; // 存入堆疊 
                        break;
                    case ')':
                        while (stack[top] != '(')
                        { // 遇 ) 輸出至 ( 
                            postfix += stack[top--];
                            stack.RemoveAt(stack.Count - 1);
                        }
                        top--;  // 不輸出 ( 
                        stack.RemoveAt(stack.Count - 1);
                        break;
                    default:  // 運算元直接輸出 
                        if (infix[i] < 'a' || infix[i] > 'z')
                            throw new Exception("錯誤：函數中序轉後序中存在非a~z的字元變數。");
                        postfix += infix[i];
                        break;
                }
            while (top > 0)
            {
                postfix += stack[top--];
                stack.RemoveAt(stack.Count - 1);
            }
        }

        int priority(char op)
        {
            switch (op)
            {
                case '+': case '-': return 1;
                case '*': case '/': return 2;
                default: return 0;
            }
        }

        Vector FormulaCalculateV(string infix, List<string> paramname)
        {
            string postfix = "";
            char opnd = '\0';
            List<Vector> stack = new List<Vector>(){ new Vector() };

            inToPostfix(infix, ref postfix);

            int top, i;
            for (top = 0, i = 0; i < postfix.Length; i++) 
                switch (postfix[i])
                {
                    case '+':
                        Vector v1 = stack[top - 1], v2 = stack[top];
                        stack.RemoveRange(top - 1, 2);
                        stack.Add(v1 + v2);
                        top--;
                        break;
                    case '-':
                        Vector v3 = stack[top - 1], v4 = stack[top];
                        stack.RemoveRange(top - 1, 2);
                        stack.Add(v3 - v4);
                        top--;
                        break;
                    case '*':
                        Vector v5 = stack[top - 1], v6 = stack[top];
                        stack.RemoveRange(top - 1, 2);
                        if (v5.dimention == 1)
                            stack.Add(v5[0] * v6);
                        else if (v6.dimention == 1)
                            stack.Add(v5 * v6[0]);
                        else
                            stack.Add(new Vector(new List<double>() { v5 * v6 }));
                        top--;
                        break;
                    default:
                        opnd = postfix[i];
                        if (opnd - 'a' >= paramname.Count)
                            throw new Exception("錯誤－後序式計算參數不足。");
                        Vector v7 = new Vector();
                        vm.Find(paramname[opnd - 'a'], ref v7);
                        stack.Add(v7);
                        top++;
                        break;
                }

            return stack[top];
        }

        Matrix FormulaCalculateM(string infix, List<string> paramname)
        {
            string postfix = "";
            char opnd = '\0';
            List<Matrix> stack = new List<Matrix>() { new Matrix() };

            inToPostfix(infix, ref postfix);

            int top, i;
            for (top = 0, i = 0; i < postfix.Length; i++)
                switch (postfix[i])
                {
                    case '+':
                        Matrix v1 = stack[top - 1], v2 = stack[top];
                        stack.RemoveRange(top - 1, 2);
                        stack.Add(v1 + v2);
                        top--;
                        break;
                    case '-':
                        Matrix v3 = stack[top - 1], v4 = stack[top];
                        stack.RemoveRange(top - 1, 2);
                        stack.Add(v3 - v4);
                        top--;
                        break;
                    case '*':
                        Matrix v5 = stack[top - 1], v6 = stack[top];
                        stack.RemoveRange(top - 1, 2);
                        stack.Add(v5 * v6);
                        top--;
                        break;
                    default:
                        opnd = postfix[i];
                        if (opnd - 'a' >= paramname.Count)
                            throw new Exception("錯誤－後序式計算參數不足。");
                        Matrix v7 = new Matrix();
                        vm.Find(paramname[opnd - 'a'], ref v7);
                        stack.Add(v7);
                        top++;
                        break;
                }

            return stack[top];
        }
    }

    class Vector
    {
        public string name = "defaultname";
        private List<double> v;

        public Vector()
        {
            v = new List<double>();
        }
        public Vector(int l)
        {
            v = new List<double>();
            for (int i = 0; i < l; i++)
                v.Add(0);
        }
        public Vector(List<double> data)
        {
            v = data;
        }
        public Vector(string data)
        {
            List<char> excList = new List<char> { '-', '.', ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < data.Length; i++)
                if (!excList.Contains(data[i]))
                    throw new Exception("錯誤：Vector類別建構式傳入字串參數不符合格式。內含負號、空格、數字以外之字元。");

            string[] nums = data.Split(' ');
            List<double> para = new List<double>();
            foreach (string num in nums)
            {
                if (num == "") continue;
                para.Add(double.Parse(num));
            }

            v = para;
        }
        public Vector(Vector vec)
        {
            v = new List<double>();
            for (int i = 0; i < vec.dimention; i++)
                v.Add(vec[i]);
        }

        #region 存取介面

        public int dimention
        {
            get
            {
                return v.Count;
            }
        }
        
        public double magnitude
        {
            get
            {
                double opt = 0;
                for (int i = 0; i < dimention; i++)
                    opt += v[i] * v[i];
                return Math.Sqrt(opt);
            }
        }

        #endregion

        #region 運算子重載

        //索引存取運算子
        public double this[int index]
        {
            get
            {
                if (index >= v.Count || index < 0)
                    throw new Exception("錯誤：Vector類別索引超出維度。");
                return v[index];
            }

            set
            {
                if (index >= v.Count || index < 0)
                    throw new Exception("錯誤：Vector類別索引超出維度。");
                v[index] = value;
            }
        }

        //字串加法，方便做輸出
        public static string operator +(string str, Vector vec)
        {
            return str + vec.ToString();
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1.dimention != v2.dimention)
                throw new Exception("錯誤：Vector類別加法運算子傳入參數維度不相等。");

            Vector opt = new Vector(v1.dimention);
            for (int i = 0; i < v1.dimention; i++)
                opt[i] = v1[i] + v2[i];
            return opt;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.dimention != v2.dimention)
                throw new Exception("錯誤：Vector類別減法運算子傳入參數維度不相等。");

            Vector opt = new Vector(v1.dimention);
            for (int i = 0; i < v1.dimention; i++)
                opt[i] = v1[i] - v2[i];
            return opt;
        }

        public static double operator *(Vector v1, Vector v2)
        {
            if (v1.dimention != v2.dimention)
                throw new Exception("錯誤：Vector類別內積運算傳入參數維度不相等。");

            double opt = 0;
            for (int i = 0; i < v1.dimention; i++)
                opt += v1[i] * v2[i];
            return opt;
        }

        public static Vector operator *(Vector vec, double num)
        {
            Vector opt = new Vector(vec);
            for (int i = 0; i < opt.dimention; i++)
                opt[i] = opt[i] * num;
            return opt;
        }

        public static Vector operator *(double num, Vector vec)
        {
            Vector opt = new Vector(vec);
            for (int i = 0; i < opt.dimention; i++)
                opt[i] = opt[i] * num;
            return opt;
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            v1 = v1 ?? new Vector();
            v2 = v2 ?? new Vector();

            if (v1.dimention != v2.dimention)
                return false;

            for (int i = 0; i < v1.dimention; i++)
                if (v1[i] != v2[i])
                    return false;
            return true;
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            v1 = v1 ?? new Vector();
            v2 = v2 ?? new Vector();

            if (v1.dimention != v2.dimention)
                return false;

            for (int i = 0; i < v1.dimention; i++)
                if (v1[i] != v2[i])
                    return true;
            return false;
        }

        #endregion

        #region 函數
        
        //零向量
        public static Vector zero(int dimen)
        {
            if (dimen <= 0)
                throw new Exception("錯誤：Vector類別試圖要求零向量予以非法維度，參數－維度 = " + dimen + "。");
            return new Vector(dimen);
        }

        /// <summary>
        /// 回傳一個向量正規化後的向量。
        /// </summary>
        /// <param name="vec">目標向量。</param>
        /// <returns>目標正規化後的向量。</returns>
        public static Vector Normalized(in Vector vec)
        {
            if (vec == zero(vec.dimention))
                throw new Exception("錯誤：Vector類別試圖將零向量正規化。");

            Vector opt = new Vector(vec);
            double l = opt.magnitude;
            for (int i = 0; i < opt.dimention; i++)
                opt[i] = opt[i] / l;
            return opt;
        }

        public static Vector crossProduct(Vector u, Vector v)
        {
            if (u.dimention != 3 || v.dimention != 3)
                throw new Exception("錯誤：Vector類別外積運算傳入參數不符合定義，維度非3維向量。");

            return new Vector(new List<double>() {
                 u[1] * v[2] - u[2] * v[1],
                -u[0] * v[2] + u[2] * v[0],
                 u[0] * v[1] - u[1] * v[0]});
        }

        public static double Component(Vector vec, Vector OnNormal)
        {
            if(vec.dimention != OnNormal.dimention)
                throw new Exception("錯誤：Vector類別Component函數傳入參數維度不相等。");
            return (vec * OnNormal) / OnNormal.magnitude;
        }
        
        public static Vector projection(Vector vec, Vector OnNormal)
        {
            if (vec.dimention != OnNormal.dimention)
                throw new Exception("錯誤：Vector類別向量投影方法傳入參數維度不相等。");

            return ((vec * OnNormal) / (OnNormal * OnNormal)) * OnNormal;
        }

        public static double TriangleArea(Vector v1, Vector v2)
        {
            if (v1.dimention != v2.dimention)
                throw new Exception("錯誤：Vector類別三角面積函數傳入參數維度不相等。");
            
            return v1.magnitude * v2.magnitude * Math.Sin(clipAngle(v1, v2) / 180 * Math.PI) / 2;
        }

        public static bool isParalle(Vector v1, Vector v2)
        {
            double temp = clipAngle(v1, v2);
            return temp == 0 || temp == 180;
        }

        public static bool isOrthogonal(Vector v1, Vector v2)
        {
            return v1 * v2 == 0;
        }

        public static double clipAngle(Vector v1, Vector v2)
        {
            if (v1.dimention != v2.dimention)
                throw new Exception("錯誤：Vector類別向量夾角函數傳入參數維度不相等。");
            return Math.Acos((v1 * v2) / (v1.magnitude * v2.magnitude)) * 180 / Math.PI;
        }

        public static Vector PlaneNormal(Vector v1, Vector v2)
        {
            return crossProduct(v1, v2);
        }

        public static bool isLinearIndependence(List<Vector> vset)
        {
            //建構出矩陣，若消去後存在一列零向量，就沒有independence
            Matrix mat = new Matrix(vset);
            return mat.Rank == mat.row;
        }

        public static List<Vector> getOrthonormalBasis(List<Vector> vset)
        {
            //gram-schmidt
            List<Vector> opt = new List<Vector>();
            for (int i = 0; i < vset.Count; i++)
            {
                if (i == 0) opt.Add(vset[i]);
                else
                {
                    Vector wi = vset[i];
                    for (int j = 0; j < opt.Count; j++)
                        wi -= projection(vset[i], opt[j]);
                    opt.Add(wi);
                }
                Vector check1 = opt[0];
            }
            Vector check = opt[0];
            for (int i = 0; i < opt.Count; i++)
                opt[i].Normalize();
            return opt;
        }

        public List<double> ToList()
        {
            return v;
        }

        /// <summary>
        /// 將向量自身做正規化。
        /// </summary>
        public void Normalize()
        {
            if (this == zero(dimention))
                throw new Exception("錯誤：Vector類別試圖將零向量正規化。");

            double l = magnitude;
            for (int i = 0; i < dimention; i++)
                v[i] = v[i] / l;
        }

        //複寫轉字串方法
        public override string ToString()
        {
            string opt = "Vector { name: " + name + "  dimention: " + dimention + "  | ";
            for (int i = 0; i < dimention; i++)
                opt += (i == 0 ? "" : ", ") + v[i];
            opt += "  }";
            return opt;
        }

        public string ToSimpleString()
        {
            string opt = "";
            for (int i = 0; i < dimention; i++)
                opt += (i == 0 ? "" : " ") + v[i];
            return opt;
        }

        #endregion
    }

    class Matrix
    {
        private List<List<double>> m;
        public string name = "defaulname";

        private int rank = 0;
        private double determinant = 0;

        public Matrix()
        {
            m = new List<List<double>>();
        }
        /// <summary>
        /// 建構零矩陣，並指定行數與列數。
        /// </summary>
        /// <param name="r">列數。</param>
        /// <param name="c">行數。</param>
        public Matrix(int r, int c)
        {
            m = new List<List<double>>();

            for (int i = 0; i < r; i++)
            {
                m.Add(new List<double>());
                for (int j = 0; j < c; j++)
                    m[i].Add(0);
            }
        }
        /// <summary>
        /// 以二維LIST建構矩陣。
        /// </summary>
        /// <param name="data">二維LIST資料。</param>
        /// <param name="rowMajor">true表傳入資料參數為rowMajor，反之則為ColunmMajor。</param>
        public Matrix(List<List<double>> data)
        {
            m = data;
        }
        public Matrix(List<Vector> rowvec)
        {
            m = new List<List<double>>();
            for (int i = 0; i < rowvec.Count; i++)
            {
                if (i > 0 && rowvec[i].dimention != rowvec[0].dimention)
                    throw new Exception("錯誤：Matrix類別建構式傳入參數不相容。列向量建構式－行數不等。");
                m.Add(rowvec[i].ToList());
            }
        }
        public Matrix(List<string> data)
        {
            int myrow = data.Count;
            if (myrow == 0)
                throw new Exception("錯誤：Matrix類別建構式嘗試建立空矩陣。string data is null");

            List<char> excList = new List<char> { '-', '.', ' ', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int mycolunm = -1;
            for (int k = 0; k < myrow; k++)
                for (int i = 0; i < data[k].Length; i++)
                    if (!excList.Contains(data[k][i]))
                        throw new Exception("錯誤：Matrix類別建構式傳入字串參數不符合格式。內含負號、空格、數字以外之字元。");

            m = new List<List<double>>();
            for (int i = 0; i < myrow; i++)
            {
                string[] nums = data[i].Split(' ');
                List<double> rowvec = new List<double>();
                foreach (string num in nums)
                {
                    if (num == "") continue;
                    rowvec.Add(double.Parse(num));
                }

                if (mycolunm == -1) mycolunm = rowvec.Count;
                else if (rowvec.Count != mycolunm)
                    throw new Exception("錯誤：Matrix類別建構式傳入字串參數不符合格式。內含兩列行數不等的列向量。");

                m.Add(rowvec);
            }
        }
        public Matrix(Matrix mat)
        {
            m = new List<List<double>>();

            for (int i = 0; i < mat.row; i++)
            {
                m.Add(new List<double>());
                for (int j = 0; j < mat.column; j++)
                    m[i].Add(mat[i, j]);
            }
        }

        #region 存取介面

        public int row
        {
            get
            {
                return m.Count;
            }
        }
        public int column
        {
            get
            {
                if (row == 0) return 0;
                return m[0].Count;
            }
        }
        
        public int Rank
        {
            get
            {
                //列消去函數內會更新rank值
                rowReduce(this);
                return rank;
            }
        }
        
        public double Determinant
        {
            get
            {
                if (row != column)
                    throw new Exception("錯誤：Matrix類別非方形矩陣試圖要求矩陣值。");
                else if (row <= 0)
                    throw new Exception("錯誤：Matrix類別空矩陣試圖要求矩陣值。");

                if (row == 1)
                    return determinant = m[0][0];
                else if (row == 2)
                    return determinant = m[0][0] * m[1][1] - m[1][0] * m[0][1];
                else//n>2
                {
                    //列消去函數內會更新determinant
                    rowReduce(this);
                    return determinant;
                }
            }
        }

        public Matrix Inverse
        {
            get
            {
                if (row != column)
                    throw new Exception("錯誤：Matrix類別非方形矩陣試圖要求反矩陣。");
                else if (row <= 0)
                    throw new Exception("錯誤：Matrix類別空矩陣試圖要求反矩陣。");

                Matrix src = new Matrix(this);
                Matrix des = Identity(src.row);
                determinant = 1;

                //消去下三角
                for (int i = 0; i < src.row - 1; i++)
                {
                    //但若i列i行值為0，往下找i行為非0值的列做交換
                    if (src[i, i] == 0)
                    {
                        bool b = false;//判斷是否找到非0的列

                        //找i行非0的列
                        for (int j = i + 1; j < src.row; j++)
                        {
                            if (src[j, i] != 0)
                            {//找到做列交換
                                List<double> temp = src[i];
                                src[i] = src[j];
                                src[j] = temp;

                                //B向量做列交換
                                List<double> dtemp = des[i];
                                des[i] = des[j];
                                des[j] = dtemp;

                                determinant *= -1;
                                b = true;
                                break;
                            }
                            else if (j == src.row - 1)//沒找到則此行不須消去
                                b = false;
                        }
                        if (!b) continue;
                    }

                    //i列i行值非0，每列做消去
                    for (int j = i + 1; j < src.row; j++)
                    {
                        double scale = -src[j, i] / src[i, i];
                        for (int k = 0; k < src.column; k++)
                        {
                            src[j, k] = src[j, k] + src[i, k] * scale;
                            des[j, k] = des[j, k] + des[i, k] * scale;
                        }
                    }
                }
                
                //計算矩陣值
                for (int i = 0; i < src.row; i++)
                    determinant *= src[i, i];
                //矩陣值為0則不可逆
                if (determinant == 0)
                    throw new Exception("錯誤：Matrix類別不可逆矩陣試求反矩陣。");

                //消去上三角
                for (int i = src.row - 1; i >= 1; i--)
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        double scale = -src[j, i] / src[i, i];
                        src[j, i] = src[j, i] + src[i, i] * scale;
                        for (int k = 0; k < des.column; k++)
                            des[j, k] = des[j, k] + des[i, k] * scale;
                    }
                }

                //將對角線正規為1
                for (int i = 0; i < des.row; i++)
                {
                    double scale = 1 / src[i, i];
                    src[i, i] = src[i, i] / src[i, i];
                    for (int k = 0; k < des.column; k++)
                        des[i, k] = des[i, k] * scale;
                }

                return des;
            }
        }

        public Matrix Adjoint
        {
            get
            {
                if (row != column)
                    throw new Exception("錯誤：Matrix類別非方形矩陣試圖要求Adjoint。");
                else if (row <= 0)
                    throw new Exception("錯誤：Matrix類別空矩陣試圖要求Adjoint。");

                //公式：A.Inverse = (1 / A.Determinant) * A.Adjoint
                //反求Adjoint = Inverse * Determinant
                return this.Inverse * this.determinant;
            }
        }

        #endregion

        #region 運算子

        public List<double> this[int index]
        {
            get
            {
                if (index >= row)
                    throw new Exception("錯誤：Matrix類別索引超出維度。");
                return m[index];
            }
            set
            {
                if (index >= row)
                    throw new Exception("錯誤：Matrix類別索引超出維度。");
                m[index] = value;
            }
        }

        public double this[int index1, int index2]
        {
            get
            {
                if (index1 >= row || index2 >= column)
                    throw new Exception("錯誤：Matrix類別索引超出維度。");
                return m[index1][index2];
            }
            set
            {
                if (index1 >= row || index2 >= column)
                    throw new Exception("錯誤：Matrix類別索引超出維度。");
                m[index1][index2] = value;
            }
        }

        public static string operator +(string str, Matrix mat)
        {
            return str + mat.ToString();
        }

        public static Matrix operator +(Matrix mat1, Matrix mat2)
        {
            if (mat1.row != mat2.row || mat1.column != mat2.column)
                throw new Exception("錯誤：Matrix類別加法運算傳入參數維度不相等。");

            Matrix opt = new Matrix(mat1);
            for (int i = 0; i < mat1.row; i++)
                for (int j = 0; j < mat1.column; j++)
                    opt[i, j] += mat2[i, j];
            return opt;
        }

        public static Matrix operator -(Matrix mat1, Matrix mat2)
        {
            if (mat1.row != mat2.row || mat1.column != mat2.column)
                throw new Exception("錯誤：Matrix類別減法運算傳入參數維度不相等。");

            Matrix opt = new Matrix(mat1);
            for (int i = 0; i < mat1.row; i++)
                for (int j = 0; j < mat1.column; j++)
                    opt[i, j] -= mat2[i, j];
            return opt;
        }

        public static Matrix operator *(Matrix mat1, Matrix mat2)
        {
            if (mat1.column != mat2.row) 
                throw new Exception("錯誤：Matrix類別矩陣相乘運算傳入參數維度不相容。");

            Matrix opt = new Matrix(mat1.row, mat2.column);
            for (int i = 0; i < mat1.row; i++)
                for (int j = 0; j < mat2.column; j++)
                    for (int k = 0; k < mat1.column; k++)
                        opt[i, j] += mat1[i, k] * mat2[k, j];

            return opt;
        }

        public static Matrix operator *(double d, Matrix mat)
        {
            Matrix opt = new Matrix(mat);
            for (int i = 0; i < opt.row; i++)
                for (int j = 0; j < opt.column; j++)
                    opt[i, j] *= d;
            return opt;
        }

        public static Matrix operator *(Matrix mat, double d)
        {
            Matrix opt = new Matrix(mat);
            for (int i = 0; i < opt.row; i++)
                for (int j = 0; j < opt.column; j++)
                    opt[i, j] *= d;
            return opt;
        }

        public static bool operator ==(Matrix mat1, Matrix mat2)
        {
            mat1 = mat1 ?? new Matrix();
            mat2 = mat2 ?? new Matrix();

            if (mat1.row != mat2.row || mat1.column != mat2.column)
                return false;

            for (int i = 0; i < mat1.row; i++)
                for (int j = 0; j < mat1.column; j++)
                    if (mat1[i, j] != mat2[i, j])
                        return false;
            return true;
        }

        public static bool operator !=(Matrix mat1, Matrix mat2)
        {
            mat1 = mat1 ?? new Matrix();
            mat2 = mat2 ?? new Matrix();

            if (mat1.row != mat2.row || mat1.column != mat2.column)
                return true;

            for (int i = 0; i < mat1.row; i++)
                for (int j = 0; j < mat1.column; j++)
                    if (mat1[i, j] != mat2[i, j])
                        return true;
            return false;
        }

        #endregion

        #region 函數

        //單位矩陣
        public static Matrix Identity(int dimention)
        {
            Matrix opt = new Matrix(dimention, dimention);
            for (int i = 0; i < dimention; i++)
                opt[i, i] = 1;
            return opt;
        }

        /// <summary>
        /// 傳回一個矩陣轉置後的結果。
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix Transposed(Matrix mat)
        {
            Matrix opt = new Matrix(mat.column, mat.row);

            for (int i = 0; i < opt.row; i++)
                for (int j = 0; j < opt.column; j++)
                    opt[i, j] = mat[j, i];
            return opt;
        }

        /// <summary>
        /// 將傳入矩陣做列消去（下三角消去）後傳回。
        /// </summary>
        /// <param name="src">欲做列消去的矩陣。</param>
        /// <returns>消去後的矩陣。</returns>
        public static Matrix rowReduce(Matrix src)
        {
            Matrix des = new Matrix(src);
            int exchanges = 0;

            //rank = 非0列的數量
            src.rank = Math.Min(src.row, src.column);

            for (int i = 0, offset = 0; i < des.row && i + offset < des.column; i++)
            {
                if (Form1.isZero(des[i, i + offset]))
                {
                    bool findrow = false;
                    //找i行非0的列
                    for (int j = i + 1; j < des.row; j++)
                    {
                        if (!Form1.isZero(des[j, i + offset]))
                        {//找到做列交換
                            List<double> temp = des[i];
                            des[i] = des[j];
                            des[j] = temp;

                            exchanges++;
                            findrow = true;
                            break;
                        }
                    }

                    if (!findrow)
                    {
                        offset++;
                        src.rank--;
                        i--;
                        continue;
                    }
                }

                //i列i行值非0，每列做消去
                for (int j = i + 1; j < des.row; j++)
                {
                    //double scale = -des[j, i + offset] / des[i, i + offset];
                    for (int k = 0; k < des.column; k++)
                        if (k != i + offset)
                            des[j, k] += des[i, k] * -des[j, i + offset] / des[i, i + offset];
                    des[j, i + offset] = 0;
                }
            }


            //update determinant
            if (src.row == src.column && src.row > 0)
            {
                src.determinant = (exchanges % 2 == 1 ? -1 : 1);//列交換每次會乘上-1
                for (int i = 0; i < src.row; i++)
                    src.determinant *= des[i, i];//上三角形式矩陣值等於對角線相乘
            }

            return des;
        }
        
        /// <summary>
        /// 解線性系統Ax = B。
        /// </summary>
        /// <param name="A">方程組。</param>
        /// <param name="B">常數。</param>
        /// <returns></returns>
        public static List<Vector> SolveLS(Matrix A, Vector B)
        {
            if (A.row != B.dimention)
                throw new Exception("錯誤：Matrix類別解線性系統函數傳入參數不相容。方程式數目與常數數目不相等。");

            int n = A.column;

            #region 開始做列消去（此運算為消去下三角）  ***** B 向量一起做消去

            Matrix reduced = new Matrix(A);
            Vector Breduced = new Vector(B);

            //從第i列開始，以下的每列做消去
            for (int i = 0, offset = 0; i < reduced.row - 1 && i + offset < reduced.column; i++)
            {
                //但若i列i行值為0，往下找i行為非0值的列做交換
                if (Form1.isZero(reduced[i, i + offset]))
                {
                    bool findrow = false;//判斷是否找到非0的列

                    //找i行非0的列
                    for (int j = i + 1; j < reduced.row; j++)
                    {
                        if (!Form1.isZero(reduced[j, i + offset]))
                        {//找到做列交換
                            List<double> temp = reduced[i];
                            reduced[i] = reduced[j];
                            reduced[j] = temp;

                            //B向量做列交換
                            double dtemp = Breduced[i];
                            Breduced[i] = Breduced[j];
                            Breduced[j] = dtemp;

                            findrow = true;
                            break;
                        }
                    }
                    if (!findrow)
                    {
                        offset++;
                        i--;
                        continue;
                    }
                }

                //i列i行值非0，每列做消去
                for (int j = i + 1; j < reduced.row; j++)
                {
                    //double scale = -reduced[j, i] / reduced[i, i];
                    for (int k = 0; k < reduced.column; k++)
                        if (k != i + offset)
                            reduced[j, k] += reduced[i, k] * -reduced[j, i + offset] / reduced[i, i + offset];
                    Breduced[j] += Breduced[i] * -reduced[j, i + offset] / reduced[i, i + offset];
                    reduced[j, i + offset] = 0;
                }
            }

            #endregion

            //*****取得Rank(A) and Rank(A|B)
            int rankA = Math.Min(reduced.row, reduced.column), rankAB = Math.Min(reduced.row, reduced.column + 1);
            for (int i = reduced.row - 1; i >= 0; i--)
                if (new Vector(reduced[i]) == Vector.zero(reduced.column))
                {
                    rankA--;
                    if (Breduced[i] == 0)
                        rankAB--;
                }


            if (B == Vector.zero(B.dimention) && rankA == n)//Ax=0 && rankA == n  唯一0解
                    return new List<Vector>() { Vector.zero(A.column) };
            else if (B == Vector.zero(B.dimention) || (rankA == rankAB && rankA != n)) //參數解
            {
                List<Vector> opt = new List<Vector>();
                List<int> known = new List<int>();
                for (int i = 0; i < n - rankA + 1; i++) opt.Add(new Vector(n));
                for (int i = 0; i < n; i++) known.Add(0);//0=未知  1=已知  2=複合參數已知  1XX=參數XX 

                int xn = reduced.column - 1;
                for (int i = reduced.row - 1; i >= 0; i--)
                {
                    int numOfx = reduced.FirstNonzero(reduced[i]);
                    numOfx = numOfx == -1 ? 0 : reduced.column - numOfx;
                    if (numOfx == 0) continue;
                    else if (numOfx == 1)
                    {
                        opt[opt.Count - 1][xn] = Breduced[i] / reduced[i][xn];
                        known[xn] = 1;
                        xn--;
                    }
                    else
                    {
                        double consta = Breduced[i];//constant item
                        for (int k = reduced.column - 1, j = 1; j < numOfx; k--, j++)
                        {
                            if (known[k] == 1) consta += -reduced[i][k] * opt[opt.Count - 1][k];
                            else if (known[k] == 2)
                            {
                                consta += -reduced[i][k] * opt[opt.Count - 1][k];
                                for (int fori = 0; fori < opt.Count - 1; fori++)
                                    if (!Form1.isZero(opt[fori][k]))
                                        opt[fori][xn] += opt[fori][k] * reduced[i][k];
                            }
                            else if (known[k] > 100) 
                            {
                                opt[known[k] - 101][xn] =
                                       -reduced[i][k] / reduced[i][xn];
                            }
                            else if (known[k] == 0)
                            {
                                known[k] = 100 + known.Count(x => x > 100) + 1;
                                opt[known[k] - 101][k] = 1;

                                opt[known[k] - 101][reduced.column - numOfx] =
                                       -reduced[i][k] / reduced[i][reduced.column - numOfx];
                                xn--;
                            }
                        }
                        opt[opt.Count - 1][xn] = consta / reduced[i][xn];
                        known[xn] = 2;
                        xn--;
                    }
                }
                return opt;
            }
            else//Ax=B
            {
                if (rankA < rankAB)//無解
                    return new List<Vector>();
                else if (rankA == rankAB)//consistance
                {//unique solution
                    Vector X = new Vector(n);

                    //做上三角消去
                    for (int i = reduced.row - 1; i >= 1; i--)
                    {
                        if (i >= n) continue;
                        for (int j = i - 1; j >= 0; j--) 
                        {
                            double scale = -reduced[j, i] / reduced[i, i];
                            reduced[j, i] = reduced[j, i] + reduced[i, i] * scale;
                            Breduced[j] = Breduced[j] + Breduced[i] * scale;
                        }
                    }
                    //對角線正規為1後便得解
                    for (int i = 0; i < n; i++)
                        X[i] = Breduced[i] / reduced[i, i];
                    return new List<Vector>() { X };
                }
                return new List<Vector>();//default return value
            }
        }
                
        public static void Eigen(Matrix A, ref List<double> evalue, ref List<Vector> evector)
        {
            if (A.row != A.column)
                throw new Exception("錯誤：Matrix類別非方形矩陣試圖要求特徵向量。");
            else if (A.row <= 0)
                throw new Exception("錯誤：Matrix類別空矩陣試圖要求特徵向量。");
            else if (A.row != 2 && A.row != 3)
                throw new Exception("錯誤：Matrix類別求特徵向量函數參數不相容。不接受的維度：" + A.row + "。");

            evalue.Clear();
            evector.Clear();
            if (A.row == 2)
            {
                double a = 1;//coef of x^2
                double b = -(A[0, 0] + A[1, 1]);//coef of x^1
                double c = A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0];//coef of const

                if (b * b - 4 * a * c < 0)
                    throw new Exception("錯誤：Matrix類別求特徵向量函數傳回無法表示的複數。");
                else if (b * b - 4 * a * c == 0)
                    evalue.Add((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                else
                {
                    evalue.Add((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                    evalue.Add((-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                }
                
                for (int i = 0; i < evalue.Count; i++)
                {
                    Matrix t = new Matrix(A);
                    t -= evalue[i] * Identity(2);
                    t = rowReduce(t);

                    Vector Kvec = new Vector(2);
                    if (new Vector(t[0]) != Vector.zero(2))
                    {
                        if (t[0, 0] != 0 && t[0, 1] != 0)
                        {
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                            Kvec[0] = (-t[0, 1] * Kvec[1]) / t[0, 0];
                        }
                        else if (t[0, 1] != 0)
                        {
                            Kvec[0] = (t[0, 0] <= 0 ? 1 : -1);
                            Kvec[1] = 0;
                        }
                        else
                        {
                            Kvec[0] = 0;
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                        }
                    }
                    else
                    {
                        Kvec[0] = 1;
                        Kvec[1] = 1;
                    }
                    Kvec.Normalize();
                    evector.Add(Kvec);
                }
            }
            else//3*3
            {
                double a = -(A[0, 0] + A[1, 1] + A[2, 2]);//coef of x^2
                double b = A[0, 0] * A[1, 1] + A[1, 1] * A[2, 2] + A[0, 0] * A[2, 2]
                    - A[1, 0] * A[0, 1] - A[1, 2] * A[2, 1] - A[0, 2] * A[2, 0];//coef of x^1
                double c = A[1, 0] * A[0, 1] * A[2, 2] + A[0, 0] * A[1, 2] * A[2, 1] + A[0, 2] * A[1, 1] * A[2, 0]
                    - A[0, 0] * A[1, 1] * A[2, 2] - A[0, 2] * A[1, 0] * A[2, 1] - A[0, 1] * A[1, 2] * A[2, 0];
                //coef of const

                double Q = (a * a - 3 * b) / 9;
                double R = (2 * a * a * a - 9 * a * b + 27 * c) / 54;
                double algeK = 36 * a * b - 8 * a * a * a - 108 * c;//代數K
                double delta = algeK * algeK + Math.Pow(12 * b - 4 * a * a, 3);

                //if (R * R >= Q * Q * Q)
                //throw new Exception("錯誤：Matrix類別求特徵向量函數傳回無法表示的複數。");

                if (delta < 0)//3相異實根
                {
                    double theda = Math.Acos(R / Math.Sqrt(Q * Q * Q));
                    evalue.Add(-2 * Math.Sqrt(Q) * Math.Cos(theda / 3) - a / 3);
                    evalue.Add(-2 * Math.Sqrt(Q) * Math.Cos((theda + 2 * Math.PI) / 3) - a / 3);
                    evalue.Add(-2 * Math.Sqrt(Q) * Math.Cos((theda - 2 * Math.PI) / 3) - a / 3);
                }
                else if (Form1.isZero(delta))
                {
                    if (Form1.isZero(algeK))//3重實根
                        evalue.Add(-a / 3);
                    else//2重實根+另1實根
                    {
                        evalue.Add((-a + Math.Pow(algeK, 1.0 / 3.0)) / 3);
                        evalue.Add(-a / 3 - Math.Pow(algeK, 1.0 / 3.0) / 6);
                    }
                }
                else//1實根，忽略另2個虛根
                    evalue.Add((-a * 2 + Math.Pow(algeK + Math.Sqrt(delta), 1.0 / 3.0)
                        + Math.Pow(algeK - Math.Sqrt(delta), 1.0 / 3.0)) / 6.0);

                for (int i = 0; i < evalue.Count; i++)
                {
                    Matrix t = new Matrix(A);
                    t -= evalue[i] * Identity(3);

                    t = rowReduce(t);
                    Vector Kvec = new Vector(3);
                    if(new Vector(t[0]) != Vector.zero(3) && new Vector(t[1]) != Vector.zero(3))//1、2->非0列，參數為1個
                    {
                        if (t[1, 1] != 0 && t[1, 2] != 0)
                        {
                            Kvec[2] = (t[1, 2] <= 0 ? 1 : -1);
                            Kvec[1] = -t[1, 2] * Kvec[2] / t[1, 1];
                            Kvec[0] = (-t[0, 2] * Kvec[2] - t[0, 1] * Kvec[1]) / t[0, 0];
                        }
                        else if (t[1, 1] != 0) 
                        {
                            Kvec[1] = 0;
                            Kvec[2] = (t[1, 2] <= 0 ? 1 : -1);
                            Kvec[0] = (-t[0, 2] * Kvec[2]) / t[0, 0];
                        }
                        else
                        {
                            Kvec[1] = (t[1, 1] <= 0 ? 1 : -1);
                            Kvec[2] = 0;
                            Kvec[0] = (-t[0, 1] * Kvec[1]) / t[0, 0];
                        }
                    }
                    else if(new Vector(t[0]) != Vector.zero(3))//1->非0列  2->0列，參數為2個
                    {
                        if (t[0, 0] != 0 && t[0, 1] != 0 && t[0, 2] != 0)
                        {
                            Kvec[2] = (t[0, 2] <= 0 ? 1 : -1);
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                            Kvec[0] = (-t[0, 2] * Kvec[2] - t[0, 1] * Kvec[1]) / t[0, 0];
                        }
                        else if (t[0, 0] == 0 && t[0, 1] != 0 && t[0, 2] != 0)
                        {
                            Kvec[0] = (t[0, 0] <= 0 ? 1 : -1);
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                            Kvec[2] = (-t[0, 0] * Kvec[0] - t[0, 1] * Kvec[1]) / t[0, 2];
                        }
                        else if (t[0, 0] != 0 && t[0, 1] == 0 && t[0, 2] != 0)
                        {
                            Kvec[0] = (t[0, 0] <= 0 ? 1 : -1);
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                            Kvec[2] = (-t[0, 0] * Kvec[0] - t[0, 1] * Kvec[1]) / t[0, 2];
                        }
                        else if (t[0, 0] != 0 && t[0, 1] != 0 && t[0, 2] == 0)
                        {
                            Kvec[0] = (t[0, 0] <= 0 ? 1 : -1);
                            Kvec[2] = (t[0, 2] <= 0 ? 1 : -1);
                            Kvec[1] = (-t[0, 0] * Kvec[0] - t[0, 2] * Kvec[2]) / t[0, 1];
                        }
                        else if (t[0, 0] != 0 && t[0, 1] == 0 && t[0, 2] == 0)
                        {
                            Kvec[0] = 0;
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                            Kvec[2] = (t[0, 2] <= 0 ? 1 : -1);
                        }
                        else if (t[0, 0] == 0 && t[0, 1] == 0 && t[0, 2] != 0)
                        {
                            Kvec[0] = (t[0, 0] <= 0 ? 1 : -1);
                            Kvec[1] = (t[0, 1] <= 0 ? 1 : -1);
                            Kvec[2] = 0;
                        }
                        else if (t[0, 0] == 0 && t[0, 1] != 0 && t[0, 2] == 0)
                        {
                            Kvec[0] = (t[0, 0] <= 0 ? 1 : -1);
                            Kvec[1] = 0;
                            Kvec[2] = (t[0, 2] <= 0 ? 1 : -1);
                        }
                    }
                    else//0矩陣，3參數
                    {
                        Kvec[0] = 1;
                        Kvec[1] = 1;
                        Kvec[2] = 1;
                    }

                    Kvec.Normalize();
                    evector.Add(Kvec);
                }
            }
        }

        public static void PowerMethod(Matrix A, ref List<double> evalue, ref List<Vector> evector)
        {
            if (A.row != A.column)
                throw new Exception("錯誤：Matrix類別非方形矩陣試圖要求特徵向量。");
            else if (A.row <= 0)
                throw new Exception("錯誤：Matrix類別空矩陣試圖要求特徵向量。");

            evalue.Clear();
            evector.Clear();

            Matrix src = A;
            for (int i = 0; i < A.row; i++)
            {
                Vector Vm1, Vm = new Vector(src.row);
                for (int j = 0; j < Vm.dimention; j++) Vm[j] = 1;
                Matrix Xm = Transposed(new Matrix(new List<Vector>() { Vm }));

                int avoidloop = 0;
                for (avoidloop = 0; avoidloop < 100000; avoidloop++)
                {
                    Vm1 = Vm;
                    Xm = src * Xm;
                    Vm = new Vector(Transposed(Xm)[0]);

                    Xm = Xm * (1 / Vm.ToList().Max(d => Math.Abs(d)));
                    Vm = new Vector(Transposed(Xm)[0]);

                    if (Vm[0] * Vm1[0] < 0) Vm1 = Vm1 * -1;
                    if ((Vm - Vm1).magnitude < Math.Pow(10, -10))
                        break;
                }
                evalue.Add(((new Vector(Transposed(src * Xm)[0])) * Vm) / (Vm * Vm));
                evector.Add(Vm);

                src = src - (evalue[evalue.Count - 1] / Math.Pow(Vm.magnitude, 2)) * (Xm * Transposed(Xm));
            }
            for (int i = 0; i < evector.Count; i++)
                evector[i].Normalize();
        }

        public static Vector LeastSquare(Matrix A, Vector B)
        {
            if (A.row != B.dimention)
                throw new Exception("錯誤：Matrix類別最小平方差函數傳入參數不相容。方程式數目與常數數目不相等。");

            //公式：X = (AT * A).Inverse * AT * B
            Matrix AT = Transposed(A);
            Matrix Bcol = Transposed(new Matrix(new List<List<double>>() { B.ToList() }));
            return new Vector(Transposed((AT * A).Inverse * AT * Bcol)[0]);
        }

        public List<List<double>> ToList()
        {
            return m;
        }

        public void Transpose()
        {
            Matrix temp = new Matrix(column, row);

            for (int i = 0; i < temp.row; i++)
                for (int j = 0; j < temp.column; j++)
                    temp[i, j] = this[j, i];
            m = temp.m;
        }

        public override string ToString()
        {
            string str = "";
            str += "Matrix { name: " + name + "  row: " + row + " column: " + column + " | \r\n";
            for (int i = 0; i < row; i++)
            {
                str += (i == 0 ? "" : ", \r\n");
                for (int j = 0; j < column; j++)
                    str += (j == 0 ? "" : ", ") + m[i][j];
            }
            str += " }";
            return str;
        }

        public string ToSimpleString()
        {
            string str = "";
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                    //str += (j == 0 ? "" : " ") + m[i][j];
                    str += ("" + Math.Round(m[i][j], 4)).PadLeft(15);

                if (i == row - 1) break;
                str += "\r\n";
            }
            return str;
        }

        /// <summary>
        /// 回傳該列中第一個非0數值的索引，若此列為0列則回傳-1。
        /// </summary>
        /// <param name="trow"></param>
        /// <returns></returns>
        private int FirstNonzero(List<double> trow)
        {
            for (int i = 0; i < trow.Count; i++)
                if (Math.Abs(trow[i]) > Math.Pow(10, -10))
                    return i;
            return -1;
        }

        #endregion
    }

    class VarManager
    {
        private List<Vector> vlst;
        private List<Matrix> mlst;
        public ListBox lbx;

        public int vecCount
        {
            get
            {
                return vlst.Count;
            }
        }
        public int matCount
        {
            get
            {
                return mlst.Count;
            }
        }

        public VarManager(ref ListBox _lbx)
        {
            vlst = new List<Vector>();
            mlst = new List<Matrix>();
            lbx = _lbx;
        }
        
        public bool Find(string name, ref Vector vec)
        {
            Vector opt = vlst.Find(x => x.name == name) ?? new Vector();
            if (opt == new Vector())
                throw new Exception("var doesnt exist");
            else
                vec = opt;
            return true;
        }

        public bool Find(string name, ref Matrix mat)
        {
            Matrix opt = mlst.Find(x => x.name == name) ?? new Matrix();
            if (opt == new Matrix())
                throw new Exception("var doesnt exist");
            else
                mat = opt;
            return true;
        }

        public string DataText(int index)
        {
            if (index >= vlst.Count + mlst.Count || index < 0)
                throw new Exception("錯誤－VarManager類別要求資料文字索引超出陣列。");

            if(index >= vlst.Count)
                return "" + mlst[index - vlst.Count].ToSimpleString();
            else
                return "" + vlst[index].ToSimpleString();
        }

        public string DataName(int index)
        {
            if (index >= vlst.Count + mlst.Count || index < 0)
                throw new Exception("錯誤－VarManager類別要求資料名稱索引超出陣列。");

            if (index >= vlst.Count)
                return mlst[index - vlst.Count].name;
            else
                return vlst[index].name;
        }

        public void ReadFile(string path)
        {
            List<string> ipt = File.ReadAllLines(path).ToList();

            int step = 0;
            bool VorM = true;//T for Vector, F for Matrix
            Vector vtmp = new Vector();
            Matrix mtmp = new Matrix();
            List<string> formtmp = new List<string>();

            for (int i = 1; i < ipt.Count; i++)
            {
                if(i == 24)
                {
                    int x = 0;
                }

                if (step == 0)
                {
                    step++;
                    VorM = (ipt[i] == "V");
                }
                else if (step == 1)
                {
                    step++;
                    if (VorM)
                        vtmp = new Vector(int.Parse(ipt[i]));
                    else
                    {
                        string[] dimen = ipt[i].Split(' ');
                        mtmp = new Matrix(int.Parse(dimen[0]), int.Parse(dimen[1]));
                    }
                }
                else
                {
                    if (VorM)
                    {
                        ipt[i] = ipt[i].Replace('\t', ' ');
                        vtmp = new Vector(ipt[i]);
                        vtmp.name = "v" + vlst.Count;
                        vlst.Add(vtmp);

                        step = 0;
                    }
                    else
                    {
                        ipt[i] = ipt[i].Replace('\t', ' ');
                        formtmp.Add(ipt[i]);
                        if (formtmp.Count == mtmp.row)
                        {
                            mtmp = new Matrix(formtmp);
                            mtmp.name = "m" + mlst.Count;
                            mlst.Add(mtmp);


                            formtmp.Clear();
                            step = 0;
                        }
                    }
                }
            }

            UpdateView();
        }

        public void Clear()
        {
            vlst.Clear();
            mlst.Clear();
            UpdateView();
        }

        private void UpdateView()
        {
            lbx.Items.Clear();
            for (int i = 0; i < vlst.Count; i++)
                lbx.Items.Add("$" + vlst[i].name + " " + vlst[i]);
            for (int i = 0; i < mlst.Count; i++)
                lbx.Items.Add("$" + mlst[i].name + " " + mlst[i]);
        }
    }
}
