using System;
using System.Windows.Forms;

namespace Tweety
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Forms.MainForm MainForm = new Forms.MainForm();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(MainForm.ExceptionHandler);
			Application.Run(MainForm);
		}
		
	}
}
