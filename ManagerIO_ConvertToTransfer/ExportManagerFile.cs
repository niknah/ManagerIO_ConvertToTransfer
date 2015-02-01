using System;
using Manager;
using Manager.Model;
using System.Linq;

namespace ManagerIO_ConvertToTransfer
{
	public class ExportManagerFile
	{
		ManagerFile managerFile;
		public ExportManagerFile (ManagerFile managerFile)
		{
			this.managerFile = managerFile;
		}

		public void ExportToCsv(Guid? bankAccountGuid,string filename) {
			var csvExport=new CsvExport();
			foreach (Manager.Model.Object transaction in managerFile.GetObjects().Values) {
				if (transaction.GetType () == typeof(Payment)) {
					Payment payment=(Payment)transaction;
					if (bankAccountGuid!=null && payment.CreditAccount != bankAccountGuid) {
						continue;
					}
					csvExport.AddRow ();
					csvExport ["BankAccount"] = managerFile.GetAccountName (payment.CreditAccount);
					csvExport ["Date"] = payment.Date;
					csvExport ["Description"] = payment.Description;
					csvExport ["Payee"] = payment.Payee;
					csvExport ["Reference"] = payment.Reference;
					int lineUpto = 1;
					foreach (TransactionLine line in payment.Lines) {
						csvExport ["Amount"+lineUpto] = line.Amount;
						string taxCode = "";
						string account="";
						if (line.TaxCode != null) {
							object taxCodeObj=managerFile.GetObject ((Guid)line.TaxCode);
							if (taxCodeObj.GetType () == typeof(InBuiltTaxCode)) {
								TaxCodes.InBuiltTaxCode inBuiltTaxCode =
									TaxCodes.MasterTaxCodes.FirstOrDefault (i=>i.Key==line.TaxCode);
								if(inBuiltTaxCode!=null)
									taxCode=inBuiltTaxCode.Code;
								//taxCode = ((InBuiltTaxCode)taxCodeObj);
							} else if (taxCodeObj.GetType () == typeof(TaxCode)) {
								taxCode = ((TaxCode)taxCodeObj).Name;
							} else {
								taxCode = line.TaxCode.ToString();
							}
						}
						if (line.Account != null) {
							account=((GeneralLedgerAccount)managerFile.GetObject ((Guid)line.Account)).Name;
						}
						csvExport ["Tax" + lineUpto]=taxCode;
						csvExport ["Account" + lineUpto]=account;
						++lineUpto;
					}
				} else if (transaction.GetType () == typeof(Receipt)) {
					Receipt payment = (Receipt)transaction;
					if (bankAccountGuid!=null && payment.DebitAccount != bankAccountGuid) {
						continue;
					}
					csvExport.AddRow ();
					csvExport ["BankAccount"] = managerFile.GetAccountName (payment.DebitAccount);
					csvExport ["Date"] = payment.Date;
					csvExport ["Description"] = payment.Description;
					csvExport ["Payee"] = payment.Payer;
					csvExport ["Reference"] = payment.Reference;
					int lineUpto = 1;
					foreach (TransactionLine line in payment.Lines) {
						csvExport ["Amount" + lineUpto] = line.Amount;
						string taxCode = "";
						string account = "";
						if (line.TaxCode != null) {
							object taxCodeObj = managerFile.GetObject ((Guid)line.TaxCode);
							if (taxCodeObj.GetType () == typeof(InBuiltTaxCode)) {
								TaxCodes.InBuiltTaxCode inBuiltTaxCode =
									TaxCodes.MasterTaxCodes.FirstOrDefault (i => i.Key == line.TaxCode);
								if (inBuiltTaxCode != null)
									taxCode = inBuiltTaxCode.Code;
								//taxCode = ((InBuiltTaxCode)taxCodeObj);
							} else if (taxCodeObj.GetType () == typeof(TaxCode)) {
								taxCode = ((TaxCode)taxCodeObj).Name;
							} else {
								taxCode = line.TaxCode.ToString ();
							}
						}
						if (line.Account != null) {
							account = ((GeneralLedgerAccount)managerFile.GetObject ((Guid)line.Account)).Name;
						}
						csvExport ["Tax" + lineUpto] = taxCode;
						csvExport ["Account" + lineUpto] = account;
						++lineUpto;
					}
				}
			}

			csvExport.ExportToFile (filename);
		}
	}
}

