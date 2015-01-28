using System;
using System.Collections.Generic;
using Gtk;
using ManagerIO_ConvertToTransfer;
using Manager;

public partial class ConvertToTransferWindow: Gtk.Window
{
	ManagerFile managerFile;

	public ConvertToTransferWindow (String filename) : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		managerFile = new ManagerFile (filename);
	}


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	public class ResultRow {
		public CheckButton okButton;
		public ComboBox comboBox=null;
		public Alignment fromAlign,toAlign;
		public FindTransfers.FoundTransfer foundTransfer;

		public void DestroyGui() {
			if (comboBox != null)
				comboBox.Destroy ();
			okButton.Destroy ();
			if (fromAlign != null)
				fromAlign.Destroy ();
			if (toAlign != null)
				toAlign.Destroy ();
		}
	}

	List<ResultRow> resultRows;
	void ClearResultRows() {
		if (resultRows != null) {
			foreach (ResultRow resultRow in resultRows) {
				resultRow.DestroyGui ();
			}
		}
		resultRows = new List<ResultRow> ();
	}

	void SearchAndDisplay() {
		FindTransfers findTransfers=new FindTransfers ();
		findTransfers.SetExchangeRange(Convert.ToDecimal(minExchangeRate.Text),Convert.ToDecimal(maxExchangeRate.Text));
		findTransfers.SetDaysRange (Convert.ToInt32 (minDays.Text),Convert.ToInt32 (maxDays.Text));
		List<FindTransfers.FoundTransfer> foundTransfers=findTransfers.Search (managerFile.GetObjects());
		FillSearchResults (foundTransfers);
	}

	void FillSearchResults(List<FindTransfers.FoundTransfer> foundTransfers) {
		ClearResultRows ();
		resultsTable.Resize ((uint)foundTransfers.Count+1,3);
		uint row = 1;
		foreach (FindTransfers.FoundTransfer foundTransfer in foundTransfers) {
			string paymentStr = managerFile.GuidToText (foundTransfer.payment.Key);
			CheckButton checkButton=new CheckButton ();
			resultsTable.Attach (checkButton, 1, 2,row,row+1);

			Alignment align = new Alignment (0, 0, 0, 0);
			Label label=new Label (paymentStr);
			label.LineWrap = true;
			label.LineWrapMode = Pango.WrapMode.Word;
			label.Justify = Justification.Left;
			align.Add (label);
			resultsTable.Attach (align, 0, 1,row,row+1,AttachOptions.Fill,AttachOptions.Fill,0,0);
			List<string> receiptsStr=foundTransfer.ReceiptsToStrings (managerFile);
			ResultRow resultRow=new ResultRow() { 
				okButton=checkButton,  foundTransfer=foundTransfer,
				fromAlign=align
			};

			if (receiptsStr.Count == 1) {
				Alignment receiptAlign = new Alignment (0, 0, 0, 0);
				Label receiptLabel = new Label (receiptsStr [0]);
				receiptAlign.Add (receiptLabel);
				resultRow.toAlign=receiptAlign;
				resultsTable.Attach (receiptAlign, 2, 3, row, row + 1);
			} else {
				ComboBox comboBox = new ComboBox (receiptsStr.ToArray ());
				resultRow.comboBox = comboBox;
				resultsTable.Attach (comboBox, 2, 3, row, row + 1);
			}

			resultRows.Add (resultRow);

			++row;
		}
		resultsTable.ShowAll();
	}

	protected void OnSearch (object sender, EventArgs e)
	{
		SearchAndDisplay ();
	}


	protected void OnSaveButtonClicked (object sender, EventArgs e)
	{
		ConvertToTransfer convertToTransfer= new ConvertToTransfer (managerFile.GetObjects());
		foreach (ResultRow resultRow in resultRows) {
			if (!resultRow.okButton.Active)
				continue;
			int selected = 0;
			if(resultRow.comboBox!=null)
				selected=resultRow.comboBox.Active;
			if (selected < 0) {
				MessageBox ("Ticked but dropdown not selected");
				return;
			}

			if (!convertToTransfer.Add (resultRow.foundTransfer, selected)) {
				MessageBox("Failed to add, maybe trying to add the same receipt twice. guid:"+resultRow.foundTransfer.payment.Key);
				return;
			}
//Console.WriteLine(resultRow.foundTransfer.receipts.Count+", selected:"+selected);
		}

		managerFile.BackupFile ();
		convertToTransfer.ConvertAll ();
		MessageBox ("Converted:" + convertToTransfer.Count ());
		SearchAndDisplay ();
	}
	public void MessageBox(string message) {
		MessageDialog md = new MessageDialog(this, 
			DialogFlags.DestroyWithParent, MessageType.Info, 
			ButtonsType.Close, message);
		md.Run();
		md.Destroy();
	}
}
