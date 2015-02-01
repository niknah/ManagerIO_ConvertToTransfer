using System;
using Manager.Model;
using System.Linq;
using Gtk;

namespace ManagerIO_ConvertToTransfer
{
	public partial class BankAccountUtilsWindow : Gtk.Window
	{
		ManagerFile managerFile;

		public BankAccountUtilsWindow (ManagerFile managerFile) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.managerFile = managerFile;
			bankAccountCombo.FillBankAccounts (managerFile.GetObjects());
		}


		protected void OnClearButtonClicked (object sender, EventArgs e)
		{
			Guid? bankAccountGuid = bankAccountCombo.GetSelectedBankAccount ();
			managerFile.BackupFile ();
			managerFile.ClearReconciliation (bankAccountGuid);
		}

		protected void OnExportButtonClicked (object sender, EventArgs e)
		{
			Gtk.FileChooserDialog fc=
				new Gtk.FileChooserDialog("Choose the file to save",
					this,
					FileChooserAction.Save,
					"Cancel",ResponseType.Cancel,
					"Open",ResponseType.Accept);
			fc.DoOverwriteConfirmation = true;
			FileFilter filter = new FileFilter();
			filter.AddPattern ("*.csv");
			filter.Name = "*.csv";
			fc.AddFilter (filter);

			string filename;
			try {
				if (fc.Run() != (int)ResponseType.Accept) 
				{
					return;
				}
				filename=fc.Filename;
			} finally {
				fc.Destroy ();
			}

			ExportManagerFile exportManagerFile=new ExportManagerFile (managerFile);
			Guid? bankAccountGuid = bankAccountCombo.GetSelectedBankAccount ();
			exportManagerFile.ExportToCsv (bankAccountGuid,filename);
		}
	}
}

