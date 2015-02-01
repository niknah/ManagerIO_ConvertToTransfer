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
		string defaultDate;
		Gtk.TreeStore transactionsStore=null;

		public SearchTransactionsWindow (ManagerFile managerFile) :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
			this.defaultDate = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			startDate.Text = defaultDate;
			endDate.Text = defaultDate;
			this.searchTransactions=new SearchTransactions(managerFile);
			bankAccountCombo.FillBankAccounts (searchTransactions.managerFile.GetObjects());

			CellRendererToggle toggle=new Gtk.CellRendererToggle ();
			Gtk.TreeViewColumn toggleColumn = new Gtk.TreeViewColumn ("X",toggle,"active",1);
			toggle.Toggled += checkToggled;
//			toggleColumn.SortColumnId = -2; // GTK_TREE_SORTABLE_UNSORTED_SORT_COLUMN_ID

			Gtk.CellRendererText dateCell = new Gtk.CellRendererText ();
			Gtk.TreeViewColumn dateColumn = new Gtk.TreeViewColumn ("Date",dateCell,"text",2);
			dateColumn.SortColumnId = 2;
			Gtk.TreeViewColumn amountColumn = new Gtk.TreeViewColumn ("Amount",new Gtk.CellRendererText (),"text",6);
			amountColumn.SortColumnId = 3;

			Gtk.CellRendererText descriptionCell = new Gtk.CellRendererText ();
			Gtk.TreeViewColumn descriptionColumn = new Gtk.TreeViewColumn ("Description",descriptionCell,"text",4);
			descriptionColumn.SortColumnId = 4;
			Gtk.TreeViewColumn bankAccountColumn = new Gtk.TreeViewColumn ("Account",new Gtk.CellRendererText (),"text",5);
			bankAccountColumn.SortColumnId = 5;

			transactionsTreeView.AppendColumn (toggleColumn);
			transactionsTreeView.AppendColumn (dateColumn);
			transactionsTreeView.AppendColumn (amountColumn);
			transactionsTreeView.AppendColumn (descriptionColumn);
			transactionsTreeView.AppendColumn (bankAccountColumn);
		}

		void checkToggled(object o, ToggledArgs args) {
			TreeIter iter;

			if (transactionsStore.GetIter (out iter, new TreePath(args.Path))) {
				bool old = (bool) transactionsStore.GetValue(iter,1);
				transactionsStore.SetValue(iter,1,!old);
			}
		}


		DateTime? GetDateEntry(string text) {
			if (text == this.defaultDate || text=="")
				return null;
			return Convert.ToDateTime (text);
		}

		protected void OnFindButtonClicked (object sender, EventArgs e)
		{
			DoFind ();
		}


		void DoFind () {
			List<Guid> guids = searchTransactions.Find (bankAccountCombo.GetSelectedBankAccount (), 
				GetDateEntry(startDate.Text),GetDateEntry(endDate.Text),
				descriptionLabel.Text);
			uint row = 0;



			transactionsStore = new Gtk.TreeStore (typeof(Guid),typeof(bool),typeof (string), typeof (string), typeof (string), typeof (string),typeof(string));

			foreach(Guid guid in guids) {
				object o=searchTransactions.managerFile.GetObject (guid);
				if (o.GetType () == typeof(Payment)) {
					Payment payment = (Payment)o;
					string accountName=searchTransactions.managerFile.GetAccountName (payment.CreditAccount);
					decimal amount = (from l in payment.Lines
						select l.Amount).Sum ();
					transactionsStore.AppendValues (guid,false,
						String.Format ("{0:yyyy-MM-dd}", payment.Date), 
						String.Format ("{0:0000000000000.0000}",amount),
						payment.Description,accountName,String.Format ("{0:C}",amount)
					);
				} else if (o.GetType () == typeof(Receipt)) {
					Receipt payment = (Receipt)o;
					string accountName=searchTransactions.managerFile.GetAccountName (payment.DebitAccount);
					decimal amount = (from l in payment.Lines
					                select l.Amount).Sum ();
					transactionsStore.AppendValues (guid,false,
						String.Format ("{0:yyyy-MM-dd}", payment.Date), 
						String.Format ("{0:0000000000000.0000}",amount),
						payment.Description,accountName,String.Format ("{0:C}",amount)
						);
				}

				++row;
			}


			transactionsTreeView.Model = transactionsStore;
			transactionsTreeView.ShowAll ();
		}

		protected void OnDeleteButtonClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			transactionsStore.GetIterFirst (out iter);
			do {
				if(!((bool)transactionsStore.GetValue(iter,1))) continue;
				Guid guid=(Guid)transactionsStore.GetValue(iter,0);
				searchTransactions.managerFile.GetObjects ().Remove (guid);
			} while (transactionsStore.IterNext(ref iter));

			DoFind ();
		}

		protected void OnSelectAllButtonClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			transactionsStore.GetIterFirst (out iter);
			bool s=(bool)transactionsStore.GetValue(iter,1);
			s = !s;
			do {
				transactionsStore.SetValue(iter,1,s);
			} while (transactionsStore.IterNext(ref iter));
			transactionsTreeView.ShowAll ();
		}

	}
}

