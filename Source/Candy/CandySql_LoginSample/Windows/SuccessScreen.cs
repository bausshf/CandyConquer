// Project by Bauss
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CandySql_LoginSample.Windows
{
	/// <summary>
	/// Description of SuccessScreen.
	/// </summary>
	public partial class SuccessScreen : UserControl
	{
		public SuccessScreen()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			if (Program.LoggedUser.LastLoginDate == null)
			{
				Program.LoggedUser.LastLoginDate = DateTime.UtcNow;
			}
			
			loginLabel.Text = string.Format(loginLabel.Text, Program.LoggedUser.UserName, Program.LoggedUser.LastLoginDate);
			
			Program.LoggedUser.LastLoginDate = DateTime.UtcNow;
		}
	}
}
