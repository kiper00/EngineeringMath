namespace Proj3
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pbx1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOrd = new System.Windows.Forms.Label();
            this.lblRdi = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnDo = new System.Windows.Forms.Button();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.pbx2 = new System.Windows.Forms.PictureBox();
            this.pbx3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbx4 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbx1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx4)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pbx1
            // 
            this.pbx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbx1.Location = new System.Drawing.Point(37, 78);
            this.pbx1.Name = "pbx1";
            this.pbx1.Size = new System.Drawing.Size(256, 256);
            this.pbx1.TabIndex = 0;
            this.pbx1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblOrd);
            this.groupBox1.Controls.Add(this.lblRdi);
            this.groupBox1.Controls.Add(this.trackBar2);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.btn4);
            this.groupBox1.Controls.Add(this.btnRead);
            this.groupBox1.Controls.Add(this.btn3);
            this.groupBox1.Controls.Add(this.btnDo);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton6);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1183, 300);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "功能";
            // 
            // radioButton1
            // 
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(470, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(200, 50);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "1";
            this.radioButton1.Text = "None";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(676, 101);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 24);
            this.label9.TabIndex = 6;
            this.label9.Text = "Step3    →    IFFT";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(343, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 24);
            this.label8.TabIndex = 6;
            this.label8.Text = "Step2    →    ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(134, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 24);
            this.label7.TabIndex = 6;
            this.label7.Text = "Step1    →    FFT";
            // 
            // lblOrd
            // 
            this.lblOrd.AutoSize = true;
            this.lblOrd.Location = new System.Drawing.Point(616, 257);
            this.lblOrd.Name = "lblOrd";
            this.lblOrd.Size = new System.Drawing.Size(86, 24);
            this.lblOrd.TabIndex = 5;
            this.lblOrd.Text = "Order  2";
            // 
            // lblRdi
            // 
            this.lblRdi.AutoSize = true;
            this.lblRdi.Location = new System.Drawing.Point(81, 257);
            this.lblRdi.Name = "lblRdi";
            this.lblRdi.Size = new System.Drawing.Size(109, 24);
            this.lblRdi.TabIndex = 4;
            this.lblRdi.Text = "Radius  0.3";
            // 
            // trackBar2
            // 
            this.trackBar2.LargeChange = 10;
            this.trackBar2.Location = new System.Drawing.Point(737, 249);
            this.trackBar2.Maximum = 50;
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(298, 45);
            this.trackBar2.SmallChange = 5;
            this.trackBar2.TabIndex = 3;
            this.trackBar2.Value = 2;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 15;
            this.trackBar1.Location = new System.Drawing.Point(214, 249);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(298, 45);
            this.trackBar1.SmallChange = 5;
            this.trackBar1.TabIndex = 3;
            this.trackBar1.Value = 30;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(891, 102);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(120, 40);
            this.btnRead.TabIndex = 2;
            this.btnRead.Text = "讀檔";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnDo
            // 
            this.btnDo.Location = new System.Drawing.Point(891, 37);
            this.btnDo.Name = "btnDo";
            this.btnDo.Size = new System.Drawing.Size(120, 40);
            this.btnDo.TabIndex = 1;
            this.btnDo.Text = "執行";
            this.btnDo.UseVisualStyleBackColor = true;
            this.btnDo.Click += new System.EventHandler(this.btnDo_Click);
            // 
            // radioButton3
            // 
            this.radioButton3.Location = new System.Drawing.Point(470, 88);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(200, 50);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.Tag = "2";
            this.radioButton3.Text = "Low Pass Filter";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.Location = new System.Drawing.Point(470, 144);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(200, 50);
            this.radioButton6.TabIndex = 0;
            this.radioButton6.Tag = "3";
            this.radioButton6.Text = "High Pass Filter";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // pbx2
            // 
            this.pbx2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbx2.Location = new System.Drawing.Point(320, 78);
            this.pbx2.Name = "pbx2";
            this.pbx2.Size = new System.Drawing.Size(256, 256);
            this.pbx2.TabIndex = 0;
            this.pbx2.TabStop = false;
            // 
            // pbx3
            // 
            this.pbx3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbx3.Location = new System.Drawing.Point(606, 78);
            this.pbx3.Name = "pbx3";
            this.pbx3.Size = new System.Drawing.Size(256, 256);
            this.pbx3.TabIndex = 0;
            this.pbx3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(33, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "原圖";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(316, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "頻域圖";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(602, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "處理後頻域圖";
            // 
            // pbx4
            // 
            this.pbx4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbx4.Location = new System.Drawing.Point(892, 78);
            this.pbx4.Name = "pbx4";
            this.pbx4.Size = new System.Drawing.Size(256, 256);
            this.pbx4.TabIndex = 0;
            this.pbx4.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pbx4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pbx3);
            this.panel1.Controls.Add(this.pbx1);
            this.panel1.Controls.Add(this.pbx2);
            this.panel1.Location = new System.Drawing.Point(11, 318);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1183, 365);
            this.panel1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(888, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 24);
            this.label4.TabIndex = 2;
            this.label4.Text = "反轉換圖";
            // 
            // btn3
            // 
            this.btn3.Location = new System.Drawing.Point(1027, 37);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(120, 40);
            this.btn3.TabIndex = 1;
            this.btn3.Text = "連帶反轉";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn4
            // 
            this.btn4.Location = new System.Drawing.Point(1027, 102);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(120, 40);
            this.btn4.TabIndex = 2;
            this.btn4.Text = "圖片大小";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 695);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbx1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx4)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pbx1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbx2;
        private System.Windows.Forms.Button btnDo;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.PictureBox pbx3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PictureBox pbx4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOrd;
        private System.Windows.Forms.Label lblRdi;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn3;
    }
}

