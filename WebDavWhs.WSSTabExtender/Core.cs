//----------------------------------------------------------------------------------------
// <copyright file="Core.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;

namespace WebDavWhs
{
	/// <summary>
	/// 	Provides properties and methods for the central BL.
	/// </summary>
	internal class Core : IDisposable
	{
		/// <summary>
		/// 	Disposable flag.
		/// </summary>
		private bool isDisposed;

		/// <summary>
		/// 	Gets or sets the IIS.
		/// </summary>
		/// <value> The IIS. </value>
		internal Iis Iis
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the settings.
		/// </summary>
		/// <value> The settings. </value>
		internal ApplicationSettings Settings
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the storage.
		/// </summary>
		/// <value> The storage. </value>
		internal Storage Storage
		{
			get;
			set;
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="Core" /> class.
		/// </summary>
		public Core()
		{
			this.isDisposed = false;
			this.Settings = new ApplicationSettings();
			this.Storage = new Storage();
			this.Storage.StoragePropertyChanged += this.Storage_StoragePropertyChanged;
			this.Iis = new Iis();
		}

		/// <summary>
		/// 	Releases unmanaged resources and performs other cleanup operations before the <see cref="Core" /> is reclaimed by garbage collection.
		/// </summary>
		~Core()
		{
			this.Storage.StoragePropertyChanged -= this.Storage_StoragePropertyChanged;
			this.Dispose();
		}

		/// <summary>
		/// 	Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if(this.isDisposed)
			{
				return;
			}

			if(this.Storage != null)
			{
				this.Storage.Dispose();
			}

			if(this.Iis != null)
			{
				this.Iis.Dispose();
			}

			this.isDisposed = true;
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 	Creates the root dir.
		/// </summary>
		/// <param name="directoryPath"> The directory path. </param>
		private void CreateRootDir(string directoryPath)
		{
			Trace.TraceInformation("CreateRootDir...");

			try
			{
				Directory.CreateDirectory(directoryPath);
			}
			finally
			{
				Trace.TraceInformation("CreateRootDir...finished.");
			}
		}

		/// <summary>
		/// 	Removes the root dir.
		/// </summary>
		private void RemoveRootDir(string directoryPath)
		{
			Trace.TraceInformation("RemoveRootDir...");

			try
			{
				if(Directory.Exists(directoryPath))
				{
					Directory.Delete(directoryPath, true);
				}
			}
			finally
			{
				Trace.TraceInformation("RemoveRootDir...finished.");
			}
		}

		/// <summary>
		/// 	Handles the StoragePropertyChanged event of the Storage control.
		/// </summary>
		/// <param name="sender"> The source of the event. </param>
		/// <param name="e"> The <see cref="WebDavWhs.StoragePropertyEventArgs" /> instance containing the event data. </param>
		private void Storage_StoragePropertyChanged(object sender, StoragePropertyEventArgs e)
		{
		}
	}
}