using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proj2new
{
    class Optimization
    {
        public static TextBox layout;
        public static int MaxIteratCount = 200;

        public static void Powell(Formula fm, Vector iniGuess, Vector intervalX, Vector intervalY)
        {
            if (fm.ftype == Formula.FormulaType.Const)
            {
                layout.Text += "常數函數執行最佳化！\r\n\r\n";
                return;
            }

            Vector x, prex;
            bool SingleOrTwo = fm.ftype == Formula.FormulaType.TwoVar;///true for two var
            if (SingleOrTwo)
            {
                x = new Vector(iniGuess[0], iniGuess[1]);
                prex = new Vector(iniGuess[0], iniGuess[1]);
            }
            else
            {
                x = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                prex = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
            }

            Vector d = new Vector(1.0);
            List<Vector> dset = new List<Vector>() { new Vector(1, 0), new Vector(0, 1) };
            int k = 1;
            do
            {
                layout.Text += "k = " + k + "\r\n";
                prex = x;

                for (int i = 0; i < (SingleOrTwo ? 2 : 1); i++)
                {
                    layout.Text += "j = " + (i + 1) + "\r\n";
                    layout.Text += "X" + (i + 1) + " = " + x.ToSimpleString() + "\r\n";

                    double alpha = Numerical.goldenSectionSearch(fm, x,
                        SingleOrTwo ? dset[i] : d, intervalX, intervalY);
                    x = x + alpha * (SingleOrTwo ? dset[i] : d);

                    layout.Text += "X" + (i + 2) + " = " + x.ToSimpleString() + "\r\n\r\n";
                }

                if (SingleOrTwo)
                {
                    dset[0] = dset[1];
                    dset[1] = x - prex;
                }

                double alpha3 = Numerical.goldenSectionSearch(fm, x
                    , SingleOrTwo ? dset[dset.Count-1] : d, intervalX, intervalY);
                x = x + alpha3 * (SingleOrTwo ? dset[dset.Count - 1] : d);
                layout.Text += "alpha = " + alpha3 + "\r\n" +
                    "S = " + (SingleOrTwo ? dset[dset.Count - 1] : d).ToSimpleString() + "\r\n" +
                    "X = " + x.ToSimpleString() + "\r\n\r\n";

                if (Numerical.Delta(fm[x], fm[prex], 0.00000001) ||
                    Numerical.IsZero((prex - x) * (prex - x)) ||
                    dset[dset.Count - 1] == Vector.zero(2) || d == Vector.zero(1))
                    break;
            } while (++k < MaxIteratCount);

            layout.Text += "[x] = " + x.ToSimpleString() + "\r\n" +
                "min: " + fm[x] + "\r\n\r\n********************\r\n\r\n";
        }

        public static void SteepDescent(Formula fm, Vector iniGuess, Vector intervalX, Vector intervalY)
        {
            if (fm.ftype == Formula.FormulaType.Const)
            {
                layout.Text += "常數函數執行最佳化！\r\n\r\n";
                return;
            }


            Vector x, prex, d;
            bool SingleOrTwo = fm.ftype == Formula.FormulaType.TwoVar;///true for two var
            if (SingleOrTwo)
            {
                x = new Vector(iniGuess[0], iniGuess[1]);
                prex = new Vector(iniGuess[0], iniGuess[1]);
                d = new Vector(1, 0);
            }
            else
            {
                x = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                prex = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                d = new Vector(1.0);
            }
            
            int k = 1;
            do
            {
                layout.Text += "k = " + k + "\r\n";

                prex = x;

                if (SingleOrTwo) d = fm.Gradient(x) * -1;
                else if (fm.ftype == Formula.FormulaType.Single_X) d[0] = -fm.GradientX[x];
                else d[0] = -fm.GradientY[x];

                double alpha1 = Numerical.goldenSectionSearch(fm, x, d, intervalX, intervalY);
                x = x + alpha1 * d;

                layout.Text += "h = " + d.ToSimpleString() + "\r\n" +
                    "lambda = " + alpha1 + "\r\n" +
                    "X = " + x.ToSimpleString() + "\r\n\r\n";


                if (Numerical.IsZero(fm[x] - fm[prex]) ||
                    Numerical.IsZero((prex - x) * (prex - x)))
                    break;
            } while (++k < MaxIteratCount);

            layout.Text += "[x] = " + x.ToSimpleString() + "\r\n" +
                "min: " + fm[x] + "\r\n\r\n********************\r\n\r\n";
        }

        public static void Conjugate(Formula fm, Vector iniGuess, Vector intervalX, Vector intervalY)
        {
            if (fm.ftype == Formula.FormulaType.Const)
            {
                layout.Text += "常數函數執行最佳化！\r\n\r\n";
                return;
            }


            Vector x, prex, d;
            bool SingleOrTwo = fm.ftype == Formula.FormulaType.TwoVar;///true for two var
            if (SingleOrTwo)
            {
                x = new Vector(iniGuess[0], iniGuess[1]);
                prex = new Vector(iniGuess[0], iniGuess[1]);
                d = new Vector(1, 0);
            }
            else
            {
                x = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                prex = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                d = new Vector(1.0);
            }

            int k = 1;
            do
            {
                layout.Text += "k = " + k + "\r\n";

                Vector gk = SingleOrTwo ? fm.Gradient(x) : new Vector(fm.ftype == Formula.FormulaType.Single_X ?
                    fm.GradientX[x] : fm.GradientY[x]);
                if (k == 1)
                    d = gk * -1;
                else
                {
                    Vector gkm1 = SingleOrTwo ? fm.Gradient(prex) : new Vector(fm.ftype == Formula.FormulaType.Single_X ?
                        fm.GradientX[prex] : fm.GradientY[prex]);
                    double beta = (gk * gk) / (gkm1 * gkm1);
                    d = gk * -1 + beta * d;
                    layout.Text += "beta = " + beta + "\r\n";
                }

                prex = x;

                double alpha1 = Numerical.goldenSectionSearch(fm, x, d, intervalX, intervalY);
                x = x + alpha1 * d;

                layout.Text += "Si = " + d.ToSimpleString() + "\r\n" +
                    "alpha = " + alpha1 + "\r\n" +
                    "Xi = " + x.ToSimpleString() + "\r\n\r\n";


                if (Numerical.IsZero(fm[x] - fm[prex]) ||
                    Numerical.IsZero((prex - x) * (prex - x)) ||
                    Numerical.IsZero(gk * gk))
                    break;
            } while (++k < MaxIteratCount);

            layout.Text += "[x] = " + x.ToSimpleString() + "\r\n" +
                "min: " + fm[x] + "\r\n\r\n********************\r\n\r\n";
        }

        public static void Newton(Formula fm, Vector iniGuess)
        {
            if (fm.ftype == Formula.FormulaType.Const)
            {
                layout.Text += "常數函數執行最佳化！\r\n\r\n";
                return;
            }


            Vector x, prex;
            bool SingleOrTwo = fm.ftype == Formula.FormulaType.TwoVar;///true for two var
            if (SingleOrTwo)
            {
                x = new Vector(iniGuess[0], iniGuess[1]);
                prex = new Vector(iniGuess[0], iniGuess[1]);
            }
            else
            {
                x = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                prex = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
            }

            int k = 1;
            do
            {
                prex = x;

                Vector tempv;
                Matrix hessian = fm.Hessian(x);
                Matrix inverse = hessian.Inverse;
                tempv = SingleOrTwo ? Numerical.MatrixToVector(inverse * Numerical.VectorToMatrix(fm.Gradient(x)))
                    : new Vector(inverse[0, 0] * (fm.ftype == Formula.FormulaType.Single_X ? fm.GradientX[x] : fm.GradientY[x]));
                x = x + (tempv * -1);

                if (x.dimention == 1)
                {
                    try
                    {
                        double test = fm[x];
                    }
                    catch (OutOfDomainException)
                    {
                        x[0] = Math.Abs(x[0]);
                    }
                }

                layout.Text += "Hessian = \r\n" + hessian.ToSimpleString() + "\r\n" +
                    "Hessian Inverse = \r\n" + inverse.ToSimpleString() + "\r\n" +
                    "x = " + x.ToSimpleString() + "\r\n\r\n";


                if (Numerical.Delta(fm[x], fm[prex], 1E-8) ||
                    Numerical.Delta((prex - x) * (prex - x), 0, 1E-8))
                    break;
            } while (++k < (int)(MaxIteratCount * 0.4));


            layout.Text += "[x] = " + x.ToSimpleString() + "\r\n" +
                "min: " + fm[x] + "\r\n\r\n********************\r\n\r\n";
        }

        public static void QuasiNewton(Formula fm, Vector iniGuess)
        {
            if (fm.ftype == Formula.FormulaType.Const)
            {
                layout.Text += "常數函數執行最佳化！\r\n\r\n";
                return;
            }


            Vector x, prex, d;
            bool SingleOrTwo = fm.ftype == Formula.FormulaType.TwoVar;///true for two var
            if (SingleOrTwo)
            {
                x = new Vector(iniGuess[0], iniGuess[1]);
                prex = new Vector(iniGuess[0], iniGuess[1]);
                d = new Vector(1, 0);
            }
            else
            {
                x = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                prex = new Vector(iniGuess[fm.ftype == Formula.FormulaType.Single_X ? 0 : 1]);
                d = new Vector(1.0);
            }
            Matrix F = Matrix.Identity(SingleOrTwo ? 2 : 1);

            int k = 1;
            do
            {
                prex = x;

                Vector gk;
                if(SingleOrTwo)
                {
                    gk = fm.Gradient(x);
                    if (Numerical.IsZero(gk * gk)) break;
                    d[0] = new Vector(F[0]) * gk * -1;
                    d[1] = new Vector(F[1]) * gk * -1;

                }
                else
                {
                    gk = new Vector(fm.ftype == Formula.FormulaType.Single_X ? fm.GradientX[x] : fm.GradientY[x]);
                    if (Numerical.IsZero(gk * gk)) break;
                    d[0] = F[0, 0] * gk[0] * -1;
                }

                double alpha1 = Numerical.goldenSectionSearch(fm, x, d,
                    new Vector(x[0] - 10, x[0] + 10)
                    , fm.ftype == Formula.FormulaType.Single_Y ? new Vector(x[0] - 10, x[0] + 10)
                        : new Vector(x[1] - 10, x[1] + 10));
                x = x + alpha1 * d;

                layout.Text += "Hessian = \r\n" + F.ToSimpleString() + "\r\n" +
                    "X = " + x.ToSimpleString() + "\r\n\r\n";

                if (SingleOrTwo)
                {
                    Vector deltax = alpha1 * d;
                    Vector deltag = fm.Gradient(x) - gk;

                    Matrix deltax2x1 = Numerical.VectorToMatrix(deltax);
                    Matrix deltag2x1 = Numerical.VectorToMatrix(deltag);
                    Matrix Fdeltag = F * deltag2x1;

                    F = F + (1 / (deltax * deltag)) * (deltax2x1 * Matrix.Transposed(deltax2x1))
                        - (1 / (Matrix.Transposed(deltag2x1) * F * deltag2x1)[0, 0]) * (Fdeltag * Matrix.Transposed(Fdeltag));


                }
                else if (fm.ftype == Formula.FormulaType.Single_X) 
                    F[0, 0] = (alpha1 * d[0]) / (fm.GradientX[x] - gk[0]);
                else
                    F[0, 0] = (alpha1 * d[0]) / (fm.GradientY[x] - gk[0]);


                if (Numerical.Delta(fm[x], fm[prex], 0.00000001) ||
                    Numerical.IsZero((prex - x) * (prex - x)))
                    break;
            } while (++k < (int)(MaxIteratCount * 0.4));

            layout.Text += "[x] = " + x.ToSimpleString() + "\r\n" +
                "min: " + fm[x] + "\r\n\r\n********************\r\n\r\n";
        }
    }

    class Numerical
    {
        public static double zerodelta = 1E-10;
        public static bool IsZero(double value)
        {
            return Math.Abs(value) < zerodelta;
        }
        /// <summary>
        /// 判斷兩數差值是否小於給予標準。
        /// </summary>
        /// <param name="value1">參數1。</param>
        /// <param name="value2">參數2。</param>
        /// <param name="delta">差值標準。</param>
        /// <returns></returns>
        public static bool Delta(double value1, double value2, double delta)
        {
            return Math.Abs(value1 - value2) < delta;
        }


        public static Matrix VectorToMatrix(Vector v)
        {
            return Matrix.Transposed(new Matrix(new List<List<double>>() { v.ToList() }));
        }

        public static Vector MatrixToVector(Matrix m)
        {
            if (m.row > 0)
                return new Vector(Matrix.Transposed(m)[0]);
            else
                return null;
        }
        
        private static double phi = 1 / ((1 + Math.Sqrt(5)) / 2);
        public static double goldenSectionSearch(Formula fm, Vector point, Vector dir, Vector xlimit, Vector ylimit)
        {
            double l = 0, r = 0;
            if (dir.dimention == 1)
            {
                if (fm.ftype == Formula.FormulaType.Single_X)
                {
                    l = (xlimit[dir[0] > 0 ? 0 : 1] - point[0]) / dir[0];
                    r = (xlimit[dir[0] > 0 ? 1 : 0] - point[0]) / dir[0];
                }
                else
                {
                    l = (ylimit[dir[0] > 0 ? 0 : 1] - point[0]) / dir[0];
                    r = (ylimit[dir[0] > 0 ? 1 : 0] - point[0]) / dir[0];
                }
            }
            else
            {
                if (dir[0] == 0 && dir[1] == 0) return 0;
                else if (dir[0] == 0)
                {
                    l = (ylimit[dir[1] > 0 ? 0 : 1] - point[1]) / dir[1];
                    r = (ylimit[dir[1] > 0 ? 1 : 0] - point[1]) / dir[1];
                }
                else if (dir[1] == 0)
                {
                    l = (xlimit[dir[1] > 0 ? 0 : 1] - point[0]) / dir[0];
                    r = (xlimit[dir[1] > 0 ? 1 : 0] - point[0]) / dir[0];
                }
                else if (dir[0] > 0 && dir[1] > 0)
                {
                    l = Math.Max((xlimit[0] - point[0]) / dir[0], (ylimit[0] - point[1]) / dir[1]);
                    r = Math.Min((xlimit[1] - point[0]) / dir[0], (ylimit[1] - point[1]) / dir[1]);
                }
                else if (dir[0] > 0)
                {
                    l = Math.Max((xlimit[0] - point[0]) / dir[0], (ylimit[1] - point[1]) / dir[1]);
                    r = Math.Min((xlimit[1] - point[0]) / dir[0], (ylimit[0] - point[1]) / dir[1]);
                }
                else if (dir[1] > 0)
                {
                    l = Math.Max((xlimit[1] - point[0]) / dir[0], (ylimit[0] - point[1]) / dir[1]);
                    r = Math.Min((xlimit[0] - point[0]) / dir[0], (ylimit[1] - point[1]) / dir[1]);
                }
                else
                {
                    l = Math.Max((xlimit[1] - point[0]) / dir[0], (ylimit[1] - point[1]) / dir[1]);
                    r = Math.Min((xlimit[0] - point[0]) / dir[0], (ylimit[0] - point[1]) / dir[1]);
                }
            }


            for (int iter_over = 0; !Delta(l, r, 0.0000001) && iter_over < 1000; iter_over++)
            {
                double c1 = r - (r - l) * phi;
                double c2 = l + (r - l) * phi;

                double c1value = 0, c2value = 0;
                try { c1value = fm[point + c1 * dir]; }
                catch (OutOfDomainException) { c1value = double.MaxValue; }
                try { c2value = fm[point + c2 * dir]; }
                catch (OutOfDomainException) { c2value = double.MaxValue; }
                if (c1value > c2value)
                    l = c1;
                else if (c1value < c2value)
                    r = c2;
                else if (c1value == double.MaxValue)
                    return double.NaN;
                else
                    r = c2;
            }
            return r;
        }

    }

    /// <summary>
    /// 表示多項式函數中個一個項。
    /// </summary>
    class Term
    {
        public double coef;
        public double exp1;
        public double exp2;

        public Term()
        {
            coef = 0;
            exp1 = 0;
            exp2 = 0;
        }
        public Term(double _coef, double _exp1, double _exp2)
        {
            coef = _coef;
            exp1 = _exp1;
            exp2 = _exp2;
        }

        public double this[Vector varValue]
        {
            get
            {
                if (varValue.dimention == 2)
                    return coef * Math.Pow(varValue[0], exp1) * Math.Pow(varValue[1], exp2);
                else
                    return coef;
            }
        }
        public double this[params double[] varValue]
        {
            get
            {
                if (varValue.Length == 2)
                    return coef * Math.Pow(varValue[0], exp1) * Math.Pow(varValue[1], exp2);
                else
                    return coef;
            }
        }

        public override string ToString()
        {
            return "[" + coef + "  x^ " + exp1 + "  y^ " + exp2 + "]";
        }
    }

    /// <summary>
    /// 表示一個多項式函數。
    /// </summary>
    class Formula
    {
        /// <summary>
        /// Formula物件中用以表示函數變量類型的列舉。
        /// </summary>
        public enum FormulaType
        {
            /// <summary>
            /// 表示常數函數。
            /// </summary>
            Const,
            /// <summary>
            /// 表示單變數函數，X變量。
            /// </summary>
            Single_X,
            /// <summary>
            /// 表示單變數函數，Y變量。
            /// </summary>
            Single_Y,
            /// <summary>
            /// 表示雙變數函數。
            /// </summary>
            TwoVar
        };

        private List<Term> terms;
        public FormulaType ftype;
        public FormulaType Applyftype
        {
            get
            {
                Apply();
                return ftype;
            }
        }

        public Formula()
        {
            terms = new List<Term>();
        }
        public Formula(string fmlstr)
        {
            terms = new List<Term>();
            if (fmlstr != "")
            {
                //解析字串
                for (int i = 0; i < fmlstr.Length; i++)
                    if (fmlstr[i] == '-')
                        fmlstr = fmlstr.Insert(i++, "+");
                string[] terms = fmlstr.Split('+');

                for (int i = 0; i < terms.Length; i++)
                {
                    string[] content = terms[i].Split('*');
                    double c = 1, e1 = 0, e2 = 0;

                    for (int j = 0; j < content.Length; j++)
                    {
                        if (content[j][0] == 'x')
                            e1 = (content[j].Length == 1 ? 1 : double.Parse(content[j].Split('^')[1]));
                        else if (content[j][0] == '-' && content[j][1] == 'x')
                        {
                            c = -1;
                            e1 = (content[j].Length == 2 ? 1 : double.Parse(content[j].Split('^')[1]));
                        }
                        else if (content[j][0] == 'y')
                            e2 = (content[j].Length == 1 ? 1 : double.Parse(content[j].Split('^')[1]));
                        else if (content[j][0] == '-' && content[j][1] == 'y')
                        {
                            c = -1;
                            e2 = (content[j].Length == 2 ? 1 : double.Parse(content[j].Split('^')[1]));
                        }
                        else
                            c = double.Parse(content[j]);
                    }
                    this.terms.Add(new Term(c, e1, e2));
                }
            }
            Apply();
        }
        ~Formula()
        {
            terms.Clear();
        }

        /// <summary>
        /// 套用現在函數形式，並辨別函數類型。處裡單變數Y的函數與常數函數。
        /// </summary>
        public void Apply()
        {
            bool havex = false, havey = false;
            for (int i = 0; i < terms.Count && !(havex && havey); i++)
            {
                if (!havex && terms[i].exp1 != 0) havex = true;
                if (!havey && terms[i].exp2 != 0) havey = true;
            }

            ftype = (FormulaType)((havex ? 1 : 0) + (havey ? 2 : 0));            

            if (!havex && !havey)
            {
                if (terms.Count == 0)
                    terms.Add(new Term(0, 0, 0));
                for (int i = terms.Count - 1; i > 0; i = terms.Count - 1)
                {
                    terms[0].coef += terms[i].coef;
                    terms.RemoveAt(i);
                }
            }
        }

        public override string ToString()
        {
            string opt = "Formula { Terms Count: " + terms.Count + "  | \r\n";
            foreach (Term trm in terms)
                opt += trm.ToString() + "    →  ";
            opt += " }";
            return opt;
        }

        public string ToSimpleString()
        {
            string opt = "";
            foreach (Term trm in terms)
            {
                if (trm.coef == 0) continue;

                if (terms.IndexOf(trm) != 0) opt += trm.coef > 0 ? "+" : "-";
                else if (trm.coef < 0) opt += "-";
                opt += "" + trm.coef;

                if (trm.exp1 != 0)
                    opt += "*x^" + trm.exp1;
                if (trm.exp2 != 0)
                    opt += "*y^" + trm.exp2;
            }
            return opt;
        }

        public double this[Vector varValue]
        {
            get
            {
                Vector ipt = new Vector(2);
                if (varValue.dimention == 2)
                {
                    ipt[0] = varValue[0];
                    ipt[1] = varValue[1];
                }
                else if (varValue.dimention == 1 && ftype == FormulaType.Single_X)
                    ipt[0] = varValue[0];
                else if (varValue.dimention == 1 && ftype == FormulaType.Single_Y)
                    ipt[1] = varValue[0];
                else if (ftype != FormulaType.Const)
                    throw new IllegalInputException("錯誤：Formula類別輸入自變數不相容。");


                //計算式子
                double opt = 0;
                foreach (Term trm in terms)
                {
                    double reg = trm[ipt];
                    if (double.IsNaN(reg))
                        throw new OutOfDomainException("錯誤：Formula類別輸入自變數未定義。");
                    else
                        opt += reg;
                }
                return opt;
            }
        }
        public double this[params double[] varValue]
        {
            get
            {
                Vector ipt = new Vector(2);
                if (varValue.Length == 2 && ftype == FormulaType.TwoVar)
                {
                    ipt[0] = varValue[0];
                    ipt[1] = varValue[1];
                }
                else if (varValue.Length == 1 && ftype == FormulaType.Single_X)
                    ipt[0] = varValue[0];
                else if (varValue.Length == 1 && ftype == FormulaType.Single_Y)
                    ipt[1] = varValue[0];
                else if (ftype != FormulaType.Const)
                    throw new IllegalInputException("錯誤：Formula類別輸入自變數不相容。");


                //計算式子
                double opt = 0;
                foreach (Term trm in terms)
                {
                    double reg = trm[ipt[0], ipt[1]];
                    if (double.IsNaN(reg))
                        throw new OutOfDomainException("錯誤：Formula類別輸入自變數未定義。");
                    else
                        opt += reg;
                }
                return opt;
            }
        }

        public static bool operator ==(Formula fm1, Formula fm2)
        {
            fm1 = fm1 ?? new Formula();
            fm2 = fm2 ?? new Formula();

            if (fm1.terms.Count == 0 && fm2.terms.Count == 0) return true;
            else if (fm1.terms.Count > 0 && fm2.terms.Count > 0) return true;
            else return false;
        }
        public static bool operator !=(Formula fm1, Formula fm2)
        {
            fm1 = fm1 ?? new Formula();
            fm2 = fm2 ?? new Formula();

            if (fm1.terms.Count == 0 && fm2.terms.Count == 0) return false;
            else if (fm1.terms.Count > 0 && fm2.terms.Count > 0) return false;
            else return true;
        }

        public Formula GradientX
        {
            get
            {
                if (ftype == FormulaType.Const || ftype == FormulaType.Single_Y) return new Formula("0");
                else
                {
                    Formula opt = new Formula();
                    opt.terms.Clear();
                    foreach (Term trm in terms)
                        if (Numerical.IsZero(trm.coef * trm.exp1))
                            continue;
                        else
                            opt.terms.Add(new Term(trm.coef * trm.exp1, trm.exp1 - 1, trm.exp2));
                    opt.Apply();
                    return opt;
                }
            }
        }

        public Formula GradientY
        {
            get
            {
                if (ftype == FormulaType.Const || ftype == FormulaType.Single_X) return new Formula("0");
                else
                {
                    Formula opt = new Formula();
                    opt.terms.Clear();
                    foreach (Term trm in terms)
                        if (Numerical.IsZero(trm.coef * trm.exp2))
                            continue;
                        else
                            opt.terms.Add(new Term(trm.coef * trm.exp2, trm.exp1, trm.exp2 - 1));
                    opt.Apply();
                    return opt;
                }
            }
        }

        public Vector Gradient(Vector x)
        {
            return new Vector(GradientX[x], GradientY[x]);
        }

        public Matrix Hessian(Vector x)
        {
            if (ftype == FormulaType.Const) return new Matrix(1, 1);
            else if (ftype == FormulaType.Single_X)
            {
                Matrix opt = new Matrix(1, 1);
                opt[0, 0] = GradientX.GradientX[x];
                return opt;
            }
            else if (ftype == FormulaType.Single_Y)
            {
                Matrix opt = new Matrix(1, 1);
                opt[0, 0] = GradientY.GradientY[x];
                return opt;
            }
            else
            {
                Matrix opt = new Matrix(2, 2);
                opt[0, 0] = GradientX.GradientX[x];
                opt[0, 1] = GradientX.GradientY[x];
                opt[1, 0] = GradientY.GradientX[x];
                opt[1, 1] = GradientY.GradientY[x];
                return opt;
            }
        }
    }

    /// <summary>
    /// 表示一個向量。
    /// </summary>
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
            string[] nums = data.Split(' ');
            List<double> para = new List<double>();
            foreach (string num in nums)
            {
                if (num == "") continue;
                try { para.Add(double.Parse(num)); }
                catch (Exception ep)
                {
                    throw new Exception("錯誤：Vector類別字串建構式字串不符合數字轉型。");
                }
            }

            v = para;
        }
        public Vector(Vector vec)
        {
            v = new List<double>();
            for (int i = 0; i < vec.dimention; i++)
                v.Add(vec[i]);
        }
        public Vector(params double[] data)
        {
            v = data.ToList();
        }

        ~Vector()
        {
            v.Clear();
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
            if (vec == null)
                return "null Vector";
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
        public static Vector Normalized(Vector vec)
        {
            if (vec == zero(vec.dimention))
                throw new Exception("錯誤：Vector類別試圖將零向量正規化。");

            if (vec.magnitude == 1)
                return vec;
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
            if (vec.dimention != OnNormal.dimention)
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
        public Vector Normalize()
        {
            if (this == zero(dimention))
                throw new Exception("錯誤：Vector類別試圖將零向量正規化。");

            if (magnitude == 1)
                return this;
            double l = magnitude;
            for (int i = 0; i < dimention; i++)
                v[i] = v[i] / l;
            return this;
        }

        //複寫轉字串方法
        public override string ToString()
        {
            if (this == null)
                return "null Vector";

            string opt = "Vector { name: " + name + "  dimention: " + dimention + "  | ";
            for (int i = 0; i < dimention; i++)
                opt += (i == 0 ? "" : ", ") + v[i];
            opt += "  }";
            return opt;
        }

        public string ToSimpleString()
        {
            if (this == null)
                return "null Vector";

            string opt = "";
            for (int i = 0; i < dimention; i++)
                opt += (i == 0 ? "" : " ") + v[i];
            return opt;
        }

        #endregion
    }

    /// <summary>
    /// 表示一個矩陣。
    /// </summary>
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

            int mycolunm = -1;
            m = new List<List<double>>();
            for (int i = 0; i < myrow; i++)
            {
                string[] nums = data[i].Split(' ');
                List<double> rowvec = new List<double>();
                foreach (string num in nums)
                {
                    if (num == "") continue;
                    try { rowvec.Add(double.Parse(num)); }
                    catch (Exception ep)
                    {
                        throw new Exception("錯誤：Matrix類別建構式傳入字串參數不符合格式。");
                    }
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

        ~Matrix()
        {
            m.ForEach(delegate (List<double> n)
            {
                n.Clear();
            });
            m.Clear();
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
            if (mat == null)
                return "null Matrix";
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
                if (Numerical.IsZero(des[i, i + offset]))
                {
                    bool findrow = false;
                    //找i行非0的列
                    for (int j = i + 1; j < des.row; j++)
                    {
                        if (!Numerical.IsZero(des[j, i + offset]))
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
                if (Numerical.IsZero(reduced[i, i + offset]))
                {
                    bool findrow = false;//判斷是否找到非0的列

                    //找i行非0的列
                    for (int j = i + 1; j < reduced.row; j++)
                    {
                        if (!Numerical.IsZero(reduced[j, i + offset]))
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
                                    if (!Numerical.IsZero(opt[fori][k]))
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
                else if (Numerical.IsZero(delta))
                {
                    if (Numerical.IsZero(algeK))//3重實根
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
                    if (new Vector(t[0]) != Vector.zero(3) && new Vector(t[1]) != Vector.zero(3))//1、2->非0列，參數為1個
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
                    else if (new Vector(t[0]) != Vector.zero(3))//1->非0列  2->0列，參數為2個
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
            if (this == null)
                return "null Matrix";

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
            if (this == null)
                return "null Matrix";

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

    /// <summary>
    /// 表示對一個Formula物件給予一個未定義的輸入時傳回的錯誤類別。
    /// </summary>
    class OutOfDomainException : Exception
    {
        public OutOfDomainException() : base()
        {

        }

        public OutOfDomainException(string _message) : base(_message)
        {

        }
    }

    /// <summary>
    /// 表示對一個Formula物件給予一個不相容的輸入時傳回的錯誤類別。
    /// </summary>
    class IllegalInputException : Exception
    {
        public IllegalInputException()
        {

        }

        public IllegalInputException(string _message) : base(_message)
        {

        }
    }
}
