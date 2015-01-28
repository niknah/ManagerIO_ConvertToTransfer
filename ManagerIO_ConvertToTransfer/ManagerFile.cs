﻿using System;
using System.IO;
using Gtk;
using Manager;
using System.Linq;
using Manager.Model;

namespace ManagerIO_ConvertToTransfer
{
	public class ManagerFile
	{
		public PersistentObjects objects;
		string filename;

		public ManagerFile (string filename)
		{
			this.filename = filename;
			if(this.filename!=null)
				this.objects = new PersistentObjects(this.filename);
		}
		public PersistentObjects GetObjects() {
			return objects;
		}

		public object GetObject(Guid guid) {
			return objects [guid];
		}

		public string GuidToText(Guid id) {
			object o=GetObject(id);
			if (o.GetType () == typeof(Payment)) {
				Payment payment=(Payment)o;
				return string.Format ("{0:yyyy-MM-dd} {1:C} {3}\n{2}\n{4}", 
					payment.Date, payment.Lines[0].Amount,payment.Description, 
					((BankAccount)this.GetObject((Guid)payment.CreditAccount)).Name,payment.Key);
			} else if (o.GetType () == typeof(Receipt)) {
				Receipt receipt = (Receipt)o;
				Guid account = (Guid) receipt.DebitAccount;
				return string.Format ("{0:yyyy-MM-dd} {1:C} {3}\n{2}\n{4}", 
					receipt.Date, receipt.Lines[0].Amount,receipt.Description, 
					((BankAccount)this.GetObject(account)).Name,receipt.Key);
			}
			return o.GetType ().Name;
		}


		static public string SelectFile(Window parent) {
			Gtk.FileChooserDialog fc=
				new Gtk.FileChooserDialog("Choose the file to open",
					parent,
					FileChooserAction.Open,
					"Cancel",ResponseType.Cancel,
					"Open",ResponseType.Accept);
			try {

				if (fc.Run() != (int)ResponseType.Accept) 
				{
					return null;
				}
				return fc.Filename;
			} finally {
				fc.Destroy ();
			}
		}

		public void BackupFile() {
			string backupFilename=Path.Combine(
				Path.GetDirectoryName(filename),
				Path.GetFileNameWithoutExtension (filename)+
				string.Format("_{0:yyyy-MM-dd-HHmmss}_backup.manager",DateTime.Now));
			File.Copy (filename, backupFilename);
		}
	}
}
