using System;
using Manager;
using Manager.Model;
using System.Linq;
using System.Collections.Generic;

namespace ManagerIO_ConvertToTransfer
{
	public class SearchTransactions
	{

		public ManagerFile managerFile;
		public SearchTransactions (string filename)
		{
			managerFile = new ManagerFile (filename);
		}


		public List<Guid> Find(Guid bankAccountGuid,DateTime startTime, DateTime endTime) {
			List<Guid> guids=new List<Guid>();
			foreach (Manager.Model.Object transaction in managerFile.GetObjects().Values) {
				if (transaction.GetType () == typeof(Payment)) {
					Payment payment=(Payment)transaction;
					if (payment.CreditAccount != bankAccountGuid) {
						continue;
					}
					if (payment.Date.CompareTo(startTime)<0 || payment.Date.CompareTo(endTime)>0)
						continue;
					guids.Add(payment.Key);
				} else if (transaction.GetType () == typeof(Receipt)) {
					Receipt receipt = (Receipt)transaction;
					if (receipt.DebitAccount != bankAccountGuid) {
						continue;
					}
					if (receipt.Date.CompareTo(startTime)<0 || receipt.Date.CompareTo(endTime)>0)
						continue;
					guids.Add(receipt.Key);
				}
			}
			return guids;
		}

		public void ClearReconciliation (Guid bankAccountGuid)
		{
			managerFile.BackupFile ();
			foreach (AccountReconciliationBalance reconcile in managerFile.GetObjects().Values.OfType<AccountReconciliationBalance>()) {
				if (reconcile.Account != bankAccountGuid) {
					continue;
				}
				managerFile.GetObjects().Remove (reconcile.Key);
			}
		}
	}
}

