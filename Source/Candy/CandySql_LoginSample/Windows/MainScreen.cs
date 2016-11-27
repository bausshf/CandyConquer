// Project by Bauss
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CandySql_LoginSample.Windows
{
	/// <summary>
	/// Description of MainScreen.
	/// </summary>
	public partial class MainScreen : UserControl
	{
		public MainScreen()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void LoginButtonClick(object sender, EventArgs e)
		{
			MainForm.Form.DisplayScreen<LoginScreen>();
		}
		
		void RegisterButtonClick(object sender, EventArgs e)
		{
			MainForm.Form.DisplayScreen<RegisterScreen>();
		}
	}
}
