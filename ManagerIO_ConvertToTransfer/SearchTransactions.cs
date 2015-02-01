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
		public SearchTransactions (ManagerFile managerFile)
		{
			this.managerFile = managerFile;
		}

		public List<Guid> Find(Guid? bankAccountGuid,DateTime? startTime, DateTime? endTime,string description) {
			List<Guid> guids=new List<Guid>();
			description=description.ToLower ();
			foreach (Manager.Model.Object transaction in managerFile.GetObjects().Values) {
				if (transaction.GetType () == typeof(Payment)) {
					Payment payment=(Payment)transaction;
					if (bankAccountGuid!=null && payment.CreditAccount != bankAccountGuid) {
						continue;
					}
					if (description != "" && payment.Description.ToLower().IndexOf(description)<0)
						continue;

					if ((startTime!=null && payment.Date.CompareTo(startTime)<0) || 
						(endTime!=null && payment.Date.CompareTo(endTime)>0)
						)
						continue;
					guids.Add(payment.Key);
				} else if (transaction.GetType () == typeof(Receipt)) {
					Receipt receipt = (Receipt)transaction;
					if (bankAccountGuid!=null && receipt.DebitAccount != bankAccountGuid) {
						continue;
					}
					if (description != "" && receipt.Description.ToLower().IndexOf(description)<0)
						continue;
					if ((startTime != null && receipt.Date.CompareTo (startTime) < 0) ||
					    (endTime != null && receipt.Date.CompareTo (endTime) > 0))
						continue;
					guids.Add(receipt.Key);
				}
			}
			return guids;
		}

	}
}

