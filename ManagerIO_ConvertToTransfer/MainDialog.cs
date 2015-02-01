using System;

namespace ManagerIO_ConvertToTransfer
{
	public partial class MainDialog : Gtk.Window
	{
		string filename;
		public MainDialog (): base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.filename=ManagerFile.SelectFile (this);
			//this.filename = "/tmp/test.manager";
		}


		protected void OnConvertTransfersClicked (object sender, EventArgs e)
		{
			ConvertToTransferWindow win = new ConvertToTransferWindow (this.filename);
			win.Show ();
		}

		protected void OnSearchTransactionsClicked (object sender, EventArgs e)
		{
			SearchTransactionsWindow win=new SearchTransactionsWindow (this.filename);
			win.Show ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			this.Destroy ();
		}
	}
}

