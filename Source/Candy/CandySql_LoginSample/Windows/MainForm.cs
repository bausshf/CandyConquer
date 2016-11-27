// Project by Bauss
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CandySql_LoginSample.Windows
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private static MainForm _form;
		
		public static MainForm Form
		{
			get
			{
				if (_form == null)
				{
					_form = new MainForm();
					_form.DisplayScreen<MainScreen>();
				}
				
				return _form;
			}
		}
		
		private MainForm()
			: base()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public void DisplayScreen<TScreen>()
			where TScreen : UserControl, new()
		{
			this.Controls.Clear();
			this.Controls.Add(new TScreen
			                  {
			                  	Dock = DockStyle.Fill
			                  });
		}
	}
}
