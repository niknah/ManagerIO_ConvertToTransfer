
// This file has been generated by the GUI designer. Do not modify.
namespace ManagerIO_ConvertToTransfer
{
	public partial class MainWindow
	{
		private global::Gtk.VBox vbox3;
		private global::Gtk.Button convertTransfers;
		private global::Gtk.Button searchTransactions;
		private global::Gtk.Button clearReconciliation;
		private global::Gtk.Button okButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ManagerIO_ConvertToTransfer.MainWindow
			this.Name = "ManagerIO_ConvertToTransfer.MainWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child ManagerIO_ConvertToTransfer.MainWindow.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.convertTransfers = new global::Gtk.Button ();
			this.convertTransfers.CanFocus = true;
			this.convertTransfers.Name = "convertTransfers";
			this.convertTransfers.UseUnderline = true;
			this.convertTransfers.Label = global::Mono.Unix.Catalog.GetString ("_Convert transactions to transfers");
			this.vbox3.Add (this.convertTransfers);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.convertTransfers]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.searchTransactions = new global::Gtk.Button ();
			this.searchTransactions.CanFocus = true;
			this.searchTransactions.Name = "searchTransactions";
			this.searchTransactions.UseUnderline = true;
			this.searchTransactions.Label = global::Mono.Unix.Catalog.GetString ("_Search transactions");
			this.vbox3.Add (this.searchTransactions);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.searchTransactions]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.clearReconciliation = new global::Gtk.Button ();
			this.clearReconciliation.CanFocus = true;
			this.clearReconciliation.Name = "clearReconciliation";
			this.clearReconciliation.UseUnderline = true;
			this.clearReconciliation.Label = global::Mono.Unix.Catalog.GetString ("_Bank account tools");
			this.vbox3.Add (this.clearReconciliation);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.clearReconciliation]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.okButton = new global::Gtk.Button ();
			this.okButton.CanFocus = true;
			this.okButton.Name = "okButton";
			this.okButton.UseStock = true;
			this.okButton.UseUnderline = true;
			this.okButton.Label = "gtk-ok";
			this.vbox3.Add (this.okButton);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.okButton]));
			w4.Position = 3;
			w4.Expand = false;
			w4.Fill = false;
			this.Add (this.vbox3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 235;
			this.DefaultHeight = 150;
			this.Show ();
			this.convertTransfers.Clicked += new global::System.EventHandler (this.OnConvertTransfersClicked);
			this.searchTransactions.Clicked += new global::System.EventHandler (this.OnSearchTransactionsClicked);
			this.clearReconciliation.Clicked += new global::System.EventHandler (this.OnClearReconciliationClicked);
			this.okButton.Clicked += new global::System.EventHandler (this.OnOkButtonClicked);
		}
	}
}
