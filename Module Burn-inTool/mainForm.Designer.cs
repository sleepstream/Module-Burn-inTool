namespace Module_Burn_inTool
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtModuleID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtModuleFirmware = new System.Windows.Forms.TextBox();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPD3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUset = new System.Windows.Forms.TextBox();
            this.txtILD1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtILD2 = new System.Windows.Forms.TextBox();
            this.indPower = new System.Windows.Forms.TextBox();
            this.txtILD3 = new System.Windows.Forms.TextBox();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.txtILD4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtILD5 = new System.Windows.Forms.TextBox();
            this.lblPulsedMode = new System.Windows.Forms.Label();
            this.txtILD6 = new System.Windows.Forms.TextBox();
            this.lblExternalShutdown = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblEmission = new System.Windows.Forms.Label();
            this.txtPD2 = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblPowerSupply = new System.Windows.Forms.Label();
            this.txtPD4 = new System.Windows.Forms.TextBox();
            this.lblEnabled = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtPD1 = new System.Windows.Forms.TextBox();
            this.udIset = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.txtPD5 = new System.Windows.Forms.TextBox();
            this.txtLogFile = new System.Windows.Forms.TextBox();
            this.tbIset = new System.Windows.Forms.TrackBar();
            this.btnLogFile = new System.Windows.Forms.Button();
            this.btnStartBurnIn = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.chbEmissionOn = new System.Windows.Forms.CheckBox();
            this.lblPLDFailure = new System.Windows.Forms.Label();
            this.lstFailures = new System.Windows.Forms.ListBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.udMinDrop = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tmrLog = new System.Windows.Forms.Timer(this.components);
            this.label19 = new System.Windows.Forms.Label();
            this.txtPSVoltage = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.PD3Monitoring = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.udMinDropPowerDrop = new System.Windows.Forms.NumericUpDown();
            this.chbEmissionOffwhenPowerDrop = new System.Windows.Forms.CheckBox();
            this.chkQCW = new System.Windows.Forms.CheckBox();
            this.gbPLD = new System.Windows.Forms.GroupBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtInstabPower = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txtInstabPD3 = new System.Windows.Forms.TextBox();
            this.txtCycleTime = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.IgType = new System.Windows.Forms.ComboBox();
            this.txtFullTime = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.autoStartBrn = new System.Windows.Forms.NumericUpDown();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label24 = new System.Windows.Forms.Label();
            this.txtMesPower = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.cbMeasuringDevice = new System.Windows.Forms.ComboBox();
            this.cmbPMport = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.udCalCoeff = new System.Windows.Forms.NumericUpDown();
            this.btnCalibratePwM = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.averagingPD3 = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.PD3ChangeDirect = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.PD3MaxTimeMonitoringCycle = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.btnPLDTest = new System.Windows.Forms.Button();
            this.waitingTmr = new System.Windows.Forms.Timer(this.components);
            this.btnCalibration = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.TextBox();
            this.statusLbl = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pwrMesure = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label34 = new System.Windows.Forms.Label();
            this.warmUpTimePower = new System.Windows.Forms.NumericUpDown();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.burnInCurrent = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.udMinBurnIn = new System.Windows.Forms.NumericUpDown();
            this.lstError = new System.Windows.Forms.ListBox();
            this.label36 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udIset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbIset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinDrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PD3Monitoring)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinDropPowerDrop)).BeginInit();
            this.gbPLD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoStartBrn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCalCoeff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.averagingPD3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PD3ChangeDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PD3MaxTimeMonitoringCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warmUpTimePower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinBurnIn)).BeginInit();
            this.SuspendLayout();
            // 
            // cbPort
            // 
            this.cbPort.BackColor = System.Drawing.SystemColors.Window;
            this.cbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(10, 188);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(153, 21);
            this.cbPort.TabIndex = 0;
            // 
            // connectBtn
            // 
            this.connectBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.connectBtn.Location = new System.Drawing.Point(354, 187);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(144, 45);
            this.connectBtn.TabIndex = 1;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Module SN:";
            // 
            // txtModuleID
            // 
            this.txtModuleID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtModuleID.Location = new System.Drawing.Point(72, 232);
            this.txtModuleID.Name = "txtModuleID";
            this.txtModuleID.ReadOnly = true;
            this.txtModuleID.Size = new System.Drawing.Size(117, 20);
            this.txtModuleID.TabIndex = 6;
            this.txtModuleID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtModuleID.DoubleClick += new System.EventHandler(this.ModuleId_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Firmware:";
            // 
            // txtModuleFirmware
            // 
            this.txtModuleFirmware.ForeColor = System.Drawing.Color.Black;
            this.txtModuleFirmware.Location = new System.Drawing.Point(72, 211);
            this.txtModuleFirmware.Name = "txtModuleFirmware";
            this.txtModuleFirmware.ReadOnly = true;
            this.txtModuleFirmware.Size = new System.Drawing.Size(271, 20);
            this.txtModuleFirmware.TabIndex = 8;
            this.txtModuleFirmware.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtModuleFirmware.DoubleClick += new System.EventHandler(this.ModuleFimware_Click);
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 10;
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(154, 404);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "ILD2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(154, 380);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "ILD1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(154, 428);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "ILD3";
            // 
            // txtPD3
            // 
            this.txtPD3.Location = new System.Drawing.Point(322, 428);
            this.txtPD3.Name = "txtPD3";
            this.txtPD3.ReadOnly = true;
            this.txtPD3.Size = new System.Drawing.Size(64, 20);
            this.txtPD3.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(154, 452);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "ILD4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 380);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "PD1:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(154, 476);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "ILD5";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(18, 428);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = "Uset:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(154, 500);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "ILD6";
            // 
            // txtUset
            // 
            this.txtUset.Location = new System.Drawing.Point(82, 428);
            this.txtUset.Name = "txtUset";
            this.txtUset.ReadOnly = true;
            this.txtUset.Size = new System.Drawing.Size(64, 20);
            this.txtUset.TabIndex = 18;
            // 
            // txtILD1
            // 
            this.txtILD1.Location = new System.Drawing.Point(202, 380);
            this.txtILD1.Name = "txtILD1";
            this.txtILD1.ReadOnly = true;
            this.txtILD1.Size = new System.Drawing.Size(64, 20);
            this.txtILD1.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 380);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Power  ind:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtILD2
            // 
            this.txtILD2.Location = new System.Drawing.Point(202, 404);
            this.txtILD2.Name = "txtILD2";
            this.txtILD2.ReadOnly = true;
            this.txtILD2.Size = new System.Drawing.Size(64, 20);
            this.txtILD2.TabIndex = 35;
            // 
            // indPower
            // 
            this.indPower.Location = new System.Drawing.Point(82, 380);
            this.indPower.Name = "indPower";
            this.indPower.ReadOnly = true;
            this.indPower.Size = new System.Drawing.Size(64, 20);
            this.indPower.TabIndex = 16;
            // 
            // txtILD3
            // 
            this.txtILD3.Location = new System.Drawing.Point(202, 428);
            this.txtILD3.Name = "txtILD3";
            this.txtILD3.ReadOnly = true;
            this.txtILD3.Size = new System.Drawing.Size(64, 20);
            this.txtILD3.TabIndex = 36;
            // 
            // txtTemp
            // 
            this.txtTemp.Location = new System.Drawing.Point(82, 452);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.ReadOnly = true;
            this.txtTemp.Size = new System.Drawing.Size(64, 20);
            this.txtTemp.TabIndex = 13;
            // 
            // txtILD4
            // 
            this.txtILD4.Location = new System.Drawing.Point(202, 452);
            this.txtILD4.Name = "txtILD4";
            this.txtILD4.ReadOnly = true;
            this.txtILD4.Size = new System.Drawing.Size(64, 20);
            this.txtILD4.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 452);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Temp:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtILD5
            // 
            this.txtILD5.Location = new System.Drawing.Point(202, 476);
            this.txtILD5.Name = "txtILD5";
            this.txtILD5.ReadOnly = true;
            this.txtILD5.Size = new System.Drawing.Size(64, 20);
            this.txtILD5.TabIndex = 38;
            // 
            // lblPulsedMode
            // 
            this.lblPulsedMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPulsedMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPulsedMode.ForeColor = System.Drawing.Color.Gray;
            this.lblPulsedMode.Location = new System.Drawing.Point(276, 260);
            this.lblPulsedMode.Name = "lblPulsedMode";
            this.lblPulsedMode.Size = new System.Drawing.Size(120, 24);
            this.lblPulsedMode.TabIndex = 26;
            this.lblPulsedMode.Text = "Pulsed Mode";
            this.lblPulsedMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtILD6
            // 
            this.txtILD6.Location = new System.Drawing.Point(202, 500);
            this.txtILD6.Name = "txtILD6";
            this.txtILD6.ReadOnly = true;
            this.txtILD6.Size = new System.Drawing.Size(64, 20);
            this.txtILD6.TabIndex = 39;
            // 
            // lblExternalShutdown
            // 
            this.lblExternalShutdown.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblExternalShutdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblExternalShutdown.ForeColor = System.Drawing.Color.Gray;
            this.lblExternalShutdown.Location = new System.Drawing.Point(138, 292);
            this.lblExternalShutdown.Name = "lblExternalShutdown";
            this.lblExternalShutdown.Size = new System.Drawing.Size(120, 24);
            this.lblExternalShutdown.TabIndex = 25;
            this.lblExternalShutdown.Text = "External Shutdown";
            this.lblExternalShutdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(282, 404);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 40;
            this.label13.Text = "PD2:";
            // 
            // lblEmission
            // 
            this.lblEmission.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEmission.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEmission.ForeColor = System.Drawing.Color.Gray;
            this.lblEmission.Location = new System.Drawing.Point(138, 260);
            this.lblEmission.Name = "lblEmission";
            this.lblEmission.Size = new System.Drawing.Size(120, 24);
            this.lblEmission.TabIndex = 24;
            this.lblEmission.Text = "Emission";
            this.lblEmission.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPD2
            // 
            this.txtPD2.Location = new System.Drawing.Point(322, 404);
            this.txtPD2.Name = "txtPD2";
            this.txtPD2.ReadOnly = true;
            this.txtPD2.Size = new System.Drawing.Size(64, 20);
            this.txtPD2.TabIndex = 41;
            // 
            // lblError
            // 
            this.lblError.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblError.ForeColor = System.Drawing.Color.Gray;
            this.lblError.Location = new System.Drawing.Point(276, 292);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(120, 24);
            this.lblError.TabIndex = 23;
            this.lblError.Text = "Error";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(282, 428);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 13);
            this.label14.TabIndex = 42;
            this.label14.Text = "PD3:";
            // 
            // lblPowerSupply
            // 
            this.lblPowerSupply.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPowerSupply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPowerSupply.ForeColor = System.Drawing.Color.Gray;
            this.lblPowerSupply.Location = new System.Drawing.Point(10, 292);
            this.lblPowerSupply.Name = "lblPowerSupply";
            this.lblPowerSupply.Size = new System.Drawing.Size(112, 24);
            this.lblPowerSupply.TabIndex = 21;
            this.lblPowerSupply.Text = "Power Supply";
            this.lblPowerSupply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPD4
            // 
            this.txtPD4.Location = new System.Drawing.Point(322, 452);
            this.txtPD4.Name = "txtPD4";
            this.txtPD4.ReadOnly = true;
            this.txtPD4.Size = new System.Drawing.Size(64, 20);
            this.txtPD4.TabIndex = 43;
            // 
            // lblEnabled
            // 
            this.lblEnabled.BackColor = System.Drawing.SystemColors.Control;
            this.lblEnabled.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEnabled.ForeColor = System.Drawing.Color.Gray;
            this.lblEnabled.Location = new System.Drawing.Point(10, 260);
            this.lblEnabled.Name = "lblEnabled";
            this.lblEnabled.Size = new System.Drawing.Size(112, 24);
            this.lblEnabled.TabIndex = 22;
            this.lblEnabled.Text = "Module Enabled";
            this.lblEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(282, 452);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 44;
            this.label15.Text = "PD4:";
            // 
            // txtPD1
            // 
            this.txtPD1.Location = new System.Drawing.Point(322, 380);
            this.txtPD1.Name = "txtPD1";
            this.txtPD1.ReadOnly = true;
            this.txtPD1.Size = new System.Drawing.Size(64, 20);
            this.txtPD1.TabIndex = 45;
            // 
            // udIset
            // 
            this.udIset.Enabled = false;
            this.udIset.Location = new System.Drawing.Point(332, 338);
            this.udIset.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
            this.udIset.Name = "udIset";
            this.udIset.Size = new System.Drawing.Size(64, 20);
            this.udIset.TabIndex = 20;
            this.udIset.ValueChanged += new System.EventHandler(this.udIset_ValueChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(675, 455);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(54, 16);
            this.label17.TabIndex = 11;
            this.label17.Text = "Log file:";
            // 
            // txtPD5
            // 
            this.txtPD5.Location = new System.Drawing.Point(322, 476);
            this.txtPD5.Name = "txtPD5";
            this.txtPD5.ReadOnly = true;
            this.txtPD5.Size = new System.Drawing.Size(64, 20);
            this.txtPD5.TabIndex = 46;
            // 
            // txtLogFile
            // 
            this.txtLogFile.Location = new System.Drawing.Point(723, 451);
            this.txtLogFile.Name = "txtLogFile";
            this.txtLogFile.ReadOnly = true;
            this.txtLogFile.Size = new System.Drawing.Size(128, 20);
            this.txtLogFile.TabIndex = 12;
            // 
            // tbIset
            // 
            this.tbIset.Enabled = false;
            this.tbIset.LargeChange = 500;
            this.tbIset.Location = new System.Drawing.Point(10, 332);
            this.tbIset.Maximum = 4095;
            this.tbIset.Name = "tbIset";
            this.tbIset.Size = new System.Drawing.Size(303, 45);
            this.tbIset.TabIndex = 11;
            this.tbIset.Scroll += new System.EventHandler(this.tbIset_Scroll);
            // 
            // btnLogFile
            // 
            this.btnLogFile.Location = new System.Drawing.Point(851, 451);
            this.btnLogFile.Name = "btnLogFile";
            this.btnLogFile.Size = new System.Drawing.Size(24, 24);
            this.btnLogFile.TabIndex = 13;
            this.btnLogFile.Text = "...";
            this.btnLogFile.UseVisualStyleBackColor = true;
            this.btnLogFile.Click += new System.EventHandler(this.btnLogFile_Click);
            // 
            // btnStartBurnIn
            // 
            this.btnStartBurnIn.Enabled = false;
            this.btnStartBurnIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStartBurnIn.Location = new System.Drawing.Point(411, 259);
            this.btnStartBurnIn.Name = "btnStartBurnIn";
            this.btnStartBurnIn.Size = new System.Drawing.Size(87, 70);
            this.btnStartBurnIn.TabIndex = 10;
            this.btnStartBurnIn.Text = "Start BurnIn";
            this.btnStartBurnIn.UseVisualStyleBackColor = true;
            this.btnStartBurnIn.Click += new System.EventHandler(this.btnStartBurnIn_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(282, 476);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 13);
            this.label16.TabIndex = 47;
            this.label16.Text = "PD5:";
            // 
            // chbEmissionOn
            // 
            this.chbEmissionOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbEmissionOn.BackColor = System.Drawing.Color.Transparent;
            this.chbEmissionOn.Enabled = false;
            this.chbEmissionOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chbEmissionOn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chbEmissionOn.Location = new System.Drawing.Point(412, 333);
            this.chbEmissionOn.Name = "chbEmissionOn";
            this.chbEmissionOn.Size = new System.Drawing.Size(87, 34);
            this.chbEmissionOn.TabIndex = 27;
            this.chbEmissionOn.Text = "Emission";
            this.chbEmissionOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chbEmissionOn.UseMnemonic = false;
            this.chbEmissionOn.UseVisualStyleBackColor = false;
            this.chbEmissionOn.CheckedChanged += new System.EventHandler(this.chbEmissionOn_CheckedChanged);
            // 
            // lblPLDFailure
            // 
            this.lblPLDFailure.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPLDFailure.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPLDFailure.ForeColor = System.Drawing.Color.Gray;
            this.lblPLDFailure.Location = new System.Drawing.Point(25, 16);
            this.lblPLDFailure.Name = "lblPLDFailure";
            this.lblPLDFailure.Size = new System.Drawing.Size(128, 24);
            this.lblPLDFailure.TabIndex = 48;
            this.lblPLDFailure.Text = "Failure";
            this.lblPLDFailure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstFailures
            // 
            this.lstFailures.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lstFailures.ForeColor = System.Drawing.Color.Black;
            this.lstFailures.FormattingEnabled = true;
            this.lstFailures.Location = new System.Drawing.Point(8, 69);
            this.lstFailures.Name = "lstFailures";
            this.lstFailures.ScrollAlwaysVisible = true;
            this.lstFailures.Size = new System.Drawing.Size(580, 69);
            this.lstFailures.TabIndex = 14;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(504, 187);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(94, 45);
            this.btnSettings.TabIndex = 49;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // udMinDrop
            // 
            this.udMinDrop.Location = new System.Drawing.Point(811, 13);
            this.udMinDrop.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.udMinDrop.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMinDrop.Name = "udMinDrop";
            this.udMinDrop.Size = new System.Drawing.Size(64, 20);
            this.udMinDrop.TabIndex = 50;
            this.udMinDrop.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.udMinDrop.ValueChanged += new System.EventHandler(this.udMinDrop_ValueChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(636, 13);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(154, 13);
            this.label18.TabIndex = 49;
            this.label18.Text = "min PD3 drop for \"PLD failure\":";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // tmrLog
            // 
            this.tmrLog.Interval = 5000;
            this.tmrLog.Tick += new System.EventHandler(this.tmrLog_Tick);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(18, 476);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(60, 16);
            this.label19.TabIndex = 50;
            this.label19.Text = "PS Voltage";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPSVoltage
            // 
            this.txtPSVoltage.Location = new System.Drawing.Point(82, 476);
            this.txtPSVoltage.Name = "txtPSVoltage";
            this.txtPSVoltage.ReadOnly = true;
            this.txtPSVoltage.Size = new System.Drawing.Size(64, 20);
            this.txtPSVoltage.TabIndex = 51;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(643, 106);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(156, 13);
            this.label20.TabIndex = 52;
            this.label20.Text = "Waiting for PD3 stabilization (s):";
            // 
            // PD3Monitoring
            // 
            this.PD3Monitoring.Location = new System.Drawing.Point(810, 104);
            this.PD3Monitoring.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.PD3Monitoring.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PD3Monitoring.Name = "PD3Monitoring";
            this.PD3Monitoring.Size = new System.Drawing.Size(64, 20);
            this.PD3Monitoring.TabIndex = 53;
            this.PD3Monitoring.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.PD3Monitoring.ValueChanged += new System.EventHandler(this.PD3Monitoring_ValueChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(635, 39);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(156, 13);
            this.label21.TabIndex = 54;
            this.label21.Tag = "";
            this.label21.Text = "min PD3 drop for \"Power drop\":";
            this.label21.Visible = false;
            // 
            // udMinDropPowerDrop
            // 
            this.udMinDropPowerDrop.Location = new System.Drawing.Point(811, 39);
            this.udMinDropPowerDrop.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.udMinDropPowerDrop.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMinDropPowerDrop.Name = "udMinDropPowerDrop";
            this.udMinDropPowerDrop.Size = new System.Drawing.Size(64, 20);
            this.udMinDropPowerDrop.TabIndex = 55;
            this.udMinDropPowerDrop.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.udMinDropPowerDrop.Visible = false;
            // 
            // chbEmissionOffwhenPowerDrop
            // 
            this.chbEmissionOffwhenPowerDrop.AutoSize = true;
            this.chbEmissionOffwhenPowerDrop.Checked = true;
            this.chbEmissionOffwhenPowerDrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbEmissionOffwhenPowerDrop.Location = new System.Drawing.Point(634, 209);
            this.chbEmissionOffwhenPowerDrop.Name = "chbEmissionOffwhenPowerDrop";
            this.chbEmissionOffwhenPowerDrop.Size = new System.Drawing.Size(189, 17);
            this.chbEmissionOffwhenPowerDrop.TabIndex = 56;
            this.chbEmissionOffwhenPowerDrop.Text = "Shut down module if \"Power drop\"";
            this.chbEmissionOffwhenPowerDrop.UseVisualStyleBackColor = true;
            // 
            // chkQCW
            // 
            this.chkQCW.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkQCW.Enabled = false;
            this.chkQCW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.chkQCW.Location = new System.Drawing.Point(510, 335);
            this.chkQCW.Name = "chkQCW";
            this.chkQCW.Size = new System.Drawing.Size(87, 32);
            this.chkQCW.TabIndex = 57;
            this.chkQCW.Text = "QCW mode";
            this.chkQCW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkQCW.UseVisualStyleBackColor = true;
            this.chkQCW.CheckedChanged += new System.EventHandler(this.chkQCW_CheckedChanged);
            // 
            // gbPLD
            // 
            this.gbPLD.Controls.Add(this.label35);
            this.gbPLD.Controls.Add(this.label33);
            this.gbPLD.Controls.Add(this.txtInstabPower);
            this.gbPLD.Controls.Add(this.lstFailures);
            this.gbPLD.Controls.Add(this.lblPLDFailure);
            this.gbPLD.Controls.Add(this.label32);
            this.gbPLD.Controls.Add(this.txtInstabPD3);
            this.gbPLD.Enabled = false;
            this.gbPLD.Location = new System.Drawing.Point(10, 516);
            this.gbPLD.Name = "gbPLD";
            this.gbPLD.Size = new System.Drawing.Size(588, 148);
            this.gbPLD.TabIndex = 49;
            this.gbPLD.TabStop = false;
            this.gbPLD.Text = "PLD Failure detection";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(8, 53);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(41, 13);
            this.label35.TabIndex = 93;
            this.label35.Text = "Статус";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label33.Location = new System.Drawing.Point(380, 23);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(122, 15);
            this.label33.TabIndex = 92;
            this.label33.Text = "Instability Power (%):";
            // 
            // txtInstabPower
            // 
            this.txtInstabPower.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInstabPower.ForeColor = System.Drawing.Color.Black;
            this.txtInstabPower.Location = new System.Drawing.Point(508, 16);
            this.txtInstabPower.Name = "txtInstabPower";
            this.txtInstabPower.ReadOnly = true;
            this.txtInstabPower.Size = new System.Drawing.Size(73, 22);
            this.txtInstabPower.TabIndex = 91;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label32.Location = new System.Drawing.Point(171, 20);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(111, 15);
            this.label32.TabIndex = 90;
            this.label32.Text = "Instability PD3 (%):";
            // 
            // txtInstabPD3
            // 
            this.txtInstabPD3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInstabPD3.ForeColor = System.Drawing.Color.Black;
            this.txtInstabPD3.Location = new System.Drawing.Point(286, 15);
            this.txtInstabPD3.Name = "txtInstabPD3";
            this.txtInstabPD3.ReadOnly = true;
            this.txtInstabPD3.Size = new System.Drawing.Size(73, 22);
            this.txtInstabPD3.TabIndex = 89;
            // 
            // txtCycleTime
            // 
            this.txtCycleTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtCycleTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCycleTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtCycleTime.Location = new System.Drawing.Point(315, 97);
            this.txtCycleTime.Name = "txtCycleTime";
            this.txtCycleTime.Size = new System.Drawing.Size(213, 73);
            this.txtCycleTime.TabIndex = 58;
            this.txtCycleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCycleTime.Click += new System.EventHandler(this.txtCycleTime_click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(682, 289);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(0, 13);
            this.label22.TabIndex = 59;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(634, 241);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(135, 13);
            this.label23.TabIndex = 60;
            this.label23.Text = "Required burn-in time (min):";
            // 
            // IgType
            // 
            this.IgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IgType.FormattingEnabled = true;
            this.IgType.Location = new System.Drawing.Point(177, 188);
            this.IgType.Name = "IgType";
            this.IgType.Size = new System.Drawing.Size(166, 21);
            this.IgType.TabIndex = 63;
            // 
            // txtFullTime
            // 
            this.txtFullTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtFullTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFullTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtFullTime.Location = new System.Drawing.Point(72, 97);
            this.txtFullTime.Name = "txtFullTime";
            this.txtFullTime.ReadOnly = true;
            this.txtFullTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtFullTime.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtFullTime.Size = new System.Drawing.Size(213, 73);
            this.txtFullTime.TabIndex = 64;
            this.txtFullTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFullTime.Click += new System.EventHandler(this.txtFullTime_click);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox2.Location = new System.Drawing.Point(72, 62);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(213, 19);
            this.textBox2.TabIndex = 65;
            this.textBox2.Text = "Full Time";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox3.Location = new System.Drawing.Point(315, 62);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(213, 19);
            this.textBox3.TabIndex = 66;
            this.textBox3.Text = "Cycle Time";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // autoStartBrn
            // 
            this.autoStartBrn.Location = new System.Drawing.Point(811, 281);
            this.autoStartBrn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.autoStartBrn.Name = "autoStartBrn";
            this.autoStartBrn.Size = new System.Drawing.Size(62, 20);
            this.autoStartBrn.TabIndex = 67;
            this.autoStartBrn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(634, 283);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(128, 13);
            this.label24.TabIndex = 69;
            this.label24.Text = "Auto start burnIn with Log";
            // 
            // txtMesPower
            // 
            this.txtMesPower.Location = new System.Drawing.Point(82, 404);
            this.txtMesPower.Name = "txtMesPower";
            this.txtMesPower.ReadOnly = true;
            this.txtMesPower.Size = new System.Drawing.Size(64, 20);
            this.txtMesPower.TabIndex = 70;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label26.Location = new System.Drawing.Point(634, 316);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(70, 13);
            this.label26.TabIndex = 75;
            this.label26.Text = "Power Meter:";
            // 
            // cbMeasuringDevice
            // 
            this.cbMeasuringDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMeasuringDevice.FormattingEnabled = true;
            this.cbMeasuringDevice.Location = new System.Drawing.Point(710, 313);
            this.cbMeasuringDevice.Name = "cbMeasuringDevice";
            this.cbMeasuringDevice.Size = new System.Drawing.Size(163, 21);
            this.cbMeasuringDevice.TabIndex = 74;
            this.cbMeasuringDevice.SelectedIndexChanged += new System.EventHandler(this.cbMeasuringDevice_SelectedIndexChanged);
            // 
            // cmbPMport
            // 
            this.cmbPMport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPMport.FormattingEnabled = true;
            this.cmbPMport.Location = new System.Drawing.Point(710, 336);
            this.cmbPMport.Name = "cmbPMport";
            this.cmbPMport.Size = new System.Drawing.Size(163, 21);
            this.cmbPMport.TabIndex = 73;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label27.Location = new System.Drawing.Point(634, 339);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(62, 13);
            this.label27.TabIndex = 72;
            this.label27.Text = "Primes port:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(14, 404);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(62, 13);
            this.label25.TabIndex = 76;
            this.label25.Text = "Power mes:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(716, 399);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(54, 13);
            this.label28.TabIndex = 78;
            this.label28.Text = "Cal. value";
            // 
            // udCalCoeff
            // 
            this.udCalCoeff.DecimalPlaces = 3;
            this.udCalCoeff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.udCalCoeff.Location = new System.Drawing.Point(789, 392);
            this.udCalCoeff.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.udCalCoeff.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.udCalCoeff.Name = "udCalCoeff";
            this.udCalCoeff.Size = new System.Drawing.Size(85, 20);
            this.udCalCoeff.TabIndex = 79;
            this.udCalCoeff.ThousandsSeparator = true;
            this.udCalCoeff.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udCalCoeff.ValueChanged += new System.EventHandler(this.udCalCoeff_ValueChanged);
            // 
            // btnCalibratePwM
            // 
            this.btnCalibratePwM.Enabled = false;
            this.btnCalibratePwM.Location = new System.Drawing.Point(724, 418);
            this.btnCalibratePwM.Name = "btnCalibratePwM";
            this.btnCalibratePwM.Size = new System.Drawing.Size(150, 23);
            this.btnCalibratePwM.TabIndex = 80;
            this.btnCalibratePwM.Text = "Calibrate power meter";
            this.btnCalibratePwM.UseVisualStyleBackColor = true;
            this.btnCalibratePwM.Click += new System.EventHandler(this.btnCalibratePwM_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(724, 363);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 23);
            this.button3.TabIndex = 81;
            this.button3.Text = "Connect power meter";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // averagingPD3
            // 
            this.averagingPD3.Location = new System.Drawing.Point(810, 78);
            this.averagingPD3.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.averagingPD3.Name = "averagingPD3";
            this.averagingPD3.Size = new System.Drawing.Size(64, 20);
            this.averagingPD3.TabIndex = 82;
            this.averagingPD3.Visible = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label29.Location = new System.Drawing.Point(673, 81);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(126, 13);
            this.label29.TabIndex = 83;
            this.label29.Text = "Averaging PD3 signal (s):";
            this.label29.Visible = false;
            // 
            // PD3ChangeDirect
            // 
            this.PD3ChangeDirect.Location = new System.Drawing.Point(810, 130);
            this.PD3ChangeDirect.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.PD3ChangeDirect.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PD3ChangeDirect.Name = "PD3ChangeDirect";
            this.PD3ChangeDirect.Size = new System.Drawing.Size(64, 20);
            this.PD3ChangeDirect.TabIndex = 85;
            this.PD3ChangeDirect.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.PD3ChangeDirect.ValueChanged += new System.EventHandler(this.PD3ChangeDirect_ValueChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(644, 131);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(155, 13);
            this.label30.TabIndex = 84;
            this.label30.Text = "PD3 stabilization or grow up (s):";
            // 
            // PD3MaxTimeMonitoringCycle
            // 
            this.PD3MaxTimeMonitoringCycle.Location = new System.Drawing.Point(810, 156);
            this.PD3MaxTimeMonitoringCycle.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.PD3MaxTimeMonitoringCycle.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PD3MaxTimeMonitoringCycle.Name = "PD3MaxTimeMonitoringCycle";
            this.PD3MaxTimeMonitoringCycle.Size = new System.Drawing.Size(64, 20);
            this.PD3MaxTimeMonitoringCycle.TabIndex = 87;
            this.PD3MaxTimeMonitoringCycle.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.PD3MaxTimeMonitoringCycle.ValueChanged += new System.EventHandler(this.PD3MaxTimeMonitoringCycle_ValueChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(631, 157);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(166, 13);
            this.label31.TabIndex = 86;
            this.label31.Text = "PD3 max time waiting for Drop (s):";
            // 
            // btnPLDTest
            // 
            this.btnPLDTest.Enabled = false;
            this.btnPLDTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPLDTest.Location = new System.Drawing.Point(511, 259);
            this.btnPLDTest.Name = "btnPLDTest";
            this.btnPLDTest.Size = new System.Drawing.Size(87, 70);
            this.btnPLDTest.TabIndex = 88;
            this.btnPLDTest.Text = "Test Laser Diod";
            this.btnPLDTest.UseVisualStyleBackColor = true;
            this.btnPLDTest.Click += new System.EventHandler(this.btnPldTest_Click);
            // 
            // waitingTmr
            // 
            this.waitingTmr.Interval = 1000;
            this.waitingTmr.Tick += new System.EventHandler(this.waitingTmr_Tick);
            // 
            // btnCalibration
            // 
            this.btnCalibration.Enabled = false;
            this.btnCalibration.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCalibration.Location = new System.Drawing.Point(413, 373);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(86, 47);
            this.btnCalibration.TabIndex = 89;
            this.btnCalibration.Text = "Калибровка";
            this.btnCalibration.UseVisualStyleBackColor = true;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(690, 634);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(185, 20);
            this.status.TabIndex = 90;
            this.status.Visible = false;
            // 
            // statusLbl
            // 
            this.statusLbl.BackColor = System.Drawing.SystemColors.Control;
            this.statusLbl.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLbl.Location = new System.Drawing.Point(10, 12);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(581, 35);
            this.statusLbl.TabIndex = 91;
            this.statusLbl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pwrMesure
            // 
            this.pwrMesure.Enabled = false;
            this.pwrMesure.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pwrMesure.Location = new System.Drawing.Point(513, 374);
            this.pwrMesure.Name = "pwrMesure";
            this.pwrMesure.Size = new System.Drawing.Size(86, 46);
            this.pwrMesure.TabIndex = 92;
            this.pwrMesure.Text = "Измерение мощности";
            this.pwrMesure.UseVisualStyleBackColor = true;
            this.pwrMesure.Click += new System.EventHandler(this.pwrMesure_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Lime;
            this.progressBar1.Location = new System.Drawing.Point(412, 500);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(185, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 93;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(621, 495);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(184, 13);
            this.label34.TabIndex = 94;
            this.label34.Text = "Время прогрева измерителя (сек):";
            // 
            // warmUpTimePower
            // 
            this.warmUpTimePower.Location = new System.Drawing.Point(811, 493);
            this.warmUpTimePower.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.warmUpTimePower.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.warmUpTimePower.Name = "warmUpTimePower";
            this.warmUpTimePower.Size = new System.Drawing.Size(64, 20);
            this.warmUpTimePower.TabIndex = 95;
            this.warmUpTimePower.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // progressBar2
            // 
            this.progressBar2.ForeColor = System.Drawing.Color.Lime;
            this.progressBar2.Location = new System.Drawing.Point(412, 471);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(185, 23);
            this.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar2.TabIndex = 96;
            // 
            // burnInCurrent
            // 
            this.burnInCurrent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.burnInCurrent.FormattingEnabled = true;
            this.burnInCurrent.Location = new System.Drawing.Point(411, 234);
            this.burnInCurrent.Name = "burnInCurrent";
            this.burnInCurrent.Size = new System.Drawing.Size(187, 21);
            this.burnInCurrent.TabIndex = 97;
            this.burnInCurrent.SelectedIndexChanged += new System.EventHandler(this.burnInCurrent_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(450, 428);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 27);
            this.button1.TabIndex = 98;
            this.button1.Text = "Сброс ошибок";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnReseterror_Click);
            // 
            // udMinBurnIn
            // 
            this.udMinBurnIn.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Module_Burn_inTool.Properties.Settings.Default, "MinBurnInTime", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.udMinBurnIn.Location = new System.Drawing.Point(810, 241);
            this.udMinBurnIn.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.udMinBurnIn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udMinBurnIn.Name = "udMinBurnIn";
            this.udMinBurnIn.Size = new System.Drawing.Size(64, 20);
            this.udMinBurnIn.TabIndex = 61;
            this.udMinBurnIn.Value = global::Module_Burn_inTool.Properties.Settings.Default.MinBurnInTime;
            // 
            // lstError
            // 
            this.lstError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lstError.ForeColor = System.Drawing.Color.Black;
            this.lstError.FormattingEnabled = true;
            this.lstError.Location = new System.Drawing.Point(18, 686);
            this.lstError.Name = "lstError";
            this.lstError.ScrollAlwaysVisible = true;
            this.lstError.Size = new System.Drawing.Size(580, 56);
            this.lstError.TabIndex = 93;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(18, 667);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(47, 13);
            this.label36.TabIndex = 94;
            this.label36.Text = "Ошибки";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(885, 746);
            this.Controls.Add(this.label36);
            this.Controls.Add(this.lstError);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.burnInCurrent);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.warmUpTimePower);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pwrMesure);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.status);
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.txtLogFile);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btnStartBurnIn);
            this.Controls.Add(this.btnLogFile);
            this.Controls.Add(this.btnPLDTest);
            this.Controls.Add(this.PD3MaxTimeMonitoringCycle);
            this.Controls.Add(this.label31);
            this.Controls.Add(this.PD3ChangeDirect);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.averagingPD3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnCalibratePwM);
            this.Controls.Add(this.udCalCoeff);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.cbMeasuringDevice);
            this.Controls.Add(this.cmbPMport);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.txtMesPower);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.autoStartBrn);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.txtFullTime);
            this.Controls.Add(this.IgType);
            this.Controls.Add(this.udMinBurnIn);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.txtCycleTime);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.chkQCW);
            this.Controls.Add(this.chbEmissionOffwhenPowerDrop);
            this.Controls.Add(this.udMinDropPowerDrop);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.PD3Monitoring);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.udMinDrop);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtPSVoltage);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.gbPLD);
            this.Controls.Add(this.txtModuleFirmware);
            this.Controls.Add(this.chbEmissionOn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtModuleID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.tbIset);
            this.Controls.Add(this.cbPort);
            this.Controls.Add(this.lblEnabled);
            this.Controls.Add(this.txtPD5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.udIset);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtPD1);
            this.Controls.Add(this.txtPD3);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPD4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblPowerSupply);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtUset);
            this.Controls.Add(this.txtPD2);
            this.Controls.Add(this.txtILD1);
            this.Controls.Add(this.lblEmission);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtILD2);
            this.Controls.Add(this.lblExternalShutdown);
            this.Controls.Add(this.indPower);
            this.Controls.Add(this.txtILD6);
            this.Controls.Add(this.txtILD3);
            this.Controls.Add(this.lblPulsedMode);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.txtILD5);
            this.Controls.Add(this.txtILD4);
            this.Controls.Add(this.label3);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "Module Burn-inTool IG243-IG337";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udIset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbIset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinDrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PD3Monitoring)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinDropPowerDrop)).EndInit();
            this.gbPLD.ResumeLayout(false);
            this.gbPLD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.autoStartBrn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udCalCoeff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.averagingPD3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PD3ChangeDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PD3MaxTimeMonitoringCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warmUpTimePower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udMinBurnIn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtModuleID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtModuleFirmware;
        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPD3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtUset;
        private System.Windows.Forms.TextBox txtILD1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtILD2;
        private System.Windows.Forms.TextBox indPower;
        private System.Windows.Forms.TextBox txtILD3;
        private System.Windows.Forms.TextBox txtTemp;
        private System.Windows.Forms.TextBox txtILD4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtILD5;
        private System.Windows.Forms.Label lblPulsedMode;
        private System.Windows.Forms.TextBox txtILD6;
        private System.Windows.Forms.Label lblExternalShutdown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblEmission;
        private System.Windows.Forms.TextBox txtPD2;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblPowerSupply;
        private System.Windows.Forms.TextBox txtPD4;
        private System.Windows.Forms.Label lblEnabled;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtPD1;
        private System.Windows.Forms.NumericUpDown udIset;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtPD5;
        private System.Windows.Forms.TextBox txtLogFile;
        private System.Windows.Forms.TrackBar tbIset;
        private System.Windows.Forms.Button btnLogFile;
        private System.Windows.Forms.Button btnStartBurnIn;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox chbEmissionOn;
        private System.Windows.Forms.Label lblPLDFailure;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer tmrLog;
        private System.Windows.Forms.NumericUpDown udMinDrop;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtPSVoltage;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown PD3Monitoring;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown udMinDropPowerDrop;
        private System.Windows.Forms.CheckBox chbEmissionOffwhenPowerDrop;
        private System.Windows.Forms.CheckBox chkQCW;
        private System.Windows.Forms.GroupBox gbPLD;
        public System.Windows.Forms.TextBox txtCycleTime;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown udMinBurnIn;
        private System.Windows.Forms.ComboBox IgType;
        public System.Windows.Forms.TextBox txtFullTime;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ListBox lstFailures;
        private System.Windows.Forms.NumericUpDown autoStartBrn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtMesPower;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox cbMeasuringDevice;
        private System.Windows.Forms.ComboBox cmbPMport;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.NumericUpDown udCalCoeff;
        private System.Windows.Forms.Button btnCalibratePwM;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.NumericUpDown averagingPD3;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.NumericUpDown PD3ChangeDirect;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.NumericUpDown PD3MaxTimeMonitoringCycle;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button btnPLDTest;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtInstabPD3;
        private System.Windows.Forms.Timer waitingTmr;
        private System.Windows.Forms.Button btnCalibration;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtInstabPower;
        private System.Windows.Forms.TextBox statusLbl;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button pwrMesure;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.NumericUpDown warmUpTimePower;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.ComboBox burnInCurrent;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstError;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
    }
}

