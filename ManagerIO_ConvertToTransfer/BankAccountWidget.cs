using System;
using Manager;
using Manager.Model;
using System.Linq;
using System.Collections.Generic;


namespace ManagerIO_ConvertToTransfer
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class BankAccountWidget : Gtk.Bin
	{
		List<Guid?> bankAccounts=null;
		public BankAccountWidget ()
		{
			this.Build ();
		}
		public void FillBankAccounts(PersistentObjects objects) {
			if(bankAccounts!=null)
				for (int i = 0; i < bankAccounts.Count; ++i) {
					comboBox.RemoveText (0);
				}
			bankAccounts = new List<Guid?> ();
			bankAccounts.Add(null);
			comboBox.AppendText ("All Accounts");
			foreach (BankAccount bankAccount in objects.Values.OfType<BankAccount>()) {
				bankAccounts.Add(bankAccount.Key);
				comboBox.AppendText (bankAccount.Name);
			}
		}

		public Guid? GetSelectedBankAccount() {
			int active=comboBox.Active;
			if (active <= 0)
				return null;
			return bankAccounts[active];
		}
	}
}

