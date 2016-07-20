/*
* Created by SharpDevelop.
* User: Michal
* Date: 7/12/2016
* Time: 8:38 PM
* 
* To change this template use Tools | Options | Coding | Edit Standard Headers.
*/
using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Security.Cryptography;
using System.Text;

namespace CH340_interface
{
/// <summary>
/// Simple terminal interface, scans for ports, once connected allows
/// user to connect to device, set parameters such as baud rate, addresses,
/// etc, and send/recieve messages
/// added encryption using DES, if want RSA use the link below, more complicated
/// http://stackoverflow.com/questions/17128038/c-sharp-rsa-encryption-decryption-with-transmission
/// </summary>
public partial class MainForm : Form
{
	private string[] ports;
	private static SerialPort COMport = null;
	public static bool connect_ready = false;
	public static bool connected = false;
	
	public static int baud;
	public static int rate;
	public static int freq;
	public static string rxadd;
	public static string txadd;
	public static int AT_CMD_MODE;
	
	public MainForm()
	{
		InitializeComponent();
		
		portcombobox.Enabled = false;
		baudcombobox.Enabled = false;
		ratecombobox.Enabled = false;
		freqcombobox.Enabled = false;
		sendbtn.Enabled = false;
		setbtn.Enabled = false;
		txtextbox.Enabled = false;
		usernametxtbox.Enabled = true;
		passwordbox.Enabled = true;
		cryptoOPT.Enabled = true;
		
		/*
		* need to populate serial ports in dropdown when first starting
		* EDIT: removed duplicate code, now it just calls the rescan
		* button, which does the exact same thing
		*/
		RescanbtnClick(this, new EventArgs());
	}
		
	/*
	* This is needed to check whether it's already connected or not, 
	* or if the connection is ready. Some of the args from the dropdowns
	* cant be used directly but some can, so just do a little magic
	* to either extract the index number+1 (since device has non-zero
	* indeces) or the actual value such as the frequency
	* Button will toggle
	* First thing it checks for is if the password used is less than 8
	* characters, if it is, it pops a message and tells you to use a better password.
	* Program should not connect if this is the case. But luckily it only does this when
	* the DES option is enabled.
	*/
	void Button1Click(object sender, EventArgs e)
	{
		if(cryptoOPT.CheckState == CheckState.Checked && passwordbox.Text.Length < 8) {
			MessageBox.Show("Password must be a minimum of 8 characters", "ENCRYPTION ON", MessageBoxButtons.OK);
			return;
		}
		if(!connected && connect_ready) {
//			connected = true;
//			connectbtn.Text = "Disconnect";
//			baud = baudcombobox.SelectedIndex;
			
			//check if any null
			if(baud == -1) baud = 2;
			
			int baudr;
			Int32.TryParse(baudcombobox.Items[baud].ToString(), out baudr);
			if(connect_ready) {
				connected = true;
				connectbtn.Text = "Disconnect";
				baud = baudcombobox.SelectedIndex;
				usernametxtbox.Enabled = false;
				passwordbox.Enabled = false;
				cryptoOPT.Enabled = false;

				try {
					COMport = new SerialPort(portcombobox.Text, baudr);
					COMport.BaudRate = baudr;
					COMport.Parity = Parity.None;
					COMport.StopBits = StopBits.One;
					COMport.DataBits = 8;
					COMport.Handshake = Handshake.None;
					
					COMport.Open();
					COMport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
					//COMport.WriteLine("AT?\r\n");
					
					sendbtn.Enabled = true;
					setbtn.Enabled = true;
					txtextbox.Enabled = true;
				} catch (Exception ex) {
					rxtextbox.AppendText("error opening com port: "+ex.Message+"\r\n");
					connected = false;
					connectbtn.Text = "Connect";
				}
			
			}
		} else if(connected) {
				connected = false;
				connectbtn.Text = "Connect";
				//connect_ready = false;
				COMport.Close();
				sendbtn.Enabled = false;
				setbtn.Enabled = false;
				txtextbox.Enabled = false;
				usernametxtbox.Enabled = true;
				passwordbox.Enabled = true;
				cryptoOPT.Enabled = true;
			}
		}
		
		/*
		* Text in box needs to be converted when doing AT commands,
		* and since I'm lazy I just make everything uppercase
		* and terminate with a carriage return and new line
		* The textbox has no history function either
		* 0 indicates local for AT commands,
		* 1 indicates to actually send to someone, used for DES encryption
		*/
		void Button2Click(object sender, EventArgs e)
		{
			if((txtextbox.Text.ToUpper().Contains("AT?") && txtextbox.Text.Length <= "AT?".Length) || (txtextbox.Text.ToUpper().Contains("AT+") && txtextbox.Text.Length <= "AT+FREQ=2.400G".Length)) {
//				COMport.WriteLine(txtextbox.Text.ToUpper()+"\r\n");
				SendText(txtextbox.Text.ToUpper(), 0);
			} else {
//				COMport.WriteLine(usernametxtbox.Text+":> "+txtextbox.Text+"\r\n");
				SendText(txtextbox.Text, 1);
			}
			txtextbox.Clear();
		}
		
