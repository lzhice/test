//VirtualATM
//Version alpha1
//Written BY GY@ZQ
//Use TextFile to Communicate Common
// Date [3/28/2004]
//Version alpha2
//Fix a Bug for Temp
//TODO Add Coffer and Key ID
//Edit BY ZQ
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FormOfConnectionText
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.TextBox txtCmd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button Send;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtFloorNumber;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtRoomNumber;
		private System.Windows.Forms.Button btnSetInfo;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioButton7;
		private System.Windows.Forms.RadioButton radioButton8;
		private System.Windows.Forms.RadioButton radioButton9;
		private System.Windows.Forms.RadioButton radioButton10;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radioButton11;
		private System.Windows.Forms.RadioButton radioButton12;
		private System.Windows.Forms.RadioButton radioButton13;
		private System.Windows.Forms.RadioButton radioButton14;
		private System.Windows.Forms.TextBox temp;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.RadioButton radioButton15;
		private System.Windows.Forms.RadioButton radioButton16;
		private System.Windows.Forms.RadioButton radioButton17;
		private System.Windows.Forms.RadioButton radioButton18;
		private System.Windows.Forms.RadioButton radioButton19;
		private System.Windows.Forms.RadioButton radioButton20;
		private System.Windows.Forms.RadioButton radioButton21;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.RadioButton radioButton22;
		private System.Windows.Forms.RadioButton radioButton23;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.RadioButton radioButton24;
		private System.Windows.Forms.RadioButton radioButton25;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.RadioButton radioButton26;
		private System.Windows.Forms.RadioButton radioButton27;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.RadioButton radioButton28;
		private System.Windows.Forms.RadioButton radioButton29;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.RadioButton radioButton30;
		private System.Windows.Forms.RadioButton radioButton31;
		private System.Windows.Forms.RadioButton radioButton32;
		private System.Windows.Forms.GroupBox groupBox11;
		private System.Windows.Forms.RadioButton radioButton33;
		private System.Windows.Forms.RadioButton radioButton34;
		private System.Windows.Forms.GroupBox groupBox12;
		private System.Windows.Forms.RadioButton radioButton35;
		private System.Windows.Forms.RadioButton radioButton36;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		//客人按下虚拟ATM机的按键
		private void SendCommand(string aString)
		{
			StreamWriter FileWriter=new StreamWriter(txtFileName.Text,true);
			
			FileWriter.WriteLine(txtFloorNumber.Text+" "+txtRoomNumber.Text+" "+aString);
						
			FileWriter.Close();
		}

		public Form1()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCmd = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.Send = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.txtFloorNumber = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtRoomNumber = new System.Windows.Forms.TextBox();
			this.btnSetInfo = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioButton14 = new System.Windows.Forms.RadioButton();
			this.radioButton13 = new System.Windows.Forms.RadioButton();
			this.radioButton6 = new System.Windows.Forms.RadioButton();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioButton32 = new System.Windows.Forms.RadioButton();
			this.radioButton10 = new System.Windows.Forms.RadioButton();
			this.radioButton9 = new System.Windows.Forms.RadioButton();
			this.radioButton8 = new System.Windows.Forms.RadioButton();
			this.radioButton7 = new System.Windows.Forms.RadioButton();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioButton12 = new System.Windows.Forms.RadioButton();
			this.radioButton11 = new System.Windows.Forms.RadioButton();
			this.temp = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.radioButton21 = new System.Windows.Forms.RadioButton();
			this.radioButton20 = new System.Windows.Forms.RadioButton();
			this.radioButton19 = new System.Windows.Forms.RadioButton();
			this.radioButton18 = new System.Windows.Forms.RadioButton();
			this.radioButton17 = new System.Windows.Forms.RadioButton();
			this.radioButton16 = new System.Windows.Forms.RadioButton();
			this.radioButton15 = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.radioButton23 = new System.Windows.Forms.RadioButton();
			this.radioButton22 = new System.Windows.Forms.RadioButton();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.radioButton25 = new System.Windows.Forms.RadioButton();
			this.radioButton24 = new System.Windows.Forms.RadioButton();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.radioButton27 = new System.Windows.Forms.RadioButton();
			this.radioButton26 = new System.Windows.Forms.RadioButton();
			this.groupBox9 = new System.Windows.Forms.GroupBox();
			this.radioButton29 = new System.Windows.Forms.RadioButton();
			this.radioButton28 = new System.Windows.Forms.RadioButton();
			this.groupBox10 = new System.Windows.Forms.GroupBox();
			this.radioButton30 = new System.Windows.Forms.RadioButton();
			this.radioButton31 = new System.Windows.Forms.RadioButton();
			this.groupBox11 = new System.Windows.Forms.GroupBox();
			this.radioButton33 = new System.Windows.Forms.RadioButton();
			this.radioButton34 = new System.Windows.Forms.RadioButton();
			this.groupBox12 = new System.Windows.Forms.GroupBox();
			this.radioButton35 = new System.Windows.Forms.RadioButton();
			this.radioButton36 = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox10.SuspendLayout();
			this.groupBox11.SuspendLayout();
			this.groupBox12.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(13, 30);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(214, 20);
			this.txtFileName.TabIndex = 1;
			this.txtFileName.Text = "c:\\room.inout";
			this.txtFileName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(13, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(154, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "输出文件名";
			// 
			// txtCmd
			// 
			this.txtCmd.Location = new System.Drawing.Point(7, 453);
			this.txtCmd.Name = "txtCmd";
			this.txtCmd.Size = new System.Drawing.Size(206, 20);
			this.txtCmd.TabIndex = 19;
			this.txtCmd.Text = "";
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(7, 438);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(193, 15);
			this.label2.TabIndex = 18;
			this.label2.Text = "命令";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// Send
			// 
			this.Send.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Send.Location = new System.Drawing.Point(220, 453);
			this.Send.Name = "Send";
			this.Send.Size = new System.Drawing.Size(47, 22);
			this.Send.TabIndex = 20;
			this.Send.Text = "Send";
			this.Send.Click += new System.EventHandler(this.Send_Click);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(13, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(47, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "楼层号";
			// 
			// txtFloorNumber
			// 
			this.txtFloorNumber.Location = new System.Drawing.Point(67, 52);
			this.txtFloorNumber.Name = "txtFloorNumber";
			this.txtFloorNumber.Size = new System.Drawing.Size(46, 20);
			this.txtFloorNumber.TabIndex = 3;
			this.txtFloorNumber.Text = "3";
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(133, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(47, 23);
			this.label4.TabIndex = 4;
			this.label4.Text = "房间号";
			// 
			// txtRoomNumber
			// 
			this.txtRoomNumber.Location = new System.Drawing.Point(180, 52);
			this.txtRoomNumber.Name = "txtRoomNumber";
			this.txtRoomNumber.Size = new System.Drawing.Size(47, 20);
			this.txtRoomNumber.TabIndex = 5;
			this.txtRoomNumber.Text = "1";
			// 
			// btnSetInfo
			// 
			this.btnSetInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSetInfo.Location = new System.Drawing.Point(560, 7);
			this.btnSetInfo.Name = "btnSetInfo";
			this.btnSetInfo.Size = new System.Drawing.Size(53, 30);
			this.btnSetInfo.TabIndex = 21;
			this.btnSetInfo.Text = "设置";
			this.btnSetInfo.Visible = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButton4);
			this.groupBox1.Controls.Add(this.radioButton3);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(127, 186);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(100, 193);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "空调速度";
			// 
			// radioButton4
			// 
			this.radioButton4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton4.Location = new System.Drawing.Point(13, 162);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(54, 22);
			this.radioButton4.TabIndex = 3;
			this.radioButton4.Text = "高";
			this.radioButton4.Click += new System.EventHandler(this.radioButton4_Click);
			// 
			// radioButton3
			// 
			this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton3.Location = new System.Drawing.Point(13, 115);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(54, 22);
			this.radioButton3.TabIndex = 2;
			this.radioButton3.Text = "中";
			this.radioButton3.Click += new System.EventHandler(this.radioButton3_Click);
			// 
			// radioButton2
			// 
			this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton2.Location = new System.Drawing.Point(13, 69);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(54, 22);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "低";
			this.radioButton2.Click += new System.EventHandler(this.radioButton2_Click);
			// 
			// radioButton1
			// 
			this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton1.Location = new System.Drawing.Point(13, 22);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(54, 23);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.Text = "关";
			this.radioButton1.Click += new System.EventHandler(this.radioButton1_Click);
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioButton14);
			this.groupBox2.Controls.Add(this.radioButton13);
			this.groupBox2.Controls.Add(this.radioButton6);
			this.groupBox2.Controls.Add(this.radioButton5);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(240, 186);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(100, 193);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "钥匙状态";
			// 
			// radioButton14
			// 
			this.radioButton14.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton14.Location = new System.Drawing.Point(13, 162);
			this.radioButton14.Name = "radioButton14";
			this.radioButton14.Size = new System.Drawing.Size(75, 22);
			this.radioButton14.TabIndex = 3;
			this.radioButton14.Text = "领班钥匙";
			this.radioButton14.Click += new System.EventHandler(this.radioButton14_Click);
			// 
			// radioButton13
			// 
			this.radioButton13.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton13.Location = new System.Drawing.Point(13, 111);
			this.radioButton13.Name = "radioButton13";
			this.radioButton13.Size = new System.Drawing.Size(83, 28);
			this.radioButton13.TabIndex = 2;
			this.radioButton13.Text = "服务生钥匙";
			this.radioButton13.Click += new System.EventHandler(this.radioButton13_Click);
			// 
			// radioButton6
			// 
			this.radioButton6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton6.Location = new System.Drawing.Point(13, 67);
			this.radioButton6.Name = "radioButton6";
			this.radioButton6.Size = new System.Drawing.Size(75, 22);
			this.radioButton6.TabIndex = 1;
			this.radioButton6.Text = "客人钥匙";
			this.radioButton6.Click += new System.EventHandler(this.radioButton6_Click);
			// 
			// radioButton5
			// 
			this.radioButton5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton5.Location = new System.Drawing.Point(13, 22);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(75, 23);
			this.radioButton5.TabIndex = 0;
			this.radioButton5.Text = "没有插入";
			this.radioButton5.Click += new System.EventHandler(this.radioButton5_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioButton32);
			this.groupBox3.Controls.Add(this.radioButton10);
			this.groupBox3.Controls.Add(this.radioButton9);
			this.groupBox3.Controls.Add(this.radioButton8);
			this.groupBox3.Controls.Add(this.radioButton7);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(13, 186);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(100, 193);
			this.groupBox3.TabIndex = 11;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "服务";
			// 
			// radioButton32
			// 
			this.radioButton32.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton32.Location = new System.Drawing.Point(16, 96);
			this.radioButton32.Name = "radioButton32";
			this.radioButton32.Size = new System.Drawing.Size(60, 22);
			this.radioButton32.TabIndex = 4;
			this.radioButton32.Text = "退房";
			this.radioButton32.CheckedChanged += new System.EventHandler(this.radioButton32_CheckedChanged);
			// 
			// radioButton10
			// 
			this.radioButton10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton10.Location = new System.Drawing.Point(16, 120);
			this.radioButton10.Name = "radioButton10";
			this.radioButton10.Size = new System.Drawing.Size(72, 22);
			this.radioButton10.TabIndex = 3;
			this.radioButton10.Text = "没有服务";
			this.radioButton10.Click += new System.EventHandler(this.radioButton10_Click);
			// 
			// radioButton9
			// 
			this.radioButton9.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton9.Location = new System.Drawing.Point(16, 72);
			this.radioButton9.Name = "radioButton9";
			this.radioButton9.Size = new System.Drawing.Size(60, 22);
			this.radioButton9.TabIndex = 2;
			this.radioButton9.Text = "请求";
			this.radioButton9.Click += new System.EventHandler(this.radioButton9_Click);
			// 
			// radioButton8
			// 
			this.radioButton8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton8.Location = new System.Drawing.Point(16, 48);
			this.radioButton8.Name = "radioButton8";
			this.radioButton8.Size = new System.Drawing.Size(60, 22);
			this.radioButton8.TabIndex = 1;
			this.radioButton8.Text = "勿扰";
			this.radioButton8.Click += new System.EventHandler(this.radioButton8_Click);
			// 
			// radioButton7
			// 
			this.radioButton7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton7.Location = new System.Drawing.Point(16, 24);
			this.radioButton7.Name = "radioButton7";
			this.radioButton7.Size = new System.Drawing.Size(60, 23);
			this.radioButton7.TabIndex = 0;
			this.radioButton7.Text = "清洁";
			this.radioButton7.Click += new System.EventHandler(this.radioButton7_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioButton12);
			this.groupBox4.Controls.Add(this.radioButton11);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(13, 82);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(74, 89);
			this.groupBox4.TabIndex = 6;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "门状态";
			// 
			// radioButton12
			// 
			this.radioButton12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton12.Location = new System.Drawing.Point(13, 59);
			this.radioButton12.Name = "radioButton12";
			this.radioButton12.Size = new System.Drawing.Size(27, 23);
			this.radioButton12.TabIndex = 1;
			this.radioButton12.Text = "开";
			this.radioButton12.Click += new System.EventHandler(this.radioButton12_Click);
			// 
			// radioButton11
			// 
			this.radioButton11.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton11.Location = new System.Drawing.Point(13, 22);
			this.radioButton11.Name = "radioButton11";
			this.radioButton11.Size = new System.Drawing.Size(27, 23);
			this.radioButton11.TabIndex = 0;
			this.radioButton11.Text = "关";
			this.radioButton11.Click += new System.EventHandler(this.radioButton11_Click);
			// 
			// temp
			// 
			this.temp.Location = new System.Drawing.Point(253, 30);
			this.temp.Name = "temp";
			this.temp.Size = new System.Drawing.Size(47, 20);
			this.temp.TabIndex = 16;
			this.temp.Text = "20";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(253, 52);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(47, 21);
			this.button1.TabIndex = 17;
			this.button1.Text = "设置";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(247, 7);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 15);
			this.label5.TabIndex = 15;
			this.label5.Text = "温度:";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.radioButton21);
			this.groupBox5.Controls.Add(this.radioButton20);
			this.groupBox5.Controls.Add(this.radioButton19);
			this.groupBox5.Controls.Add(this.radioButton18);
			this.groupBox5.Controls.Add(this.radioButton17);
			this.groupBox5.Controls.Add(this.radioButton16);
			this.groupBox5.Controls.Add(this.radioButton15);
			this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox5.Location = new System.Drawing.Point(353, 186);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(100, 193);
			this.groupBox5.TabIndex = 14;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "房间状态";
			// 
			// radioButton21
			// 
			this.radioButton21.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton21.Location = new System.Drawing.Point(13, 162);
			this.radioButton21.Name = "radioButton21";
			this.radioButton21.Size = new System.Drawing.Size(74, 22);
			this.radioButton21.TabIndex = 6;
			this.radioButton21.Text = "空房";
			this.radioButton21.Click += new System.EventHandler(this.radioButton21_Click);
			// 
			// radioButton20
			// 
			this.radioButton20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton20.Location = new System.Drawing.Point(13, 138);
			this.radioButton20.Name = "radioButton20";
			this.radioButton20.Size = new System.Drawing.Size(74, 23);
			this.radioButton20.TabIndex = 5;
			this.radioButton20.Text = "预订";
			this.radioButton20.Click += new System.EventHandler(this.radioButton20_Click);
			// 
			// radioButton19
			// 
			this.radioButton19.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton19.Location = new System.Drawing.Point(13, 115);
			this.radioButton19.Name = "radioButton19";
			this.radioButton19.Size = new System.Drawing.Size(74, 22);
			this.radioButton19.TabIndex = 4;
			this.radioButton19.Text = "维修";
			this.radioButton19.Click += new System.EventHandler(this.radioButton19_Click);
			// 
			// radioButton18
			// 
			this.radioButton18.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton18.Location = new System.Drawing.Point(13, 92);
			this.radioButton18.Name = "radioButton18";
			this.radioButton18.Size = new System.Drawing.Size(74, 22);
			this.radioButton18.TabIndex = 3;
			this.radioButton18.Text = "退房";
			this.radioButton18.Click += new System.EventHandler(this.radioButton18_Click);
			// 
			// radioButton17
			// 
			this.radioButton17.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton17.Location = new System.Drawing.Point(13, 69);
			this.radioButton17.Name = "radioButton17";
			this.radioButton17.Size = new System.Drawing.Size(74, 22);
			this.radioButton17.TabIndex = 2;
			this.radioButton17.Text = "待租";
			this.radioButton17.Click += new System.EventHandler(this.radioButton17_Click);
			// 
			// radioButton16
			// 
			this.radioButton16.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton16.Location = new System.Drawing.Point(13, 45);
			this.radioButton16.Name = "radioButton16";
			this.radioButton16.Size = new System.Drawing.Size(74, 23);
			this.radioButton16.TabIndex = 1;
			this.radioButton16.Text = "已租";
			this.radioButton16.Click += new System.EventHandler(this.radioButton16_Click);
			// 
			// radioButton15
			// 
			this.radioButton15.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton15.Location = new System.Drawing.Point(13, 22);
			this.radioButton15.Name = "radioButton15";
			this.radioButton15.Size = new System.Drawing.Size(74, 23);
			this.radioButton15.TabIndex = 0;
			this.radioButton15.Text = "清洁中";
			this.radioButton15.Click += new System.EventHandler(this.radioButton15_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.radioButton23);
			this.groupBox6.Controls.Add(this.radioButton22);
			this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox6.Location = new System.Drawing.Point(105, 82);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(73, 89);
			this.groupBox6.TabIndex = 7;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "空调开关";
			// 
			// radioButton23
			// 
			this.radioButton23.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton23.Location = new System.Drawing.Point(13, 59);
			this.radioButton23.Name = "radioButton23";
			this.radioButton23.Size = new System.Drawing.Size(34, 23);
			this.radioButton23.TabIndex = 1;
			this.radioButton23.Text = "开";
			this.radioButton23.Click += new System.EventHandler(this.radioButton23_Click);
			// 
			// radioButton22
			// 
			this.radioButton22.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton22.Location = new System.Drawing.Point(13, 22);
			this.radioButton22.Name = "radioButton22";
			this.radioButton22.Size = new System.Drawing.Size(34, 23);
			this.radioButton22.TabIndex = 0;
			this.radioButton22.Text = "关";
			this.radioButton22.Click += new System.EventHandler(this.radioButton22_Click);
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.radioButton25);
			this.groupBox7.Controls.Add(this.radioButton24);
			this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox7.Location = new System.Drawing.Point(197, 82);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(73, 89);
			this.groupBox7.TabIndex = 8;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "季节";
			// 
			// radioButton25
			// 
			this.radioButton25.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton25.Location = new System.Drawing.Point(13, 59);
			this.radioButton25.Name = "radioButton25";
			this.radioButton25.Size = new System.Drawing.Size(59, 23);
			this.radioButton25.TabIndex = 1;
			this.radioButton25.Text = "冬季";
			this.radioButton25.Click += new System.EventHandler(this.radioButton25_Click);
			// 
			// radioButton24
			// 
			this.radioButton24.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton24.Location = new System.Drawing.Point(13, 22);
			this.radioButton24.Name = "radioButton24";
			this.radioButton24.Size = new System.Drawing.Size(51, 23);
			this.radioButton24.TabIndex = 0;
			this.radioButton24.Text = "夏季";
			this.radioButton24.Click += new System.EventHandler(this.radioButton24_Click);
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.radioButton27);
			this.groupBox8.Controls.Add(this.radioButton26);
			this.groupBox8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox8.Location = new System.Drawing.Point(288, 82);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(74, 89);
			this.groupBox8.TabIndex = 9;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "ATM故障";
			// 
			// radioButton27
			// 
			this.radioButton27.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton27.Location = new System.Drawing.Point(7, 59);
			this.radioButton27.Name = "radioButton27";
			this.radioButton27.Size = new System.Drawing.Size(60, 23);
			this.radioButton27.TabIndex = 1;
			this.radioButton27.Text = "故障排除";
			this.radioButton27.Click += new System.EventHandler(this.radioButton27_Click);
			// 
			// radioButton26
			// 
			this.radioButton26.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton26.Location = new System.Drawing.Point(7, 22);
			this.radioButton26.Name = "radioButton26";
			this.radioButton26.Size = new System.Drawing.Size(57, 23);
			this.radioButton26.TabIndex = 0;
			this.radioButton26.Text = "产生故障";
			this.radioButton26.Click += new System.EventHandler(this.radioButton26_Click);
			// 
			// groupBox9
			// 
			this.groupBox9.Controls.Add(this.radioButton29);
			this.groupBox9.Controls.Add(this.radioButton28);
			this.groupBox9.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox9.Location = new System.Drawing.Point(380, 82);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Size = new System.Drawing.Size(73, 89);
			this.groupBox9.TabIndex = 10;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "紧急状况";
			// 
			// radioButton29
			// 
			this.radioButton29.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton29.Location = new System.Drawing.Point(13, 59);
			this.radioButton29.Name = "radioButton29";
			this.radioButton29.Size = new System.Drawing.Size(47, 23);
			this.radioButton29.TabIndex = 1;
			this.radioButton29.Text = "取消";
			this.radioButton29.Click += new System.EventHandler(this.radioButton29_Click);
			// 
			// radioButton28
			// 
			this.radioButton28.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton28.Location = new System.Drawing.Point(13, 22);
			this.radioButton28.Name = "radioButton28";
			this.radioButton28.Size = new System.Drawing.Size(47, 23);
			this.radioButton28.TabIndex = 0;
			this.radioButton28.Text = "产生";
			this.radioButton28.Click += new System.EventHandler(this.radioButton28_Click);
			// 
			// groupBox10
			// 
			this.groupBox10.Controls.Add(this.radioButton30);
			this.groupBox10.Controls.Add(this.radioButton31);
			this.groupBox10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox10.Location = new System.Drawing.Point(464, 80);
			this.groupBox10.Name = "groupBox10";
			this.groupBox10.Size = new System.Drawing.Size(73, 89);
			this.groupBox10.TabIndex = 11;
			this.groupBox10.TabStop = false;
			this.groupBox10.Text = "断网";
			// 
			// radioButton30
			// 
			this.radioButton30.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton30.Location = new System.Drawing.Point(13, 59);
			this.radioButton30.Name = "radioButton30";
			this.radioButton30.Size = new System.Drawing.Size(47, 23);
			this.radioButton30.TabIndex = 1;
			this.radioButton30.Text = "取消";
			this.radioButton30.CheckedChanged += new System.EventHandler(this.radioButton30_CheckedChanged);
			// 
			// radioButton31
			// 
			this.radioButton31.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton31.Location = new System.Drawing.Point(13, 22);
			this.radioButton31.Name = "radioButton31";
			this.radioButton31.Size = new System.Drawing.Size(47, 23);
			this.radioButton31.TabIndex = 0;
			this.radioButton31.Text = "产生";
			this.radioButton31.CheckedChanged += new System.EventHandler(this.radioButton31_CheckedChanged);
			// 
			// groupBox11
			// 
			this.groupBox11.Controls.Add(this.radioButton33);
			this.groupBox11.Controls.Add(this.radioButton34);
			this.groupBox11.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox11.Location = new System.Drawing.Point(464, 184);
			this.groupBox11.Name = "groupBox11";
			this.groupBox11.Size = new System.Drawing.Size(73, 89);
			this.groupBox11.TabIndex = 12;
			this.groupBox11.TabStop = false;
			this.groupBox11.Text = "保险箱";
			// 
			// radioButton33
			// 
			this.radioButton33.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton33.Location = new System.Drawing.Point(13, 59);
			this.radioButton33.Name = "radioButton33";
			this.radioButton33.Size = new System.Drawing.Size(47, 23);
			this.radioButton33.TabIndex = 1;
			this.radioButton33.Text = "关";
			this.radioButton33.CheckedChanged += new System.EventHandler(this.radioButton33_CheckedChanged);
			// 
			// radioButton34
			// 
			this.radioButton34.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton34.Location = new System.Drawing.Point(13, 22);
			this.radioButton34.Name = "radioButton34";
			this.radioButton34.Size = new System.Drawing.Size(47, 23);
			this.radioButton34.TabIndex = 0;
			this.radioButton34.Text = "开";
			this.radioButton34.CheckedChanged += new System.EventHandler(this.radioButton34_CheckedChanged);
			// 
			// groupBox12
			// 
			this.groupBox12.Controls.Add(this.radioButton35);
			this.groupBox12.Controls.Add(this.radioButton36);
			this.groupBox12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox12.Location = new System.Drawing.Point(464, 288);
			this.groupBox12.Name = "groupBox12";
			this.groupBox12.Size = new System.Drawing.Size(73, 89);
			this.groupBox12.TabIndex = 13;
			this.groupBox12.TabStop = false;
			this.groupBox12.Text = "冰箱";
			// 
			// radioButton35
			// 
			this.radioButton35.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton35.Location = new System.Drawing.Point(13, 59);
			this.radioButton35.Name = "radioButton35";
			this.radioButton35.Size = new System.Drawing.Size(47, 23);
			this.radioButton35.TabIndex = 1;
			this.radioButton35.Text = "关";
			this.radioButton35.CheckedChanged += new System.EventHandler(this.radioButton35_CheckedChanged);
			// 
			// radioButton36
			// 
			this.radioButton36.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioButton36.Location = new System.Drawing.Point(13, 22);
			this.radioButton36.Name = "radioButton36";
			this.radioButton36.Size = new System.Drawing.Size(47, 23);
			this.radioButton36.TabIndex = 0;
			this.radioButton36.Text = "开";
			this.radioButton36.CheckedChanged += new System.EventHandler(this.radioButton36_CheckedChanged);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(539, 480);
			this.Controls.Add(this.groupBox9);
			this.Controls.Add(this.groupBox8);
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.temp);
			this.Controls.Add(this.txtRoomNumber);
			this.Controls.Add(this.txtFloorNumber);
			this.Controls.Add(this.txtCmd);
			this.Controls.Add(this.txtFileName);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSetInfo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Send);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox10);
			this.Controls.Add(this.groupBox11);
			this.Controls.Add(this.groupBox12);
			this.Name = "Form1";
			this.Text = "VirtualATM 1.0";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox9.ResumeLayout(false);
			this.groupBox10.ResumeLayout(false);
			this.groupBox11.ResumeLayout(false);
			this.groupBox12.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles();
			Application.Run(new Form1());
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void Send_Click(object sender, System.EventArgs e)
		{
			StreamWriter FileWriter=new StreamWriter(txtFileName.Text,true);
			FileWriter.WriteLine(txtCmd.Text);
			FileWriter.Close();
			txtCmd.Text="";
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			/*aSetting["FileName"]="D:\\room.inout";
			aFloorSet.Add(new Floor(10,20));
			aFloorSet.Add(new Floor(11,30));
			aAdapter=new AdapterTriggerFileText(aFloorSet,aSetting);
			aAdapter.Connect();*/
		}

		

		private void radioButton1_Click(object sender, System.EventArgs e)
		{
			SendCommand("Speed 0");
		}

		private void radioButton2_Click(object sender, System.EventArgs e)
		{
			SendCommand("Speed 1");
		}

		private void radioButton3_Click(object sender, System.EventArgs e)
		{
			SendCommand("Speed 2");
		}

		private void radioButton4_Click(object sender, System.EventArgs e)
		{
			SendCommand("Speed 3");
		}


		private void radioButton7_Click(object sender, System.EventArgs e)
		{
			SendCommand("Clean");
		}

		private void radioButton8_Click(object sender, System.EventArgs e)
		{
			SendCommand("DontDisturb");
		}

		private void radioButton9_Click(object sender, System.EventArgs e)
		{
			SendCommand("Call");
			
		}

		private void radioButton10_Click(object sender, System.EventArgs e)
		{
			SendCommand("NoService");
		}

		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
		
		}

		private void label2_Click(object sender, System.EventArgs e)
		{
		
		}

		private void radioButton5_Click(object sender, System.EventArgs e)
		{
			SendCommand("KeyPullOut");
		}

		private void radioButton6_Click(object sender, System.EventArgs e)
		{
			SendCommand("KeyInsertGuest");
		}

		private void radioButton13_Click(object sender, System.EventArgs e)
		{
			SendCommand("KeyInsertServant");
		}

		private void radioButton14_Click(object sender, System.EventArgs e)
		{
			SendCommand("KeyInsertLeader");
		}

		private void radioButton11_Click(object sender, System.EventArgs e)
		{
			SendCommand("DoorClose");
		}

		private void radioButton12_Click(object sender, System.EventArgs e)
		{
			SendCommand("DoorOpen");
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			SendCommand("Temperature"+" "+temp.Text);
		}

		private void radioButton15_Click(object sender, System.EventArgs e)
		{
			SendCommand("Cleaning");
		}

		private void radioButton16_Click(object sender, System.EventArgs e)
		{
			SendCommand("Rented");
		}

		private void radioButton17_Click(object sender, System.EventArgs e)
		{
			SendCommand("Vacant");
		}

		private void radioButton19_Click(object sender, System.EventArgs e)
		{
			SendCommand("Maintanent");
		}

		private void radioButton18_Click(object sender, System.EventArgs e)
		{
			SendCommand("CheckOut");
		}

		private void radioButton20_Click(object sender, System.EventArgs e)
		{
			SendCommand("Booked");
		}

		private void radioButton21_Click(object sender, System.EventArgs e)
		{
			SendCommand("Empty");
		}

		private void radioButton22_Click(object sender, System.EventArgs e)
		{
			SendCommand("AirConditionerOff");
		}

		private void radioButton23_Click(object sender, System.EventArgs e)
		{
			SendCommand("AirConditionerOn");
		}

		private void radioButton24_Click(object sender, System.EventArgs e)
		{
			SendCommand("Season"+" 1");
		}

		private void radioButton25_Click(object sender, System.EventArgs e)
		{
			SendCommand("Season"+" 3");
		}

		private void radioButton26_Click(object sender, System.EventArgs e)
		{
			SendCommand("ProblemCaused");

		}

		private void radioButton27_Click(object sender, System.EventArgs e)
		{
			SendCommand("ProblemRepaired");
		}

		private void radioButton28_Click(object sender, System.EventArgs e)
		{
			SendCommand("Emergency");

		}

		private void radioButton29_Click(object sender, System.EventArgs e)
		{
			SendCommand("EmergencyCancel");

		}

		private void radioButton31_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("Disconnect");
		}

		private void radioButton30_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("Connect");
		}

		private void radioButton32_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("QuitRoom");
		}

		private void radioButton34_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("CofferOpen");
		}

		private void radioButton33_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("CofferClose");
		}

		private void radioButton36_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("RefrigeratorOpen");
		}

		private void radioButton35_CheckedChanged(object sender, System.EventArgs e)
		{
			SendCommand("RefrigeratorClose");
		}


	
	}	
}
