using System;
using System.Windows.Forms;

namespace Bau.Applications.NSharpDoc
{
	static class Program
	{
		/// <summary>
		///		Punto de entrada principal a la aplicación
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMain());
		}
	}
}
