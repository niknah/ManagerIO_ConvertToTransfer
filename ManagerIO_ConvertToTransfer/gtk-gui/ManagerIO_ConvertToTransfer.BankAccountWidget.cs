
// This file has been generated by the GUI designer. Do not modify.
namespace ManagerIO_ConvertToTransfer
{
	public partial class BankAccountWidget
	{
		private global::Gtk.Alignment alignment1;
		
		private global::Gtk.ComboBox comboBox;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ManagerIO_ConvertToTransfer.BankAccountWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "ManagerIO_ConvertToTransfer.BankAccountWidget";
			// Container child ManagerIO_ConvertToTransfer.BankAccountWidget.Gtk.Container+ContainerChild
			this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
			this.alignment1.Name = "alignment1";
			// Container child alignment1.Gtk.Container+ContainerChild
			this.comboBox = global::Gtk.ComboBox.NewText ();
			this.comboBox.Name = "comboBox";
			this.alignment1.Add (this.comboBox);
			this.Add (this.alignment1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
