/*
 * Created by SharpDevelop.
 * User: Michal
 * Date: 7/12/2016
 * Time: 8:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace CH340Lin
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox portcombobox;
		private System.Windows.Forms.ComboBox baudcombobox;
		private System.Windows.Forms.Button connectbtn;
		private System.Windows.Forms.Button sendbtn;
		private System.Windows.Forms.TextBox txtextbox;
		private System.Windows.Forms.TextBox rxtextbox;
		private System.Windows.Forms.ComboBox ratecombobox;
		private System.Windows.Forms.ComboBox freqcombobox;
		private System.Windows.Forms.TextBox rxaddr;
		private System.Windows.Forms.TextBox txaddr;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button setbtn;
		private System.Windows.Forms.Button rescanbtn;
		private System.Windows.Forms.TextBox usernametxtbox;
		private System.Windows.Forms.CheckBox cryptoOPT;
		private System.Windows.Forms.TextBox passwordbox;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.portcombobox = new System.Windows.Forms.ComboBox();
			this.baudcombobox = new System.Windows.Forms.ComboBox();
			this.connectbtn = new System.Windows.Forms.Button();
			this.sendbtn = new System.Windows.Forms.Button();
			this.txtextbox = new System.Windows.Forms.TextBox();
			this.rxtextbox = new System.Windows.Forms.TextBox();
			this.ratecombobox = new System.Windows.Forms.ComboBox();
			this.freqcombobox = new System.Windows.Forms.ComboBox();
			this.rxaddr = new System.Windows.Forms.TextBox();
			this.txaddr = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.setbtn = new System.Windows.Forms.Button();
			this.rescanbtn = new System.Windows.Forms.Button();
			this.usernametxtbox = new System.Windows.Forms.TextBox();
			this.cryptoOPT = new System.Windows.Forms.CheckBox();
			this.passwordbox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// portcombobox
			// 
			this.portcombobox.FormattingEnabled = true;
			this.portcombobox.Location = new System.Drawing.Point(12, 42);
			this.portcombobox.Name = "portcombobox";
			this.portcombobox.Size = new System.Drawing.Size(76, 21);
			this.portcombobox.TabIndex = 0;
			this.portcombobox.Text = "Port";
			this.portcombobox.SelectedIndexChanged += new System.EventHandler(this.PortChoose);
			// 
			// baudcombobox
			// 
			this.baudcombobox.FormattingEnabled = true;
			this.baudcombobox.Items.AddRange(new object[] {
			"4800",
			"9600",
			"14400",
			"19200",
			"38400",
			"115200"});
			this.baudcombobox.Location = new System.Drawing.Point(12, 69);
			this.baudcombobox.Name = "baudcombobox";
			this.baudcombobox.Size = new System.Drawing.Size(76, 21);
			this.baudcombobox.TabIndex = 1;
			this.baudcombobox.Text = "Baud";
			// 
			// connectbtn
			// 
			this.connectbtn.Location = new System.Drawing.Point(12, 13);
			this.connectbtn.Name = "connectbtn";
			this.connectbtn.Size = new System.Drawing.Size(76, 23);
			this.connectbtn.TabIndex = 2;
			this.connectbtn.Text = "Connect";
			this.connectbtn.UseVisualStyleBackColor = true;
			this.connectbtn.Click += new System.EventHandler(this.Button1Click);
			// 
			// sendbtn
			// 
			this.sendbtn.Location = new System.Drawing.Point(12, 206);
			this.sendbtn.Name = "sendbtn";
			this.sendbtn.Size = new System.Drawing.Size(76, 23);
			this.sendbtn.TabIndex = 3;
			this.sendbtn.Text = "Send";
			this.sendbtn.UseVisualStyleBackColor = true;
			this.sendbtn.Click += new System.EventHandler(this.Button2Click);
			// 
			// txtextbox
			// 
			this.txtextbox.Location = new System.Drawing.Point(95, 206);
			this.txtextbox.MaxLength = 128;
			this.txtextbox.Name = "txtextbox";
			this.txtextbox.Size = new System.Drawing.Size(377, 20);
			this.txtextbox.TabIndex = 4;
			this.txtextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.enter_KeyPress);
			// 
			// rxtextbox
			// 
			this.rxtextbox.Location = new System.Drawing.Point(95, 13);
			this.rxtextbox.Multiline = true;
			this.rxtextbox.Name = "rxtextbox";
			this.rxtextbox.ReadOnly = true;
			this.rxtextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.rxtextbox.Size = new System.Drawing.Size(377, 133);
			this.rxtextbox.TabIndex = 5;
			// 
			// ratecombobox
			// 
			this.ratecombobox.FormattingEnabled = true;
			this.ratecombobox.Items.AddRange(new object[] {
			"250kbps",
			"1Mbps",
			"2Mbps"});
			this.ratecombobox.Location = new System.Drawing.Point(251, 151);
			this.ratecombobox.Name = "ratecombobox";
			this.ratecombobox.Size = new System.Drawing.Size(75, 21);
			this.ratecombobox.TabIndex = 6;
			this.ratecombobox.Text = "Rate";
			// 
			// freqcombobox
			// 
			this.freqcombobox.FormattingEnabled = true;
			this.freqcombobox.Items.AddRange(new object[] {
			"2.400",
			"2.401",
			"2.402",
			"2.403",
			"2.404",
			"2.405",
			"2.406",
			"2.407",
			"2.408",
			"2.409",
			"2.410",
			"2.411",
			"2.412",
			"2.413",
			"2.414",
			"2.415",
			"2.416",
			"2.417",
			"2.418",
			"2.419",
			"2.420",
			"2.421",
			"2.422",
			"2.423",
			"2.424",
			"2.425",
			"2.426",
			"2.427",
			"2.428",
			"2.429",
			"2.430",
			"2.431",
			"2.432",
			"2.433",
			"2.434",
			"2.435",
			"2.436",
			"2.437",
			"2.438",
			"2.439",
			"2.440",
			"2.441",
			"2.442",
			"2.443",
			"2.444",
			"2.445",
			"2.446",
			"2.447",
			"2.448",
			"2.449",
			"2.450",
			"2.451",
			"2.452",
			"2.453",
			"2.454",
			"2.455",
			"2.456",
			"2.457",
			"2.458",
			"2.459",
			"2.460",
			"2.461",
			"2.462",
			"2.463",
			"2.464",
			"2.465",
			"2.466",
			"2.467",
			"2.468",
			"2.469",
			"2.470",
			"2.471",
			"2.472",
			"2.473",
			"2.474",
			"2.475",
			"2.476",
			"2.477",
			"2.478",
			"2.479",
			"2.480",
			"2.481",
			"2.482",
			"2.483",
			"2.484",
			"2.485",
			"2.486",
			"2.487",
			"2.488",
			"2.489",
			"2.490",
			"2.491",
			"2.492",
			"2.493",
			"2.494",
			"2.495",
			"2.496",
			"2.497",
			"2.498",
			"2.499",
			"2.500",
			"2.501",
			"2.502",
			"2.503",
			"2.504",
			"2.505",
			"2.506",
			"2.507",
			"2.508",
			"2.509",
			"2.510",
			"2.511",
			"2.512",
			"2.513",
			"2.514",
			"2.515",
			"2.516",
			"2.517",
			"2.518",
			"2.519",
			"2.520",
			"2.521",
			"2.522",
			"2.523",
			"2.524",
			"2.525"});
			this.freqcombobox.Location = new System.Drawing.Point(251, 178);
			this.freqcombobox.Name = "freqcombobox";
			this.freqcombobox.Size = new System.Drawing.Size(75, 21);
			this.freqcombobox.TabIndex = 7;
			this.freqcombobox.Text = "Frequency";
			// 
			// rxaddr
			// 
			this.rxaddr.Location = new System.Drawing.Point(95, 152);
			this.rxaddr.MaxLength = 10;
			this.rxaddr.Name = "rxaddr";
			this.rxaddr.Size = new System.Drawing.Size(150, 20);
			this.rxaddr.TabIndex = 8;
			this.rxaddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
			// 
			// txaddr
			// 
			this.txaddr.Location = new System.Drawing.Point(95, 179);
			this.txaddr.MaxLength = 10;
			this.txaddr.Name = "txaddr";
			this.txaddr.Size = new System.Drawing.Size(150, 20);
			this.txaddr.TabIndex = 9;
			this.txaddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 23);
			this.label1.TabIndex = 10;
			this.label1.Text = "Rx Address";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 179);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 23);
			this.label2.TabIndex = 11;
			this.label2.Text = "Tx Address";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// setbtn
			// 
			this.setbtn.Location = new System.Drawing.Point(397, 152);
			this.setbtn.Name = "setbtn";
			this.setbtn.Size = new System.Drawing.Size(75, 23);
			this.setbtn.TabIndex = 12;
			this.setbtn.Text = "Set";
			this.setbtn.UseVisualStyleBackColor = true;
			this.setbtn.Click += new System.EventHandler(this.SetbtnClick);
			// 
			// rescanbtn
			// 
			this.rescanbtn.Location = new System.Drawing.Point(12, 97);
			this.rescanbtn.Name = "rescanbtn";
			this.rescanbtn.Size = new System.Drawing.Size(75, 23);
			this.rescanbtn.TabIndex = 13;
			this.rescanbtn.Text = "Rescan";
			this.rescanbtn.UseVisualStyleBackColor = true;
			this.rescanbtn.Click += new System.EventHandler(this.RescanbtnClick);
			// 
			// usernametxtbox
			// 
			this.usernametxtbox.Location = new System.Drawing.Point(333, 178);
			this.usernametxtbox.MaxLength = 8;
			this.usernametxtbox.Name = "usernametxtbox";
			this.usernametxtbox.Size = new System.Drawing.Size(58, 20);
			this.usernametxtbox.TabIndex = 14;
			this.usernametxtbox.Text = "user";
			// 
			// cryptoOPT
			// 
			this.cryptoOPT.Checked = true;
			this.cryptoOPT.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cryptoOPT.Location = new System.Drawing.Point(332, 151);
			this.cryptoOPT.Name = "cryptoOPT";
			this.cryptoOPT.Size = new System.Drawing.Size(59, 24);
			this.cryptoOPT.TabIndex = 15;
			this.cryptoOPT.Text = "DES";
			this.cryptoOPT.UseVisualStyleBackColor = true;
			// 
			// passwordbox
			// 
			this.passwordbox.Location = new System.Drawing.Point(398, 179);
			this.passwordbox.MaxLength = 32;
			this.passwordbox.Name = "passwordbox";
			this.passwordbox.PasswordChar = '*';
			this.passwordbox.Size = new System.Drawing.Size(74, 20);
			this.passwordbox.TabIndex = 16;
			this.passwordbox.Text = "pass";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 243);
			this.Controls.Add(this.passwordbox);
			this.Controls.Add(this.cryptoOPT);
			this.Controls.Add(this.usernametxtbox);
			this.Controls.Add(this.rescanbtn);
			this.Controls.Add(this.setbtn);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txaddr);
			this.Controls.Add(this.rxaddr);
			this.Controls.Add(this.freqcombobox);
			this.Controls.Add(this.ratecombobox);
			this.Controls.Add(this.rxtextbox);
			this.Controls.Add(this.txtextbox);
			this.Controls.Add(this.sendbtn);
			this.Controls.Add(this.connectbtn);
			this.Controls.Add(this.baudcombobox);
			this.Controls.Add(this.portcombobox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "wTerm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
