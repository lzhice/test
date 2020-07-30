namespace ATM3300.Connection.Forms
{
    partial class TcpipRcuDiagnostic
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
            this.btnScan = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.chkSOS = new System.Windows.Forms.CheckBox();
            this.txtTemperature = new System.Windows.Forms.TextBox();
            this.chkCheckout = new System.Windows.Forms.CheckBox();
            this.txtRemotePortNumber = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.labSuccessPercent = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEmptyTemperature = new System.Windows.Forms.TextBox();
            this.labFailedTimes = new System.Windows.Forms.Label();
            this.labSuccessTimes = new System.Windows.Forms.Label();
            this.txtPortNumber = new System.Windows.Forms.TextBox();
            this.labTotalTimes = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFloor = new System.Windows.Forms.TextBox();
            this.txtNetworkID1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNetworkID2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkRepair = new System.Windows.Forms.CheckBox();
            this.chkCleaningCompleted = new System.Windows.Forms.CheckBox();
            this.chkCoffer = new System.Windows.Forms.CheckBox();
            this.txtCardNum = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.txtCardType = new System.Windows.Forms.TextBox();
            this.btnBroadcast = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.chkDontDisturb = new System.Windows.Forms.CheckBox();
            this.chkClean = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkCall = new System.Windows.Forms.CheckBox();
            this.chkCheckRoom = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtClientHumidity = new System.Windows.Forms.TextBox();
            this.txtClientTemp = new System.Windows.Forms.TextBox();
            this.txtHumidity = new System.Windows.Forms.TextBox();
            this.txtPlayingVoice = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.chkLightsOn = new System.Windows.Forms.CheckBox();
            this.chkTV = new System.Windows.Forms.CheckBox();
            this.chkRefrig = new System.Windows.Forms.CheckBox();
            this.chkProblem = new System.Windows.Forms.CheckBox();
            this.chkKey = new System.Windows.Forms.CheckBox();
            this.chkDoor = new System.Windows.Forms.CheckBox();
            this.txtLeaveTemperature = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSleepTemperature = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbRoomStatus = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtVoice = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnBroadcastScan = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbSeason = new System.Windows.Forms.ComboBox();
            this.cmbClientType = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(486, 52);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(89, 32);
            this.btnScan.TabIndex = 26;
            this.btnScan.Text = "开始查询";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(71, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 16);
            this.label10.TabIndex = 24;
            this.label10.Text = "卡类";
            // 
            // chkSOS
            // 
            this.chkSOS.AutoSize = true;
            this.chkSOS.Location = new System.Drawing.Point(90, 20);
            this.chkSOS.Name = "chkSOS";
            this.chkSOS.Size = new System.Drawing.Size(72, 16);
            this.chkSOS.TabIndex = 9;
            this.chkSOS.Text = "求救服务";
            this.chkSOS.UseVisualStyleBackColor = true;
            // 
            // txtTemperature
            // 
            this.txtTemperature.Location = new System.Drawing.Point(7, 120);
            this.txtTemperature.Name = "txtTemperature";
            this.txtTemperature.Size = new System.Drawing.Size(50, 21);
            this.txtTemperature.TabIndex = 9;
            // 
            // chkCheckout
            // 
            this.chkCheckout.AutoSize = true;
            this.chkCheckout.Location = new System.Drawing.Point(8, 86);
            this.chkCheckout.Name = "chkCheckout";
            this.chkCheckout.Size = new System.Drawing.Size(72, 16);
            this.chkCheckout.TabIndex = 7;
            this.chkCheckout.Text = "退房服务";
            this.chkCheckout.UseVisualStyleBackColor = true;
            // 
            // txtRemotePortNumber
            // 
            this.txtRemotePortNumber.Location = new System.Drawing.Point(259, 30);
            this.txtRemotePortNumber.Name = "txtRemotePortNumber";
            this.txtRemotePortNumber.Size = new System.Drawing.Size(55, 21);
            this.txtRemotePortNumber.TabIndex = 26;
            this.txtRemotePortNumber.Text = "20001";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(261, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "远程端口";
            // 
            // labSuccessPercent
            // 
            this.labSuccessPercent.AutoSize = true;
            this.labSuccessPercent.Location = new System.Drawing.Point(6, 39);
            this.labSuccessPercent.Name = "labSuccessPercent";
            this.labSuccessPercent.Size = new System.Drawing.Size(41, 12);
            this.labSuccessPercent.TabIndex = 3;
            this.labSuccessPercent.Text = "成功比";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(202, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "本地端口";
            // 
            // txtEmptyTemperature
            // 
            this.txtEmptyTemperature.Location = new System.Drawing.Point(77, 38);
            this.txtEmptyTemperature.Name = "txtEmptyTemperature";
            this.txtEmptyTemperature.Size = new System.Drawing.Size(44, 21);
            this.txtEmptyTemperature.TabIndex = 22;
            this.txtEmptyTemperature.Text = "20";
            // 
            // labFailedTimes
            // 
            this.labFailedTimes.AutoSize = true;
            this.labFailedTimes.Location = new System.Drawing.Point(303, 17);
            this.labFailedTimes.Name = "labFailedTimes";
            this.labFailedTimes.Size = new System.Drawing.Size(53, 12);
            this.labFailedTimes.TabIndex = 2;
            this.labFailedTimes.Text = "失败次数";
            // 
            // labSuccessTimes
            // 
            this.labSuccessTimes.AutoSize = true;
            this.labSuccessTimes.Location = new System.Drawing.Point(153, 17);
            this.labSuccessTimes.Name = "labSuccessTimes";
            this.labSuccessTimes.Size = new System.Drawing.Size(53, 12);
            this.labSuccessTimes.TabIndex = 1;
            this.labSuccessTimes.Text = "成功次数";
            // 
            // txtPortNumber
            // 
            this.txtPortNumber.Location = new System.Drawing.Point(200, 30);
            this.txtPortNumber.Name = "txtPortNumber";
            this.txtPortNumber.Size = new System.Drawing.Size(55, 21);
            this.txtPortNumber.TabIndex = 24;
            this.txtPortNumber.Text = "20000";
            // 
            // labTotalTimes
            // 
            this.labTotalTimes.AutoSize = true;
            this.labTotalTimes.Location = new System.Drawing.Point(6, 17);
            this.labTotalTimes.Name = "labTotalTimes";
            this.labTotalTimes.Size = new System.Drawing.Size(41, 12);
            this.labTotalTimes.TabIndex = 0;
            this.labTotalTimes.Text = "总次数";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtDelay);
            this.groupBox4.Controls.Add(this.txtRemotePortNumber);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.txtPortNumber);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtFloor);
            this.groupBox4.Controls.Add(this.txtNetworkID1);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txtNetworkID2);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.txtRoom);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(450, 72);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "程序参数";
            // 
            // txtDelay
            // 
            this.txtDelay.Location = new System.Drawing.Point(322, 30);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(39, 21);
            this.txtDelay.TabIndex = 6;
            this.txtDelay.Text = "50";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(320, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "等待延时(ms)";
            // 
            // txtFloor
            // 
            this.txtFloor.Location = new System.Drawing.Point(106, 30);
            this.txtFloor.Name = "txtFloor";
            this.txtFloor.Size = new System.Drawing.Size(39, 21);
            this.txtFloor.TabIndex = 2;
            this.txtFloor.Text = "8";
            // 
            // txtNetworkID1
            // 
            this.txtNetworkID1.Location = new System.Drawing.Point(17, 30);
            this.txtNetworkID1.Name = "txtNetworkID1";
            this.txtNetworkID1.Size = new System.Drawing.Size(38, 21);
            this.txtNetworkID1.TabIndex = 0;
            this.txtNetworkID1.Text = "192";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "网络号";
            // 
            // txtNetworkID2
            // 
            this.txtNetworkID2.Location = new System.Drawing.Point(61, 30);
            this.txtNetworkID2.Name = "txtNetworkID2";
            this.txtNetworkID2.Size = new System.Drawing.Size(38, 21);
            this.txtNetworkID2.TabIndex = 20;
            this.txtNetworkID2.Text = "168";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "楼层";
            // 
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(151, 30);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(43, 21);
            this.txtRoom.TabIndex = 4;
            this.txtRoom.Text = "8";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "房号";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(75, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 21;
            this.label12.Text = "空房温度";
            // 
            // chkRepair
            // 
            this.chkRepair.Location = new System.Drawing.Point(259, 64);
            this.chkRepair.Name = "chkRepair";
            this.chkRepair.Size = new System.Drawing.Size(69, 16);
            this.chkRepair.TabIndex = 30;
            this.chkRepair.Text = "RCU维修状态";
            // 
            // chkCleaningCompleted
            // 
            this.chkCleaningCompleted.AutoSize = true;
            this.chkCleaningCompleted.Location = new System.Drawing.Point(259, 20);
            this.chkCleaningCompleted.Name = "chkCleaningCompleted";
            this.chkCleaningCompleted.Size = new System.Drawing.Size(96, 16);
            this.chkCleaningCompleted.TabIndex = 29;
            this.chkCleaningCompleted.Text = "退房清洁完毕";
            // 
            // chkCoffer
            // 
            this.chkCoffer.Location = new System.Drawing.Point(175, 42);
            this.chkCoffer.Name = "chkCoffer";
            this.chkCoffer.Size = new System.Drawing.Size(64, 16);
            this.chkCoffer.TabIndex = 28;
            this.chkCoffer.Text = "保险箱";
            // 
            // txtCardNum
            // 
            this.txtCardNum.Location = new System.Drawing.Point(132, 160);
            this.txtCardNum.Name = "txtCardNum";
            this.txtCardNum.Size = new System.Drawing.Size(54, 21);
            this.txtCardNum.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "空调速度";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(132, 144);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 16);
            this.label11.TabIndex = 26;
            this.label11.Text = "卡号";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(7, 160);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(50, 21);
            this.txtSpeed.TabIndex = 11;
            // 
            // txtCardType
            // 
            this.txtCardType.Location = new System.Drawing.Point(71, 160);
            this.txtCardType.Name = "txtCardType";
            this.txtCardType.Size = new System.Drawing.Size(50, 21);
            this.txtCardType.TabIndex = 25;
            // 
            // btnBroadcast
            // 
            this.btnBroadcast.Enabled = false;
            this.btnBroadcast.Location = new System.Drawing.Point(487, 89);
            this.btnBroadcast.Name = "btnBroadcast";
            this.btnBroadcast.Size = new System.Drawing.Size(88, 32);
            this.btnBroadcast.TabIndex = 28;
            this.btnBroadcast.Text = "广播公共信息";
            this.btnBroadcast.UseVisualStyleBackColor = true;
            this.btnBroadcast.Click += new System.EventHandler(this.btnBroadcast_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "温度";
            // 
            // chkDontDisturb
            // 
            this.chkDontDisturb.AutoSize = true;
            this.chkDontDisturb.Location = new System.Drawing.Point(8, 42);
            this.chkDontDisturb.Name = "chkDontDisturb";
            this.chkDontDisturb.Size = new System.Drawing.Size(72, 16);
            this.chkDontDisturb.TabIndex = 5;
            this.chkDontDisturb.Text = "勿扰服务";
            this.chkDontDisturb.UseVisualStyleBackColor = true;
            // 
            // chkClean
            // 
            this.chkClean.AutoSize = true;
            this.chkClean.Location = new System.Drawing.Point(8, 20);
            this.chkClean.Name = "chkClean";
            this.chkClean.Size = new System.Drawing.Size(72, 16);
            this.chkClean.TabIndex = 4;
            this.chkClean.Text = "清洁服务";
            this.chkClean.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labSuccessPercent);
            this.groupBox2.Controls.Add(this.labFailedTimes);
            this.groupBox2.Controls.Add(this.labSuccessTimes);
            this.groupBox2.Controls.Add(this.labTotalTimes);
            this.groupBox2.Location = new System.Drawing.Point(12, 405);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 62);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "诊断结果";
            // 
            // chkCall
            // 
            this.chkCall.AutoSize = true;
            this.chkCall.Location = new System.Drawing.Point(8, 64);
            this.chkCall.Name = "chkCall";
            this.chkCall.Size = new System.Drawing.Size(72, 16);
            this.chkCall.TabIndex = 6;
            this.chkCall.Text = "呼叫服务";
            this.chkCall.UseVisualStyleBackColor = true;
            // 
            // chkCheckRoom
            // 
            this.chkCheckRoom.AutoSize = true;
            this.chkCheckRoom.Location = new System.Drawing.Point(259, 85);
            this.chkCheckRoom.Name = "chkCheckRoom";
            this.chkCheckRoom.Size = new System.Drawing.Size(72, 16);
            this.chkCheckRoom.TabIndex = 3;
            this.chkCheckRoom.Text = "查房完毕";
            this.chkCheckRoom.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtClientHumidity);
            this.groupBox1.Controls.Add(this.txtClientTemp);
            this.groupBox1.Controls.Add(this.txtHumidity);
            this.groupBox1.Controls.Add(this.txtPlayingVoice);
            this.groupBox1.Controls.Add(this.txtCardNum);
            this.groupBox1.Controls.Add(this.txtSpeed);
            this.groupBox1.Controls.Add(this.txtCardType);
            this.groupBox1.Controls.Add(this.txtTemperature);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.chkLightsOn);
            this.groupBox1.Controls.Add(this.chkTV);
            this.groupBox1.Controls.Add(this.chkRefrig);
            this.groupBox1.Controls.Add(this.chkRepair);
            this.groupBox1.Controls.Add(this.chkCleaningCompleted);
            this.groupBox1.Controls.Add(this.chkCoffer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkSOS);
            this.groupBox1.Controls.Add(this.chkCheckout);
            this.groupBox1.Controls.Add(this.chkCall);
            this.groupBox1.Controls.Add(this.chkDontDisturb);
            this.groupBox1.Controls.Add(this.chkClean);
            this.groupBox1.Controls.Add(this.chkCheckRoom);
            this.groupBox1.Controls.Add(this.chkProblem);
            this.groupBox1.Controls.Add(this.chkKey);
            this.groupBox1.Controls.Add(this.chkDoor);
            this.groupBox1.Location = new System.Drawing.Point(12, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 187);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "房间状态";
            // 
            // txtClientHumidity
            // 
            this.txtClientHumidity.Location = new System.Drawing.Point(200, 120);
            this.txtClientHumidity.Name = "txtClientHumidity";
            this.txtClientHumidity.Size = new System.Drawing.Size(50, 21);
            this.txtClientHumidity.TabIndex = 40;
            // 
            // txtClientTemp
            // 
            this.txtClientTemp.Location = new System.Drawing.Point(134, 120);
            this.txtClientTemp.Name = "txtClientTemp";
            this.txtClientTemp.Size = new System.Drawing.Size(50, 21);
            this.txtClientTemp.TabIndex = 38;
            // 
            // txtHumidity
            // 
            this.txtHumidity.Location = new System.Drawing.Point(71, 120);
            this.txtHumidity.Name = "txtHumidity";
            this.txtHumidity.Size = new System.Drawing.Size(50, 21);
            this.txtHumidity.TabIndex = 36;
            // 
            // txtPlayingVoice
            // 
            this.txtPlayingVoice.Location = new System.Drawing.Point(199, 160);
            this.txtPlayingVoice.Name = "txtPlayingVoice";
            this.txtPlayingVoice.Size = new System.Drawing.Size(53, 21);
            this.txtPlayingVoice.TabIndex = 35;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(199, 105);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 12);
            this.label21.TabIndex = 41;
            this.label21.Text = "客人湿度";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(133, 105);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 39;
            this.label20.Text = "客人温度";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(71, 105);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(29, 12);
            this.label19.TabIndex = 37;
            this.label19.Text = "湿度";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(199, 144);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 12);
            this.label18.TabIndex = 34;
            this.label18.Text = "播放语音";
            // 
            // chkLightsOn
            // 
            this.chkLightsOn.AutoSize = true;
            this.chkLightsOn.Location = new System.Drawing.Point(175, 85);
            this.chkLightsOn.Name = "chkLightsOn";
            this.chkLightsOn.Size = new System.Drawing.Size(48, 16);
            this.chkLightsOn.TabIndex = 33;
            this.chkLightsOn.Text = "开灯";
            // 
            // chkTV
            // 
            this.chkTV.AutoSize = true;
            this.chkTV.Location = new System.Drawing.Point(175, 64);
            this.chkTV.Name = "chkTV";
            this.chkTV.Size = new System.Drawing.Size(48, 16);
            this.chkTV.TabIndex = 32;
            this.chkTV.Text = "电视";
            // 
            // chkRefrig
            // 
            this.chkRefrig.AutoSize = true;
            this.chkRefrig.Location = new System.Drawing.Point(175, 20);
            this.chkRefrig.Name = "chkRefrig";
            this.chkRefrig.Size = new System.Drawing.Size(48, 16);
            this.chkRefrig.TabIndex = 31;
            this.chkRefrig.Text = "冰箱";
            // 
            // chkProblem
            // 
            this.chkProblem.AutoSize = true;
            this.chkProblem.Location = new System.Drawing.Point(259, 42);
            this.chkProblem.Name = "chkProblem";
            this.chkProblem.Size = new System.Drawing.Size(66, 16);
            this.chkProblem.TabIndex = 2;
            this.chkProblem.Text = "RCU故障";
            this.chkProblem.UseVisualStyleBackColor = true;
            // 
            // chkKey
            // 
            this.chkKey.AutoSize = true;
            this.chkKey.Location = new System.Drawing.Point(90, 86);
            this.chkKey.Name = "chkKey";
            this.chkKey.Size = new System.Drawing.Size(48, 16);
            this.chkKey.TabIndex = 1;
            this.chkKey.Text = "钥匙";
            this.chkKey.UseVisualStyleBackColor = true;
            // 
            // chkDoor
            // 
            this.chkDoor.AutoSize = true;
            this.chkDoor.Location = new System.Drawing.Point(90, 64);
            this.chkDoor.Name = "chkDoor";
            this.chkDoor.Size = new System.Drawing.Size(48, 16);
            this.chkDoor.TabIndex = 0;
            this.chkDoor.Text = "房门";
            this.chkDoor.UseVisualStyleBackColor = true;
            // 
            // txtLeaveTemperature
            // 
            this.txtLeaveTemperature.Location = new System.Drawing.Point(17, 38);
            this.txtLeaveTemperature.Name = "txtLeaveTemperature";
            this.txtLeaveTemperature.Size = new System.Drawing.Size(44, 21);
            this.txtLeaveTemperature.TabIndex = 29;
            this.txtLeaveTemperature.Text = "20";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 28;
            this.label9.Text = "暂离温度";
            // 
            // txtSleepTemperature
            // 
            this.txtSleepTemperature.Location = new System.Drawing.Point(138, 37);
            this.txtSleepTemperature.Name = "txtSleepTemperature";
            this.txtSleepTemperature.Size = new System.Drawing.Size(44, 21);
            this.txtSleepTemperature.TabIndex = 31;
            this.txtSleepTemperature.Text = "20";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(136, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 30;
            this.label13.Text = "睡眠温度";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(29, 12);
            this.label14.TabIndex = 32;
            this.label14.Text = "房态";
            // 
            // cmbRoomStatus
            // 
            this.cmbRoomStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoomStatus.FormattingEnabled = true;
            this.cmbRoomStatus.Location = new System.Drawing.Point(17, 85);
            this.cmbRoomStatus.Name = "cmbRoomStatus";
            this.cmbRoomStatus.Size = new System.Drawing.Size(92, 20);
            this.cmbRoomStatus.TabIndex = 33;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(201, 66);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 34;
            this.label15.Text = "预置语音";
            // 
            // txtVoice
            // 
            this.txtVoice.Location = new System.Drawing.Point(200, 85);
            this.txtVoice.Name = "txtVoice";
            this.txtVoice.Size = new System.Drawing.Size(54, 21);
            this.txtVoice.TabIndex = 35;
            this.txtVoice.Text = "0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(486, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 32);
            this.button2.TabIndex = 30;
            this.button2.Text = "打开端口";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnBroadcastScan
            // 
            this.btnBroadcastScan.Enabled = false;
            this.btnBroadcastScan.Location = new System.Drawing.Point(486, 127);
            this.btnBroadcastScan.Name = "btnBroadcastScan";
            this.btnBroadcastScan.Size = new System.Drawing.Size(89, 32);
            this.btnBroadcastScan.TabIndex = 31;
            this.btnBroadcastScan.Text = "广播查询信息";
            this.btnBroadcastScan.UseVisualStyleBackColor = true;
            this.btnBroadcastScan.Click += new System.EventHandler(this.btnBroadcastScan_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(261, 66);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 36;
            this.label16.Text = "季节";
            // 
            // cmbSeason
            // 
            this.cmbSeason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSeason.FormattingEnabled = true;
            this.cmbSeason.Items.AddRange(new object[] {
            "0 - 冬季",
            "1 - 夏季"});
            this.cmbSeason.Location = new System.Drawing.Point(263, 85);
            this.cmbSeason.Name = "cmbSeason";
            this.cmbSeason.Size = new System.Drawing.Size(68, 20);
            this.cmbSeason.TabIndex = 37;
            // 
            // cmbClientType
            // 
            this.cmbClientType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClientType.FormattingEnabled = true;
            this.cmbClientType.Items.AddRange(new object[] {
            "0 - 忽略",
            "1 - VIP",
            "2 - 普通",
            "3 - 残疾人"});
            this.cmbClientType.Location = new System.Drawing.Point(115, 85);
            this.cmbClientType.Name = "cmbClientType";
            this.cmbClientType.Size = new System.Drawing.Size(79, 20);
            this.cmbClientType.TabIndex = 39;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(115, 66);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 38;
            this.label17.Text = "入住类型";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cmbClientType);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.txtEmptyTemperature);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.txtLeaveTemperature);
            this.groupBox5.Controls.Add(this.cmbSeason);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.txtSleepTemperature);
            this.groupBox5.Controls.Add(this.label16);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.cmbRoomStatus);
            this.groupBox5.Controls.Add(this.txtVoice);
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Location = new System.Drawing.Point(12, 90);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(450, 116);
            this.groupBox5.TabIndex = 32;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "房间信息";
            // 
            // TcpipRcuDiagnostic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 471);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnBroadcastScan);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnBroadcast);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TcpipRcuDiagnostic";
            this.Text = "TcpipRcuDiagnostic V1.0.1";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkSOS;
        private System.Windows.Forms.TextBox txtTemperature;
        private System.Windows.Forms.CheckBox chkCheckout;
        private System.Windows.Forms.TextBox txtRemotePortNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labSuccessPercent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEmptyTemperature;
        private System.Windows.Forms.Label labFailedTimes;
        private System.Windows.Forms.Label labSuccessTimes;
        private System.Windows.Forms.TextBox txtPortNumber;
        private System.Windows.Forms.Label labTotalTimes;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtFloor;
        private System.Windows.Forms.TextBox txtNetworkID1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNetworkID2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDelay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkRepair;
        private System.Windows.Forms.CheckBox chkCleaningCompleted;
        private System.Windows.Forms.CheckBox chkCoffer;
        private System.Windows.Forms.TextBox txtCardNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.TextBox txtCardType;
        private System.Windows.Forms.Button btnBroadcast;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkDontDisturb;
        private System.Windows.Forms.CheckBox chkClean;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkCall;
        private System.Windows.Forms.CheckBox chkCheckRoom;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkProblem;
        private System.Windows.Forms.CheckBox chkKey;
        private System.Windows.Forms.CheckBox chkDoor;
        private System.Windows.Forms.TextBox txtSleepTemperature;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLeaveTemperature;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbRoomStatus;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtVoice;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnBroadcastScan;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmbSeason;
        private System.Windows.Forms.ComboBox cmbClientType;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkRefrig;
        private System.Windows.Forms.CheckBox chkTV;
        private System.Windows.Forms.CheckBox chkLightsOn;
        private System.Windows.Forms.TextBox txtPlayingVoice;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtClientHumidity;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtClientTemp;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtHumidity;
    }
}