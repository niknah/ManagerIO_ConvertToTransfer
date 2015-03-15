using System;
using Manager.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Manager.Persistence;
using Manager;
using Object = Manager.Model.Object;

namespace ManagerIO_ConvertToTransfer
{
	public sealed class PersistentObjects
	{
		//
		// Properties
		//
		Dictionary<Guid,Object> objectDictionary;
		PersistentDictionary persistentDictionary;
		public Guid[] Keys
		{
			get
			{
				return this.objectDictionary.Keys.ToArray<Guid> ();
			}
		}
		public Object[] Values
		{
			get
			{
				return this.objectDictionary.Values.ToArray<Object> ();
			}
		}
		//
		// Indexer
		//
		public Object this [Guid key]
		{
			get
			{
				return this.objectDictionary[key];
			}
		}
		//
		// Constructors
		//
		public PersistentObjects (string path)
		{
			this.objectDictionary = new Dictionary<Guid, Object> ();
			this.persistentDictionary = new PersistentDictionary (path);
			try
			{
				Guid[] keys = this.persistentDictionary.Keys;
				for (int i = 0; i < keys.Length; i++)
				{
					Guid guid = keys [i];
					byte[] source = this.persistentDictionary.Get (guid);
					Guid guid2 = new Guid (source.Take (16).ToArray<byte> ());
					if (
					    !(Serialization.GetTypeByGuid (guid2) == typeof(Blob)))
					{
						byte[] content = source.Skip (16).ToArray<byte> ();
						Object @object = (Object)Serialization.Deserialize (guid2, content);
						@object.Key = guid;
						this.objectDictionary.Add (guid, @object);
					}
				}
			}
			catch
			{
				this.persistentDictionary.Dispose ();
				this.objectDictionary = null;
				this.persistentDictionary = null;
				throw;
			}
		}
		//
		// Methods
		//
		public Guid Put (Object o)
		{
			Tuple<Guid, byte[]> tuple = Serialization.Serialize (o);
			MemoryStream memoryStream = new MemoryStream ();
			BinaryWriter binaryWriter = new BinaryWriter (memoryStream);
			binaryWriter.Write (tuple.Item1.ToByteArray ());
			binaryWriter.Write (tuple.Item2);
			binaryWriter.Flush ();
			this.persistentDictionary.Put (o.Key, memoryStream.ToArray ());
			if (this.objectDictionary.ContainsKey (o.Key))
			{
				this.objectDictionary[o.Key]=o;
			}
			else
			{
				this.objectDictionary.Add (o.Key, o);
			}
			return o.Key;
		}
		public void Remove (Guid objectId)
		{
			this.persistentDictionary.Remove (objectId);
			this.objectDictionary.Remove (objectId);
		}
	}
}

