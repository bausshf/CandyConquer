// Project by Bauss
using System;
using System.Windows.Forms;

namespace CandySql_LoginSample
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		internal static Models.User LoggedUser { get; set; }
		
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(Windows.MainForm.Form);
		}
		
	}
}
