namespace Proj1
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
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
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
            this.lbxCmd = new System.Windows.Forms.ListBox();
            this.tbxIpt = new System.Windows.Forms.TextBox();
            this.tbxOpt = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.lbxVar = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnClearO = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnGuide = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbxCmd
            // 
            this.lbxCmd.Font = new System.Drawing.Font("Hack", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxCmd.FormattingEnabled = true;
            this.lbxCmd.ItemHeight = 22;
            this.lbxCmd.Items.AddRange(new object[] {
            "Vector",
            "",
            "c1\tDot Of Vector",
            "c2\tVector Addition",
            "c3\tScalar Multiplication with vector",
            "c4\tNorm of Vector",
            "c5\tVector normalization",
            "c6\tCross Product",
            "c7\tComponent of a On b",
            "c8\tProjection of a On b",
            "c9\tTriangle area",
            "c10\tParallel judgement",
            "c11\tOrthogonal judgement",
            "c12\tThe angle between two vectors",
            "c13\tThe plane normal produced by vectors",
            "c14\tLinear independence judgement",
            "c15\tGram-schmidt orthogonormal basis",
            "------------------------------------------------------------",
            "",
            "Matrix",
            "",
            "c16\tMatrix addtion",
            "c17\tMatrix Multiplication",
            "c18\tRank of Matrix",
            "c19\tMatrix Transpose",
            "c20\tSolve Linear System",
            "c21\tDeterminants of Matrix",
            "c22\tInverse Matrix",
            "c23\tAdjoint of Matrix",
            "c24\tEigen Vector and Eigen Value",
            "c25\tPower Method of Eigen Value",
            "c26\tMethod of Least Square",
            "c27\tElimination(Upper)",
            "c27\tElimination(Lower)",
            "------------------------------------------------------------",
            "",
            "Calculate",
            "",
            "c27\tVector Calculate",
            "c28\tMatrix Calculate"});
            this.lbxCmd.Location = new System.Drawing.Point(12, 42);
            this.lbxCmd.Name = "lbxCmd";
            this.lbxCmd.Size = new System.Drawing.Size(531, 752);
            this.lbxCmd.TabIndex = 1;
            // 
            // tbxIpt
            // 
            this.tbxIpt.Font = new System.Drawing.Font("Hack", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxIpt.Location = new System.Drawing.Point(549, 42);
            this.tbxIpt.Multiline = true;
            this.tbxIpt.Name = "tbxIpt";
            this.tbxIpt.Size = new System.Drawing.Size(518, 90);
            this.tbxIpt.TabIndex = 3;
            this.tbxIpt.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxIpt_KeyUp);
            // 
            // tbxOpt
            // 
            this.tbxOpt.Font = new System.Drawing.Font("Hack", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxOpt.Location = new System.Drawing.Point(1073, 42);
            this.tbxOpt.Multiline = true;
            this.tbxOpt.Name = "tbxOpt";
            this.tbxOpt.ReadOnly = true;
            this.tbxOpt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbxOpt.Size = new System.Drawing.Size(613, 746);
            this.tbxOpt.TabIndex = 4;
            this.tbxOpt.WordWrap = false;
            // 
            // btnRead
            // 
            this.btnRead.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRead.Location = new System.Drawing.Point(967, 138);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(100, 30);
            this.btnRead.TabIndex = 5;
            this.btnRead.Text = "讀檔";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // lbxVar
            // 
            this.lbxVar.Font = new System.Drawing.Font("Hack", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxVar.FormattingEnabled = true;
            this.lbxVar.HorizontalScrollbar = true;
            this.lbxVar.ItemHeight = 22;
            this.lbxVar.Location = new System.Drawing.Point(553, 177);
            this.lbxVar.Name = "lbxVar";
            this.lbxVar.Size = new System.Drawing.Size(514, 598);
            this.lbxVar.TabIndex = 6;
            this.lbxVar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbxVar_MouseDoubleClick);
            this.lbxVar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbxVar_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "指令";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(549, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 24);
            this.label3.TabIndex = 9;
            this.label3.Text = "輸入";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1069, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "輸出";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(549, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 24);
            this.label5.TabIndex = 11;
            this.label5.Text = "資料";
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClear.Location = new System.Drawing.Point(861, 138);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnWrite
            // 
            this.btnWrite.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnWrite.Location = new System.Drawing.Point(1586, 6);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(100, 30);
            this.btnWrite.TabIndex = 13;
            this.btnWrite.Text = "寫檔";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnClearO
            // 
            this.btnClearO.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnClearO.Location = new System.Drawing.Point(1480, 6);
            this.btnClearO.Name = "btnClearO";
            this.btnClearO.Size = new System.Drawing.Size(100, 30);
            this.btnClearO.TabIndex = 14;
            this.btnClearO.Text = "清除";
            this.btnClearO.UseVisualStyleBackColor = true;
            this.btnClearO.Click += new System.EventHandler(this.btnClearO_Click);
            // 
            // btnGuide
            // 
            this.btnGuide.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGuide.Location = new System.Drawing.Point(443, 6);
            this.btnGuide.Name = "btnGuide";
            this.btnGuide.Size = new System.Drawing.Size(100, 30);
            this.btnGuide.TabIndex = 15;
            this.btnGuide.Text = "說明";
            this.btnGuide.UseVisualStyleBackColor = true;
            this.btnGuide.Click += new System.EventHandler(this.btnGuide_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1698, 803);
            this.Controls.Add(this.btnGuide);
            this.Controls.Add(this.btnClearO);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxVar);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.tbxOpt);
            this.Controls.Add(this.tbxIpt);
            this.Controls.Add(this.lbxCmd);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lbxCmd;
        private System.Windows.Forms.TextBox tbxIpt;
        private System.Windows.Forms.TextBox tbxOpt;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.ListBox lbxVar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnClearO;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnGuide;
    }
}

