//----------------------------------------------------------------------------------------
// <copyright file="Storage.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.WindowsServerSolutions.Common;
using Microsoft.WindowsServerSolutions.Storage;

namespace WebDavWhs
{
	/// <summary>
	/// 	The Storage wrapper.
	/// </summary>
	internal class Storage : IDisposable
	{
		/// <summary>
		/// 	Occurs when the sync process has been changed.
		/// </summary>
		internal event EventHandler<StoragePropertyEventArgs> StoragePropertyChanged;

		/// <summary>
		/// 	Disposable flag.
		/// </summary>
		private bool isDisposed;

		/// <summary>
		/// 	Gets or sets the storage manager.
		/// </summary>
		/// <value> The storage manager. </value>
		private StorageManager StorageManager
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets the folders.
		/// </summary>
		public Collection<Folder> Folders
		{
			get
			{
				return this.GetFolderCollection();
			}
		}

		#region Lifecycle

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Storage" /> class.
		/// </summary>
		public Storage()
		{
			this.isDisposed = false;
			this.Initialize();
		}

		/// <summary>
		/// 	Initializes this instance.
		/// </summary>
		private void Initialize()
		{
			Trace.TraceInformation("Initialize...");

			try
			{
				WindowsServerSolutionsEnvironment.Initialize();
				this.StorageManager = new StorageManager();
				this.StorageManager.PropertyChanged += this.StorageManagerPropertyChanged;
			}
			catch(Exception exception)
			{
				Trace.TraceError(exception.ToString());
			}

			Trace.TraceInformation("Initialize...finished.");
		}

		/// <summary>
		/// 	Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose()
		{
			if(this.isDisposed)
			{
				return;
			}

			if(this.StorageManager != null)
			{
				this.StorageManager.Dispose();
			}
			
			this.isDisposed = true;
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// 	Connects the storage mananger.
		/// </summary>
		public void Connect()
		{
			Trace.TraceInformation("Connect...");

			if(this.StorageManager.Connected)
			{
				Trace.TraceInformation("Storage Manager is already connected.");
				Trace.TraceInformation("Connect...finished.");
				return;
			}

			this.StorageManager.ConnectAsync();
			Trace.TraceInformation("Connect...finished.");
		}

		/// <summary>
		/// 	Gets the folder collection.
		/// </summary>
		/// <returns> The folder collection. </returns>
		private Collection<Folder> GetFolderCollection()
		{
			Trace.TraceInformation("GetFolderCollection...");

			if(this.StorageManager.Connected == false)
			{
				this.StorageManager.Connect(10000);
			}

			Collection<Folder> collection = new Collection<Folder>();

			foreach(Folder folder in this.StorageManager.Folders)
			{
				collection.Add(folder);
			}

			Trace.TraceInformation("GetFolderCollection...finished.");
			return collection;
		}

		/// <summary>
		/// 	Handles the PropertyChanged event of the StorageManager control.
		/// </summary>
		/// <param name="sender"> The source of the event. </param>
		/// <param name="e"> The <see cref="System.ComponentModel.PropertyChangedEventArgs" /> instance containing the event data. </param>
		private void StorageManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Trace.TraceInformation("StorageManagerPropertyChanged...");
			Trace.TraceInformation(string.Format("PropertyChangedEventArgs : {0}", e.PropertyName));

			if(this.StoragePropertyChanged != null)
			{
				this.StoragePropertyChanged(this, new StoragePropertyEventArgs(this.EnumServerFolders()));
			}

			Trace.TraceInformation("StorageManagerPropertyChanged...finished.");
		}

		/// <summary>
		/// 	Enums the server folders.
		/// </summary>
		/// <returns> Folder list. </returns>
		private Dictionary<Guid, ServerFolder> EnumServerFolders()
		{
			Trace.TraceInformation("EnumServerFolders...");
			Dictionary<Guid, ServerFolder> folderList = new Dictionary<Guid, ServerFolder>();

			try
			{
				foreach(Folder folder in this.StorageManager.Folders)
				{
					ServerFolder serverFolder = new ServerFolder();
					serverFolder.Name = folder.Name;
					serverFolder.Path = folder.Path;
					serverFolder.ID = folder.ID;
					folderList.Add(serverFolder.ID, serverFolder);

					Trace.TraceInformation("Folder name: '{0}'", serverFolder.Name);
					Trace.TraceInformation("Folder path: '{0}'", serverFolder.Path);
				}

				return folderList;
			}
			finally
			{
				Trace.TraceInformation("EnumServerFolders...finished.");
			}
		}
	}
}