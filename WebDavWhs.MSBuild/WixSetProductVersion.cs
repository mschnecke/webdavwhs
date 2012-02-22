using System;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace WebDavWhs.MSBuild
{
	/// <summary>
	/// 	Sets the ProductVersion property in the Wix project file.
	/// </summary>
	public class WixSetProductVersion : Task
	{
		/// <summary>
		/// Gets or sets file path to the version.wxi.
		/// </summary>
		[Required]
		public string FilePath
		{
			private get;
			set;
		}

		/// <summary>
		/// 	Sets the version number.
		/// </summary>
		[Required]
		public string Version
		{
			private get;
			set;
		}

		/// <summary>
		/// 	Executes this instance.
		/// </summary>
		/// <returns>
		/// Success statement. 
		/// </returns>
		public override bool Execute()
		{
			this.Log.LogMessage(MessageImportance.High, "Task: WixSetProductVersion");

			this.Log.LogMessage("FilePath:");
			this.Log.LogMessage("\t{0}", this.FilePath);
			this.Log.LogMessage("Version: {0}", this.Version);

			if(string.IsNullOrEmpty(this.FilePath))
			{
				this.Log.LogError("The WiX project file path is not defined.");
				return false;
			}

			if(string.IsNullOrEmpty(this.Version))
			{
				this.Log.LogError("The version number is not defined.");
				return false;
			}

			return this.SetVersion(this.FilePath, this.Version);
		}

		/// <summary>
		/// 	Sets the version.
		/// </summary>
		/// <param name="filePath">
		/// The file path. 
		/// </param>
		/// <param name="version">
		/// The version. 
		/// </param>
		/// <returns>
		/// The set version.
		/// </returns>
		private bool SetVersion(string filePath, string version)
		{
			XmlTextReader xmlTextReader = new XmlTextReader(filePath);
			XmlDocument xmlDocument = new XmlDocument();

			try
			{
				xmlDocument.Load(xmlTextReader);
			}
			catch(Exception exception)
			{
				this.Log.LogErrorFromException(exception, true);
				return false;
			}
			finally
			{
				xmlTextReader.Close();
			}

			XmlNode root;

			try
			{
				root = xmlDocument.DocumentElement;
			}
			catch(Exception exception)
			{
				this.Log.LogErrorFromException(exception, true);
				return false;
			}

			if(root == null)
			{
				return false;
			}

			if(root.HasChildNodes == false)
			{
				return false;
			}

			try
			{
				XmlNodeList nodeList = root.ChildNodes;

				foreach(XmlNode node in nodeList)
				{
					if(string.Compare(node.Name, "define", true) != 0)
					{
						continue;
					}

					if(string.IsNullOrEmpty(node.Value))
					{
						continue;
					}

					if(node.Value.Contains("ProductVersion=") == false)
					{
						continue;
					}

					try
					{
						node.Value = string.Format("ProductVersion=\"{0}\"", version);
					}
					catch(Exception exception)
					{
						this.Log.LogErrorFromException(exception, true);
					}
				}
			}
			catch(Exception exception)
			{
				this.Log.LogErrorFromException(exception, true);
				return false;
			}

			// write project file
			XmlTextWriter xmlTextWriter = new XmlTextWriter(filePath, Encoding.UTF8);

			try
			{
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlDocument.Save(xmlTextWriter);
				return true;
			}
			catch(Exception exception)
			{
				this.Log.LogErrorFromException(exception, true);
				return false;
			}
			finally
			{
				xmlTextWriter.Close();
			}
		}
	}
}