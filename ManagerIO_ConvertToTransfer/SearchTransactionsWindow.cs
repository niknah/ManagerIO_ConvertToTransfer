using System;
using System.Collections.Generic;
using Manager.Model;
using System.Linq;
using Gtk;
using System.Globalization;

namespace ManagerIO_ConvertToTransfer
{
	public partial class SearchTransactionsWindow : Gtk.Window
	{
		SearchTransactions searchTransactions;
		List<Guid> bankAccounts=null;

		public SearchTransactionsWindow (string filename) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			startDate.Text = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			endDate.Text = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			this.searchTransactions=new SearchTransactions(filename);
			this.FillBankAccounts ();
		}

		void FillBankAccounts() {
			if(bankAccounts!=null)
				for (int i = 0; i < bankAccounts.Count; ++i) {
					bankAccountCombo.RemoveText (0);
				}
			bankAccounts = new List<Guid> ();
			foreach (BankAccount bankAccount in searchTransactions.managerFile.GetObjects().Values.OfType<BankAccount>()) {
				bankAccounts.Add(bankAccount.Key);
				bankAccountCombo.AppendText (bankAccount.Name);
			}
		}

		Guid GetSelectedBankAccount() {
			return bankAccounts[bankAccountCombo.Active];
		}

		protected void OnClearReconcileButtonClicked (object sender, EventArgs e)
		{
			Guid bankAccountGuid = GetSelectedBankAccount ();
			searchTransactions.ClearReconciliation (bankAccountGuid);
		}

		public class FoundRow
		{
			CheckButton checkButton;
			Label label;
			Alignment align;
			public Guid guid;

			public FoundRow(Guid guid,string text) {
				checkButton=new CheckButton();
				align = new Alignment (0, 0, 0, 0);
				label=new Label();
				label.Text=text;
				label.LineWrap = true;
				label.LineWrapMode = Pango.WrapMode.Word;
				label.Justify = Justification.Left;
				align.Add (label);

				this.guid=guid;
			}

			public bool IsChecked() {
				return checkButton.Active;
			}
			public void Check() {
				checkButton.Active = true;
			}
			public void AddToTable(Table table,uint row) {
				table.Attach(checkButton,0,1,row,row+1);
				table.Attach(align,1,2,row,row+1,AttachOptions.Fill,AttachOptions.Fill,0,0);
			}
			public void Destroy() {
				checkButton.Destroy ();
				label.Destroy ();
				align.Destroy ();
			}
		}

		List<FoundRow> foundRows=new List<FoundRow>();
		protected void OnFindButtonClicked (object sender, EventArgs e)
		{
			List<Guid> guids = searchTransactions.Find (GetSelectedBankAccount (), 
				Convert.ToDateTime(startDate.Text),Convert.ToDateTime(endDate.Text));
			transactionsTable.Resize((uint)guids.Count,2);
			uint row = 0;
			foreach (FoundRow foundRow in foundRows) {
				foundRow.Destroy ();
			}
			foundRows.Clear ();

			foreach(Guid guid in guids) {
				FoundRow foundRow=new FoundRow(guid,searchTransactions.managerFile.GuidToText(guid));
				foundRow.AddToTable(transactionsTable,row);
				foundRows.Add(foundRow);
				++row;
			}
			transactionsTable.ShowAll ();
		}

		protected void OnDeleteButtonClicked (object sender, EventArgs e)
		{
			searchTransactions.managerFile.BackupFile ();
			foreach (FoundRow foundRow in foundRows) {
				if(!foundRow.IsChecked ()) continue;
				searchTransactions.managerFile.GetObjects ().Remove (foundRow.guid);
			}

		}

		protected void OnSelectAllButtonClicked (object sender, EventArgs e)
		{
			foreach (FoundRow foundRow in foundRows) {
				foundRow.Check ();
			}
		}

	}
}

