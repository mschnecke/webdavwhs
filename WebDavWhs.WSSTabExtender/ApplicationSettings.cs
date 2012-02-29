//----------------------------------------------------------------------------------------
// <copyright file="ApplicationSettings.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WebDavWhs
{
	/// <summary>
	/// 	The application settings.
	/// </summary>
	[Serializable]
	public class ApplicationSettings
	{
		/// <summary>
		/// Gets or sets a value indicating whether [enable logging].
		/// </summary>
		/// <value>
		///   <c>true</c> if [enable logging]; otherwise, <c>false</c>.
		/// </value>
		public bool EnableLogging
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets the virtual directory alias.
		/// </summary>
		/// <value> The virtual directory alias. </value>
		public string VirtualDirectoryAlias
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets or sets a value indicating whether WebDAV is enabled or not.
		/// </summary>
		/// <value> <c>true</c> if enabled; otherwise, <c>false</c> . </value>
		public bool WebDavEnabled
		{
			get;
			set;
		}

		/// <summary>
		/// 	Gets the application data folder.
		/// </summary>
		[XmlIgnore]
		public string ApplicationDataFolder
		{
			get
			{
				return GetApplicationDataFolder();
			}
		}

		/// <summary>
		/// 	Initializes a new instance of the <see cref="ApplicationSettings" /> class.
		/// </summary>
		public ApplicationSettings()
		{
			this.VirtualDirectoryAlias = string.Empty;
			this.WebDavEnabled = false;
		}

		/// <summary>
		/// 	Loads the settings.
		/// </summary>
		public static ApplicationSettings LoadSettings()
		{
			string settingsFile = GetSettingsFilePath();
			object obj = Deserialize(settingsFile, typeof(ApplicationSettings));

			ApplicationSettings settings = obj as ApplicationSettings;

			if(settings == null)
			{
				return new ApplicationSettings();
			}

			return settings;
		}

		/// <summary>
		/// 	Saves the settings.
		/// </summary>
		public static void SaveSettings(ApplicationSettings settings)
		{
			string settingsFile = GetSettingsFilePath();
			Serialize(settingsFile, settings, typeof(ApplicationSettings));
		}

		/// <summary>
		/// 	Serializes the specified object.
		/// </summary>
		/// <param name="fileName"> Name of the file. </param>
		/// <param name="objectToSave"> The object to save. </param>
		/// <param name="objectType"> Type of the object. </param>
		private static void Serialize(string fileName, object objectToSave, Type objectType)
		{
			ValidateFilePath(fileName);

			using(StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.UTF8))
			{
				XmlSerializer serializer = new XmlSerializer(objectType);
				serializer.Serialize(streamWriter, objectToSave);
			}
		}

		/// <summary>
		/// 	Deserializes.
		/// </summary>
		/// <param name="fileName"> Name of the file. </param>
		/// <param name="objectType"> Type of the object. </param>
		/// <returns> </returns>
		private static object Deserialize(string fileName, Type objectType)
		{
			ValidateFilePath(fileName);

			using(StreamReader streamReader = new StreamReader(fileName, Encoding.UTF8))
			{
				XmlSerializer serializer = new XmlSerializer(objectType);
				return serializer.Deserialize(streamReader);
			}
		}

		/// <summary>
		/// 	Gets the settings file path.
		/// </summary>
		/// <returns> </returns>
		private static string GetSettingsFilePath()
		{
			string directoryPath = GetApplicationDataFolder();
			return Path.Combine(directoryPath, "Settings.xml");
		}

		/// <summary>
		/// 	Gets the application data folder.
		/// </summary>
		/// <returns> </returns>
		private static string GetApplicationDataFolder()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"WebDAV for WHS");
		}

		/// <summary>
		/// 	Validates and creates the file path.
		/// </summary>
		/// <param name="filePath"> The file path. </param>
		private static void ValidateFilePath(string filePath)
		{
			string directoryName = Path.GetDirectoryName(filePath);

			if(directoryName != null)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);

				if(!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
			}

			FileInfo fileInfo = new FileInfo(filePath);

			if(fileInfo.Exists == false)
			{
				fileInfo.CreateText().Close();
			}
		}
	}
}