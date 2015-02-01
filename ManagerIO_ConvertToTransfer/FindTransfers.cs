using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Manager.Model;

namespace ManagerIO_ConvertToTransfer
{
	public class FindTransfers
	{
		public class FoundTransfer {
			public List<Receipt> receipts;
			public Payment payment;

			public FoundTransfer(Payment payment) {
				this.payment=payment;
				receipts=new List<Receipt>();
			}
			public List<string> ReceiptsToStrings(ManagerFile managerFile) {
				List<string> strings = new List<string> ();
				foreach (Receipt receipt in receipts) {
					strings.Add (managerFile.GuidToText(receipt.Key));
				}
				return strings;
			}
			/*
			static public string PaymentToString(PersistentObjects objects,Payment payment) {
				Guid account = (Guid) payment.CreditAccount;
				return string.Format ("{0:yyyy-MM-dd} {1:C} {3}\n{2}\n{4}", 
					payment.Date, payment.Lines[0].Amount,payment.Description, 
					((BankAccount)objects[account]).Name,payment.Key);
			}
			static public string ReceiptToString(PersistentObjects objects,Receipt receipt) {
				Guid account = (Guid) receipt.DebitAccount;
				return string.Format ("{0:yyyy-MM-dd} {1:C} {3}\n{2}\n{4}", 
					receipt.Date, receipt.Lines[0].Amount,receipt.Description, 
					((BankAccount)objects[account]).Name,receipt.Key);
			}
			*/
		}

		decimal minExchangeRate=1,maxExchangeRate=1;
		int minDays=0,maxDays=5;

		public FindTransfers ()
		{
		}

		public void SetDaysRange(int minDays,int maxDays) {
			this.maxDays = maxDays;
			this.minDays = minDays;
		}

		public void SetExchangeRange(decimal min,decimal max) {
			minExchangeRate=min;
			maxExchangeRate=max;
		}

		public bool IsAmountOk(decimal fromAmount,decimal toAmount) {
			decimal min=(decimal)( toAmount* minExchangeRate);
			decimal max=(decimal)( toAmount* maxExchangeRate);
			if (fromAmount >= min && fromAmount <= max)
				return true;
			return false;
		}
		public bool IsDateOk(DateTime fromDate,DateTime toDate) {
			DateTime maxDate=fromDate.AddDays (maxDays);
			DateTime minDate=fromDate.AddDays (minDays);
			if (toDate.CompareTo (minDate)>=0 && toDate.CompareTo(maxDate)<0) {
				return true;
			}
			return false;
		}

		public List<FoundTransfer> Search(PersistentObjects objects) {
			List<FoundTransfer> founds=new List<FoundTransfer>();
			foreach (Payment payment in objects.Values.OfType<Payment>()) {
				if (payment.Lines.Length != 1)
					continue;
				TransactionLine paymentLine=payment.Lines[0];
				decimal paymentAmount=paymentLine.Amount;

				FoundTransfer found=new FoundTransfer (payment);
				if (payment.CreditAccount == null)
					continue;

				object creditAccountObj=objects [(Guid)payment.CreditAccount];
				if(creditAccountObj.GetType() != typeof(BankAccount)) continue;
				BankAccount creditAccount = ((BankAccount)creditAccountObj);

				foreach (Receipt receipt in objects.Values.OfType<Receipt>()) {
					if (receipt.DebitAccount == null)
						continue;
					if (receipt.Lines.Length != 1)
						continue;
					if (payment.CreditAccount == receipt.DebitAccount)
						continue;
					if (!IsDateOk (payment.Date, receipt.Date))
						continue;


					decimal receiptAmount=receipt.Lines[0].Amount;
					if (receipt.DebitAccount == null)
						continue;
					object debitAccountObj = objects [(Guid)receipt.DebitAccount];
					if(debitAccountObj.GetType() != typeof(BankAccount)) continue;
					BankAccount debitAccount = ((BankAccount)debitAccountObj);

					if (creditAccount.Currency == debitAccount.Currency) {
						// single currency transfer
						if (paymentAmount != receiptAmount) {
							continue;
						}
					} else if (!IsAmountOk (paymentAmount, receiptAmount))
						continue;

					found.receipts.Add (receipt);
				}
				if (found.receipts.Count > 0)
					founds.Add (found);
			}
			return founds;
		}
	}
}


