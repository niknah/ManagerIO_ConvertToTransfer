
// This file has been generated by the GUI designer. Do not modify.
namespace ManagerIO_ConvertToTransfer
{
	public partial class BankAccountUtilsWindow
	{
		private global::Gtk.Table table1;
		
		private global::ManagerIO_ConvertToTransfer.BankAccountWidget bankAccountCombo;
		
		private global::Gtk.Button clearButton;
		
		private global::Gtk.Button exportButton;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ManagerIO_ConvertToTransfer.BankAccountUtilsWindow
			this.Name = "ManagerIO_ConvertToTransfer.BankAccountUtilsWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("ClearReconciliationWindow");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child ManagerIO_ConvertToTransfer.BankAccountUtilsWindow.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(1)), ((uint)(4)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.bankAccountCombo = new global::ManagerIO_ConvertToTransfer.BankAccountWidget ();
			this.bankAccountCombo.Events = ((global::Gdk.EventMask)(256));
			this.bankAccountCombo.Name = "bankAccountCombo";
			this.table1.Add (this.bankAccountCombo);
			global::Gtk.Table.TableChild w1 = ((global::Gtk.Table.TableChild)(this.table1 [this.bankAccountCombo]));
			w1.XOptions = ((global::Gtk.AttachOptions)(4));
			w1.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.clearButton = new global::Gtk.Button ();
			this.clearButton.CanFocus = true;
			this.clearButton.Name = "clearButton";
			this.clearButton.UseUnderline = true;
			this.clearButton.Label = global::Mono.Unix.Catalog.GetString ("_Clear reconciliation");
			this.table1.Add (this.clearButton);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.clearButton]));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.exportButton = new global::Gtk.Button ();
			this.exportButton.CanFocus = true;
			this.exportButton.Name = "exportButton";
			this.exportButton.UseUnderline = true;
			this.exportButton.Label = global::Mono.Unix.Catalog.GetString ("_Export");
			this.table1.Add (this.exportButton);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.exportButton]));
			w3.LeftAttach = ((uint)(2));
			w3.RightAttach = ((uint)(3));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			this.Add (this.table1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 58;
			this.Show ();
			this.exportButton.Clicked += new global::System.EventHandler (this.OnExportButtonClicked);
			this.clearButton.Clicked += new global::System.EventHandler (this.OnClearButtonClicked);
		}
	}
}
