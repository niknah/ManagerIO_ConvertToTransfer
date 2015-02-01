using System;
using Gtk;

namespace ManagerIO_ConvertToTransfer
{
	class MainClass
	{
		static void quit (object o, EventArgs args)
		{
			Application.Quit ();
		}

		public static void Main (string[] args)
		{
			Application.Init ();

			MainWindow mainDialog=new MainWindow ();
			//mainDialog.Close += new EventHandler (quit);
			mainDialog.Destroyed += new EventHandler (quit);
			mainDialog.Show ();
			Application.Run ();
		}
	}
}
