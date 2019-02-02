namespace Module_Burn_inTool
{
    partial class DiodTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiodTestForm));
            this.SetPD3Drop = new System.Windows.Forms.NumericUpDown();
            this.TimeWaitPD3Check = new System.Windows.Forms.NumericUpDown();
            this.btnStartCheck = new System.Windows.Forms.Button();
            this.tmrCheck = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartPD3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DropPD3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeltaPD3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.DevitationPD3 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.sfdSaveReport = new System.Windows.Forms.SaveFileDialog();
            this.txtReportFile = new System.Windows.Forms.TextBox();
            this.btnReportFile = new System.Windows.Forms.Button();
            this.waitBeforNext = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.deviationDeltaPD3 = new System.Windows.Forms.NumericUpDown();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.SetPD3Drop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeWaitPD3Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DevitationPD3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitBeforNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviationDeltaPD3)).BeginInit();
            this.SuspendLayout();
            // 
            // SetPD3Drop
            // 
            this.SetPD3Drop.Location = new System.Drawing.Point(444, 21);
            this.SetPD3Drop.Name = "SetPD3Drop";
            this.SetPD3Drop.Size = new System.Drawing.Size(40, 20);
            this.SetPD3Drop.TabIndex = 1;
            this.SetPD3Drop.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // TimeWaitPD3Check
            // 
            this.TimeWaitPD3Check.Location = new System.Drawing.Point(444, 54);
            this.TimeWaitPD3Check.Name = "TimeWaitPD3Check";
            this.TimeWaitPD3Check.Size = new System.Drawing.Size(39, 20);
            this.TimeWaitPD3Check.TabIndex = 2;
            this.TimeWaitPD3Check.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnStartCheck
            // 
            this.btnStartCheck.Location = new System.Drawing.Point(393, 212);
            this.btnStartCheck.Name = "btnStartCheck";
            this.btnStartCheck.Size = new System.Drawing.Size(91, 52);
            this.btnStartCheck.TabIndex = 3;
            this.btnStartCheck.Text = "Start Test";
            this.btnStartCheck.UseVisualStyleBackColor = true;
            this.btnStartCheck.Click += new System.EventHandler(this.btnStartCheck_Click);
            this.btnStartCheck.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DiodTestForm_KeyPress);
            // 
            // tmrCheck
            // 
            this.tmrCheck.Tick += new System.EventHandler(this.tmrCheck_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.StartPD3,
            this.DropPD3,
            this.DeltaPD3});
            this.dataGridView1.Location = new System.Drawing.Point(2, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.Size = new System.Drawing.Size(282, 409);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DiodTestForm_KeyPress);
            // 
            // Number
            // 
            this.Number.HeaderText = "№";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 30;
            // 
            // StartPD3
            // 
            this.StartPD3.HeaderText = "Начальное значение PD3";
            this.StartPD3.Name = "StartPD3";
            this.StartPD3.ReadOnly = true;
            this.StartPD3.Width = 70;
            // 
            // DropPD3
            // 
            this.DropPD3.HeaderText = "Нижнее значение PD3";
            this.DropPD3.Name = "DropPD3";
            this.DropPD3.ReadOnly = true;
            this.DropPD3.Width = 70;
            // 
            // DeltaPD3
            // 
            this.DeltaPD3.HeaderText = "Просадка PD3";
            this.DeltaPD3.Name = "DeltaPD3";
            this.DeltaPD3.Width = 70;
            // 
            // textBox3
            // 
            this.textBox3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox3.Location = new System.Drawing.Point(2, 425);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(477, 20);
            this.textBox3.TabIndex = 8;
            // 
            // DevitationPD3
            // 
            this.DevitationPD3.Location = new System.Drawing.Point(444, 90);
            this.DevitationPD3.Name = "DevitationPD3";
            this.DevitationPD3.Size = new System.Drawing.Size(39, 20);
            this.DevitationPD3.TabIndex = 9;
            this.DevitationPD3.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 26);
            this.label1.TabIndex = 10;
            this.label1.Text = "Регистрируемая \r\nпросадка PD3 (mV)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 26);
            this.label2.TabIndex = 11;
            this.label2.Text = "Время ожидания \r\nперед записью (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Нестабильность PD3";
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(391, 309);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(93, 53);
            this.btnReport.TabIndex = 13;
            this.btnReport.TabStop = false;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // txtReportFile
            // 
            this.txtReportFile.Location = new System.Drawing.Point(322, 283);
            this.txtReportFile.Name = "txtReportFile";
            this.txtReportFile.Size = new System.Drawing.Size(123, 20);
            this.txtReportFile.TabIndex = 15;
            // 
            // btnReportFile
            // 
            this.btnReportFile.Location = new System.Drawing.Point(451, 283);
            this.btnReportFile.Name = "btnReportFile";
            this.btnReportFile.Size = new System.Drawing.Size(29, 20);
            this.btnReportFile.TabIndex = 16;
            this.btnReportFile.Text = "...";
            this.btnReportFile.UseVisualStyleBackColor = true;
            this.btnReportFile.Click += new System.EventHandler(this.btnReportFile_Click);
            // 
            // waitBeforNext
            // 
            this.waitBeforNext.Location = new System.Drawing.Point(444, 121);
            this.waitBeforNext.Name = "waitBeforNext";
            this.waitBeforNext.Size = new System.Drawing.Size(38, 20);
            this.waitBeforNext.TabIndex = 17;
            this.waitBeforNext.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 26);
            this.label4.TabIndex = 18;
            this.label4.Text = "Пауза пеерд проверкой \r\nследующего диода (s)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(312, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 26);
            this.label5.TabIndex = 20;
            this.label5.Text = "Отклонение \r\nпросадки от среднего";
            // 
            // deviationDeltaPD3
            // 
            this.deviationDeltaPD3.Location = new System.Drawing.Point(444, 159);
            this.deviationDeltaPD3.Name = "deviationDeltaPD3";
            this.deviationDeltaPD3.Size = new System.Drawing.Size(38, 20);
            this.deviationDeltaPD3.TabIndex = 19;
            this.deviationDeltaPD3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.deviationDeltaPD3.ValueChanged += new System.EventHandler(this.deviationDeltaPD3_ValueChanged);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(347, 385);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(125, 13);
            this.linkLabel2.TabIndex = 22;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Редактировать шаблон";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // DiodTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 462);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.deviationDeltaPD3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.waitBeforNext);
            this.Controls.Add(this.btnReportFile);
            this.Controls.Add(this.txtReportFile);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevitationPD3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnStartCheck);
            this.Controls.Add(this.TimeWaitPD3Check);
            this.Controls.Add(this.SetPD3Drop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(500, 1090);
            this.MinimumSize = new System.Drawing.Size(500, 38);
            this.Name = "DiodTestForm";
            this.Text = "DiodTestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiodTestForm_FormClosing);
            this.Load += new System.EventHandler(this.DiodTestForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DiodTestForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.SetPD3Drop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeWaitPD3Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DevitationPD3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitBeforNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deviationDeltaPD3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown SetPD3Drop;
        private System.Windows.Forms.NumericUpDown TimeWaitPD3Check;
        private System.Windows.Forms.Button btnStartCheck;
        private System.Windows.Forms.Timer tmrCheck;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartPD3;
        private System.Windows.Forms.DataGridViewTextBoxColumn DropPD3;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeltaPD3;
        private System.Windows.Forms.NumericUpDown DevitationPD3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.SaveFileDialog sfdSaveReport;
        private System.Windows.Forms.TextBox txtReportFile;
        private System.Windows.Forms.Button btnReportFile;
        private System.Windows.Forms.NumericUpDown waitBeforNext;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown deviationDeltaPD3;
        private System.Windows.Forms.LinkLabel linkLabel2;

    }
}