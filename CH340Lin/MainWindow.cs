using System;
using Gtk;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

public partial class MainWindow: Gtk.Window
{	
	private string[] ports;
	private static SerialPort serial_tty = new SerialPort();
	public static int baud_sel;
	public static int rate_sel;
	public static int freq_sel;
	public static string rxadd_sel;
	public static string txadd_sel;
	public static int AT_cmd_mode;
	public Thread readThread;

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		getCOMports (this, new EventArgs ());
		toggleFields (false);

		//readThread = new Thread (new ThreadStart (ReadThread));
		//readThread.Start ();
	}

	/*
	public void ReadThread()
	{
	SerialPort sp = new SerialPort (serial_tty.PortName, serial_tty.BaudRate, serial_tty.Parity, serial_tty.DataBits, serial_tty.StopBits);
		sp.Open ();
		while (serial_tty.IsOpen) {
			if (sp.BytesToRead > 0) {
				//Console.WriteLine (sp.ReadLine ());
				Gtk.Application.Invoke (delegate {
					SetText (sp.ReadExisting ());
				});
			}
		}
	}
	*/

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	public void toggleEncryptButton(object sender, EventArgs e)
	{
		if (encryptionOPT.Active == true) {
			desradio.Sensitive = true;
			aesradio.Sensitive = true;
			otherradio.Sensitive = true;
		} else {
			desradio.Sensitive = false;
			aesradio.Sensitive = false;
			otherradio.Sensitive = false;
		}
	}

	public void getCOMports(object sender, EventArgs e)
	{
		Console.WriteLine ("checking ports");
		//comportbox.Clear ();
		ListStore comportstore = new ListStore (typeof(string));
		comportbox.Model = comportstore;

		ports = SerialPort.GetPortNames ();
		foreach (string port in ports) {
			if (!port.Contains ("ttyS")) {
				Console.WriteLine ("ports: " + port.ToString ());
				comportstore.AppendValues (port);
			}
		}
	}

	public void connectOnClick(object sender, EventArgs e)
	{
		Console.WriteLine ("attempting to connect");
		if (serial_tty != null && !serial_tty.IsOpen) {
//			Int32.TryParse (baudratebox.ActiveText, out baud_sel);
//			if (baud_sel == 0)
//				baud_sel = 4800;
			if (comportbox.ActiveText == null)
				return;
			Console.WriteLine ("baud: " + baud_select());
			//connect to port
			serial_tty.PortName = comportbox.ActiveText;
			serial_tty.ReadBufferSize = 8192;
			serial_tty.ReadTimeout = 400;
			serial_tty.WriteBufferSize = 128;
			serial_tty.BaudRate = baud_select();
			serial_tty.Parity = Parity.None;
			serial_tty.StopBits = StopBits.One;
			serial_tty.DataBits = 8;
			serial_tty.Handshake = Handshake.None;

			serial_tty.DataReceived += new SerialDataReceivedEventHandler (datareceivedhandler);
			//serial_tty.DataReceived += datareceivedhandler;
			if (!serial_tty.IsOpen) {
				Console.WriteLine ("Opening port");
				serial_tty.Open ();
			} else {
				Console.WriteLine ("Closing port");
			}

			toggleFields (true);
		} else {
			serial_tty.Close ();
			toggleFields (false);
		}
	}

	public void toggleFields(bool on)
	{
		if (on) {
			sendbutton.Sensitive = true;
			setbutton.Sensitive = true;
			rxbox.Sensitive = true;
			txentry.Sensitive = true;
			rxaentry.Sensitive = true;
			txaentry.Sensitive = true;
			connectbutton.Label = "Disconnect";
		} else {
			sendbutton.Sensitive = false;
			setbutton.Sensitive = false;
			rxbox.Sensitive = false;
			txentry.Sensitive = false;
			rxaentry.Sensitive = false;
			txaentry.Sensitive = false;
			connectbutton.Label = "Connect";
		}
	}

	public void datareceivedhandler(object sender, SerialDataReceivedEventArgs e)
	{
		Console.WriteLine ("data received");
		SerialPort sp = (SerialPort)sender;
		SetText (sp.ReadExisting ());
	}

	private void SetText(string text)
	{
		Console.WriteLine (text);
		rxtext.Buffer.Text = text;
	}

	private void send_data(object sender, EventArgs e)
	{
		if ((txentry.Text.ToUpper ().Contains ("AT?") && txentry.Text.Length <= "AT?".Length) || (txentry.Text.ToUpper ().Contains ("AT+") && txentry.Text.Length <= "AT+FREQ=2.400G".Length)) {
			Sendtext (txentry.Text, 0);
		} else {
			Sendtext (txentry.Text, 1);
		}
		txentry.Text = "";
	}

	private int encMode()
	{
		return 1;
	}

	private void Sendtext(string text, int mode)
	{
		switch (mode) {
		case 0:
			{
				Console.WriteLine (text.ToUpper());
				serial_tty.WriteLine (text.ToUpper()+"\r\n");
				//serial_tty.Write (text.ToUpper()+"\r\n");
				rxtext.Buffer.Text = text.ToUpper();
			}
			break;
		case 1:
			{
				Console.WriteLine (text);
				serial_tty.WriteLine (text+"\r\n");
				//serial_tty.Write (text+"\r\n");
				rxtext.Buffer.Text = text;
			}
			break;
		case 2:
			{
				string enc_text = DESEncrypt (text, "12345678");
				serial_tty.WriteLine (enc_text);
				//serial_tty.Write (enc_text);
				rxtext.Buffer.Text = enc_text;
			}
			break;
		}
	}

	private void txarxaentryHandler(object sender, KeyPressEventArgs e)
	{

	}

	private void textEnterActivated(object sender, EventArgs e)
	{
		Console.WriteLine ("enter pressed");
		send_data (this, new EventArgs ());
	}


	/*
		 * DES encryption using a password as the key and init vector
		 * This password is what's shared between talking users so they
		 * can decrypt messages. It MUST be minimum 8 characters or this
		 * breaks. It can be more than 8 but only the first 8 are actually used
		 */
	public static string DESEncrypt(string message, string password)
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
	public static string DESDecrypt(string encryptedMessage, string password)
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

	protected void textCondition (object o, TextInsertedArgs args)
	{
		args.RetVal = !char.IsDigit (args.Text[0]) && !char.IsControl (args.Text[0])
			&& (args.Text[0] != 'a') && (args.Text[0] != 'b') && (args.Text[0] != 'c')
				&& (args.Text[0] != 'e') && (args.Text[0] != 'f');
	}

	protected void setProperties (object sender, EventArgs e)
	{
		Sendtext ("AT+BAUD="+baud_select()+"\r\n", 0);
		Thread.Sleep (500);

		if(bitratebox.Active != -1)
			Sendtext ("AT+RATE="+rate_select()+"\r\n", 0);
		else 
			Sendtext ("AT+RATE=1\r\n", 0);
		Thread.Sleep (500);

		if(frequencybox.Active != -1)
			Sendtext ("AT+FREQ="+frequencybox.ActiveText+"G\r\n", 0);
		else
			Sendtext ("AT+FREQ=2.400G\r\n", 0);
		Thread.Sleep (500);

		if (string.IsNullOrWhiteSpace (rxaentry.Text) || rxaentry.Text.Length < 10) {
			Sendtext ("AT+RXA=0xff,0xff,0xff,0xff,0xff\r\n", 0);
		} else if (rxaentry.Text.Length == 10) {
			Sendtext ("AT+RXA=0x" + rxaentry.Text.ToCharArray () [0] + rxaentry.Text.ToCharArray () [1] + ",0x" + rxaentry.Text.ToCharArray () [2] + rxaentry.Text.ToCharArray () [3] + ",0x" + rxaentry.Text.ToCharArray () [4] + rxaentry.Text.ToCharArray () [5] + ",0x" + rxaentry.Text.ToCharArray () [6] + rxaentry.Text.ToCharArray () [7] + ",0x" + rxaentry.Text.ToCharArray () [8] + rxaentry.Text.ToCharArray () [9] + "\r\n", 0);
		}
		else 
			ShowErrorBox (this, "Bad Rx Address", "Rx address must be minimum of 10 hex characters");
		Thread.Sleep (500);

		if (string.IsNullOrWhiteSpace (txaentry.Text) || txaentry.Text.Length < 10) {
			Sendtext ("AT+TXA=0xff,0xff,0xff,0xff,0xff\r\n", 0);
		} else if (txaentry.Text.Length == 10) {
			Sendtext ("AT+TXA=0x" + txaentry.Text.ToCharArray () [0] + txaentry.Text.ToCharArray () [1] + ",0x" + txaentry.Text.ToCharArray () [2] + txaentry.Text.ToCharArray () [3] + ",0x" + txaentry.Text.ToCharArray () [4] + txaentry.Text.ToCharArray () [5] + ",0x" + txaentry.Text.ToCharArray () [6] + txaentry.Text.ToCharArray () [7] + ",0x" + txaentry.Text.ToCharArray () [8] + txaentry.Text.ToCharArray () [9] + "\r\n", 0);
		}
		else 
			ShowErrorBox (this, "Bad Tx Address", "Tx address must be minimum of 10 hex characters");
		Thread.Sleep (500);
	}

	private void ShowErrorBox(Window parent, string title, string message)
	{
		Dialog dialog = null;
		try {
			dialog = new Dialog(title, parent,
			                    DialogFlags.DestroyWithParent | DialogFlags.Modal,
			                    ResponseType.Ok);
			dialog.VBox.Add(new Label(message));
			dialog.ShowAll();
			dialog.Run();
		} finally {
			if (dialog != null)
				dialog.Destroy ();
		}
	}

	private int baud_select()
	{
		Int32.TryParse (baudratebox.ActiveText, out baud_sel);
		if (baud_sel == -1)
			baud_sel = 4800;
		Console.WriteLine ("baud selected: " + baud_sel);
		return baud_sel;
	}

	private int rate_select()
	{
		return bitratebox.Active + 1;
	}
}