		/*
		* if no com port then disable other boxes
		*/
		void PortChoose(object sender, EventArgs e)
		{
			if(portcombobox.SelectedIndex != -1) {
				connectbtn.Enabled = true;
				baudcombobox.Enabled = true;
				ratecombobox.Enabled = true;
				freqcombobox.Enabled = true;
				connect_ready = true;
			} else {
				connectbtn.Enabled = false;
				baudcombobox.Enabled = false;
				ratecombobox.Enabled = false;
				freqcombobox.Enabled = false;
				COMport = null;
				connect_ready = false;
			}
		}
		
		/*
		* only allows HEX characters in the address textboxes since
		* we dont want illegal characters or screw ups
		* The setting function parses this data out anyway
		*/
		void textBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)
				&& (e.KeyChar != 'a') && (e.KeyChar != 'b') && (e.KeyChar != 'c') && (e.KeyChar != 'd') 
				&& (e.KeyChar != 'e') && (e.KeyChar != 'f');
		}
		
		/*
		* pressing enter in the text box is the same as
		* clicking the Tx button
		*/
		void enter_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)Keys.Enter) Button2Click(this, new EventArgs());
		}
		
		/*
		* simple handler for the serial port needed since data
		* will come asynchronously and needs to be written to
		* the Rx textbox
		* it's a little messy since apparantely accessing the rx textbox
		* from another thread breaks the universe so get around it
		* by using a delegate, whatever that is
		*/
		private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
		{
			SerialPort sp = (SerialPort)sender;
			SetText(sp.ReadExisting());
		}
		
		delegate void SetTextCallback(string text);
		
		/*
		* needed for preventing the whole thing from breaking
		* I don't actually know how this works or what it's doing
		* exactly, but I think it's making a new object from 
		* the received text, then safely passing it to the 
		* textbox? I dunno
		* Since adding encryption, it has to look for the AT command
		* more precisely, which uses the AT flag set by the SendText method,
		* otherwise it all breaks.
		*/
		private void SetText(string text)
		{
			if(this.rxtextbox.InvokeRequired) {
				SetTextCallback d = new SetTextCallback(SetText);
				this.Invoke(d, new object[] { text });
			} else {
				if(cryptoOPT.CheckState == CheckState.Checked && AT_CMD_MODE == 0) {
					this.rxtextbox.AppendText("DES--> "+Decrypt(text, passwordbox.Text));
				} else if(AT_CMD_MODE == 1) {
					this.rxtextbox.AppendText(text);
				} else {
					this.rxtextbox.AppendText(text);
				}
			}
		}
		
		/*
		* needed a quick way to set a bunch of parameters at once
		* this seemed like the best way without having to
		* do a bunch of AT commands
		* Need 0.5sec between each one just so it can breathe
		* a little
		* also checks if fields are empty and uses defaults
		*/
		void SetbtnClick(object sender, EventArgs e)
		{
			rate = ratecombobox.SelectedIndex+1;
			freq = freqcombobox.SelectedIndex;
			
			if(string.IsNullOrEmpty(rxaddr.Text)) rxadd = "0xff,0xff,0xff,0xff,0xff";
				else rxadd = "0x"+rxaddr.Text[0]+rxaddr.Text[1]+",0x"+rxaddr.Text[2]+rxaddr.Text[3]+",0x"+rxaddr.Text[4]+rxaddr.Text[5]+",0x"+rxaddr.Text[6]+rxaddr.Text[7]+",0x"+rxaddr.Text[8]+rxaddr.Text[9];
			if(string.IsNullOrEmpty(txaddr.Text)) txadd = "0xff,0xff,0xff,0xff,0xff";
				else txadd = "0x"+txaddr.Text[0]+txaddr.Text[1]+",0x"+txaddr.Text[2]+txaddr.Text[3]+",0x"+txaddr.Text[4]+txaddr.Text[5]+",0x"+txaddr.Text[6]+txaddr.Text[7]+",0x"+txaddr.Text[8]+txaddr.Text[9];
			
			if(rate == -1) rate = 2;
			if(freq == -1) freq = 0;
			
			rxtextbox.AppendText("Setting\r\n");
//			COMport.WriteLine("AT+RATE="+rate+"\r\n");
			SendText("AT+RATE="+rate,0);
			Thread.Sleep(500);
//			COMport.WriteLine("AT+FREQ="+freqcombobox.Items[freq]+"G\r\n");
			SendText("AT+FREQ="+freqcombobox.Items[freq]+"G",0);
			Thread.Sleep(500);
//			COMport.WriteLine("AT+RXA="+rxadd+"\r\n");
			SendText("AT+RXA="+rxadd,0);
			Thread.Sleep(500);
//			COMport.WriteLine("AT+TXA="+txadd+"\r\n");
			SendText("AT+TXA="+txadd,0);
		}
		
		/*
		* cant connect without first scanning the ports and
		* populating the device list
		* tried using a usb library to actually claim and
		* control the usb interface, but apparently these
		* devices aren't visible as usb devices and are
		* instead serial com ports, so that simplifies
		* things a lot
		* this app does also check if the com port is
		* busy or not and tells you, so if you run
		* multiple instances of this you'll see the
		* device but shouldn't be able to connect
		*/
		void RescanbtnClick(object sender, EventArgs e)
		{
			ports = SerialPort.GetPortNames();
			foreach(string port in ports) {
				if(!portcombobox.Items.Contains(port)) portcombobox.Items.Add(port);
			}
			portcombobox.Enabled = true;
		}
		
		/*
		 * needed to make a standard way of sending text
		 * this takes the input text and depending on security
		 * it will either encrypt it or not
		 * it checks for AT commands vs actual text
		 * Actual text will always go through encryption (if chosen)
		 * and will transmit
		 * AT commands need a flag for the delegate text handler or 
		 * it breaks
		 * The type of text coming in will determine if it's AT or text
		 * textmode = 1 when sending text
		 * textmode = 0 when sending AT commands
		 */
		void SendText(string text, int textmode)
		{
			if(textmode == 0 && cryptoOPT.CheckState == CheckState.Unchecked) {
				AT_CMD_MODE = 1;
				COMport.WriteLine(text+"\r\n");
				this.rxtextbox.AppendText(text+"\r\n");
				Console.WriteLine("non crypto, unchecked");
			} else if(textmode == 0 && cryptoOPT.CheckState == CheckState.Checked) {
				AT_CMD_MODE = 1;
				COMport.WriteLine(text+"\r\n");
				this.rxtextbox.AppendText(text+"\r\n");
				Console.WriteLine("non crypto, checked");
			} else if(textmode == 1 && cryptoOPT.CheckState == CheckState.Checked) {
				AT_CMD_MODE = 0;
				var enc_text = Encrypt(usernametxtbox.Text+":> "+text, passwordbox.Text);
//				var enc_text = Encrypt(text, passwordbox.Text);
//				COMport.WriteLine(usernametxtbox.Text+":> "+enc_text+"\r\n");
				COMport.WriteLine(enc_text+"\r\n");
				this.rxtextbox.AppendText(usernametxtbox.Text+":> "+text+"\r\n");
				this.rxtextbox.AppendText("DES<-- "+enc_text+"\r\n");
			} else if(textmode == 1 && cryptoOPT.CheckState == CheckState.Unchecked) {
				AT_CMD_MODE = 0;
				COMport.WriteLine(usernametxtbox.Text+":> "+text+"\r\n");
				this.rxtextbox.AppendText(usernametxtbox.Text+":> "+text+"\r\n");
			}
		}
		
		/*
		 * DES encryption using a password as the key and init vector
		 * This password is what's shared between talking users so they
		 * can decrypt messages. It MUST be minimum 8 characters or this
		 * breaks. It can be more than 8 but only the first 8 are actually used
		 */
		public static string Encrypt(string message, string password)
		{
			// Encode message and password
			byte[] messageBytes = ASCIIEncoding.ASCII.GetBytes(message);
			byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password.Substring(0, 8));
			
			// Set encryption settings -- Use password for both key and init. vector
			var provider = new DESCryptoServiceProvider();
			ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
			CryptoStreamMode mode = CryptoStreamMode.Write;
			
			// Set up streams and encrypt
			var memStream = new MemoryStream();
			var cryptoStream = new CryptoStream(memStream, transform, mode);
			cryptoStream.Write(messageBytes, 0, messageBytes.Length);
			cryptoStream.FlushFinalBlock();
			
			// Read the encrypted message from the memory stream
			var encryptedMessageBytes = new byte[memStream.Length];
			memStream.Position = 0;
			memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
			
			// Encode the encrypted message as base64 string
			string encryptedMessage = Convert.ToBase64String(encryptedMessageBytes);
			
			return encryptedMessage; 
		}
		
		/*
		 * DES decryptor which behaves like the encryptor
		 * except backwards
		 */
		public static string Decrypt(string encryptedMessage, string password)
		{
			// Convert encrypted message and password to bytes
			byte[] encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
			byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(password.Substring(0, 8));
			
			// Set encryption settings -- Use password for both key and init. vector
			var provider = new DESCryptoServiceProvider();
			ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
			CryptoStreamMode mode = CryptoStreamMode.Write;
			
			// Set up streams and decrypt
			var memStream = new MemoryStream();
			var cryptoStream = new CryptoStream(memStream, transform, mode);
			cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
			cryptoStream.FlushFinalBlock();
			
			// Read decrypted message from memory stream
			var decryptedMessageBytes = new byte[memStream.Length];
			memStream.Position = 0;
			memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);
			
			// Encode deencrypted binary data to base64 string
			string message = ASCIIEncoding.ASCII.GetString(decryptedMessageBytes);
			
			return message;
		}
	}
}
