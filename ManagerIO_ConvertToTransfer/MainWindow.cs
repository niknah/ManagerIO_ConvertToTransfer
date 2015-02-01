using System;

namespace ManagerIO_ConvertToTransfer
{
	public partial class MainWindow : Gtk.Window
	{
		string filename;
		ManagerFile managerFile;


		public MainWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.filename=ManagerFile.SelectFile (this);
			managerFile = new ManagerFile (filename);
		}

		protected void OnSearchTransactionsClicked (object sender, EventArgs e)
		{
			SearchTransactionsWindow win=new SearchTransactionsWindow (managerFile);
			win.Show ();
		}

		protected void OnConvertTransfersClicked (object sender, EventArgs e)
		{
			ConvertToTransferWindow win = new ConvertToTransferWindow (managerFile);
			win.Show ();
		}

		protected void OnOkButtonClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}

		protected void OnClearReconciliationClicked (object sender, EventArgs e)
		{
			BankAccountUtilsWindow win=new BankAccountUtilsWindow (managerFile);
			win.Show ();
		}
	}
}

