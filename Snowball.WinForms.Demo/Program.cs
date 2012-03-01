using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Snowball.WinForms.Demo
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.ThreadException += (s, e) =>
			{
				MessageBox.Show(e.Exception.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
			};

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
