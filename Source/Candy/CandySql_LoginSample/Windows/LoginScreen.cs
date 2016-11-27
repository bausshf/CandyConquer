// Project by Bauss
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CandySql_LoginSample.Windows
{
	/// <summary>
	/// Description of LoginScreen.
	/// </summary>
	public partial class LoginScreen : UserControl
	{
		public LoginScreen()
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
			var response = Controllers.UserController.Login(userNameTextBox.Text, passwordTextBox.Text);
			
			if (response.Success)
			{
				MainForm.Form.DisplayScreen<SuccessScreen>();
			}
			else
			{
				MessageBox.Show(response.Message);
			}
		}
	}
}
