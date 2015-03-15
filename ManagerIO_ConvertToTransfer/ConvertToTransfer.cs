using System;
using System.IO;
using System.Collections.Generic;
using ManagerIO_ConvertToTransfer;
using Manager;
using Manager.Model;
using Manager.Persistence;

namespace ManagerIO_ConvertToTransfer
{
	public class ConvertToTransfer
	{
		public class TransferItem
		{
			public FindTransfers.FoundTransfer foundTransfer;
			public int nthReceipt;
		}

		List<TransferItem> transferItems;
		PersistentObjects objects;
		Dictionary<Guid,bool> addedReceipts;

		public ConvertToTransfer (PersistentObjects objects)
		{
			transferItems = new List<TransferItem> ();
			addedReceipts = new Dictionary<Guid,bool> ();
			this.objects = objects;
		}
		public void ConvertAll() {
			foreach (TransferItem item in transferItems) {
				Receipt receipt=item.foundTransfer.receipts [item.nthReceipt];
				Convert (item.foundTransfer.payment, receipt);
			}
		}

		public void Convert(Payment payment, Receipt receipt) {
			decimal paymentAmount = 0;
			decimal receiptAmount = 0;
			foreach (TransactionLine line in payment.Lines) {
				paymentAmount+=line.Amount;
			}
			foreach (TransactionLine line in receipt.Lines) {
				receiptAmount+=line.Amount;
			}
			objects.Remove (payment.Key);
			objects.Remove (receipt.Key);

			Transfer transfer=new Transfer ()  { 
				Key = Guid.NewGuid(),
				CreditAccount=payment.CreditAccount,
				DebitAccount=receipt.DebitAccount,
				CreditAmount = paymentAmount,
				DebitAmount=receiptAmount,
				Date=receipt.Date,
				Description=receipt.Description+" "+payment.Description+ " (Converted)",
				Reference=receipt.Reference+ " "+payment.Reference
			};
			objects.Put (transfer);
			/*
			if (paymentAmount != receiptAmount) {
				decimal exchangeRate =  paymentAmount/receiptAmount;
				TransactionExchangeRate exchange=new TransactionExchangeRate () {
					Key=Guid.NewGuid(),
					Transaction=transfer.Key,
					ExchangeRate=(decimal)exchangeRate,
					Account=(Guid)payment.CreditAccount
				};

				objects.Put(exchange);
			}
			*/
		}

		public bool Add(FindTransfers.FoundTransfer foundTransfer,int nthReceipt) {
			if(nthReceipt>=foundTransfer.receipts.Count) return false;

			TransferItem item=new TransferItem () { foundTransfer=foundTransfer, nthReceipt=nthReceipt };
			Receipt receipt=foundTransfer.receipts [nthReceipt];

			// check that we haven't added the same 
			if (addedReceipts.ContainsKey (receipt.Key)) {
				return false;
			}
			addedReceipts [receipt.Key] = true;

			transferItems.Add(item);
			return true;
		}
		public int Count() {
			return transferItems.Count;
		}

	}
}

