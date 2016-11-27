// Project by Bauss
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CandySql_LoginSample.Windows
{
	/// <summary>
	/// Description of RegisterScreen.
	/// </summary>
	public partial class RegisterScreen : UserControl
	{
		public RegisterScreen()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void RegisterButtonClick(object sender, EventArgs e)
		{
			var response = Controllers.UserController.CreateUser(userNameTextBox.Text, passwordTextBox.Text);
			if (response.Success)
			{
				MainForm.Form.DisplayScreen<LoginScreen>();
			}
			else
			{
				MessageBox.Show(response.Message);
			}
		}
	}
}
