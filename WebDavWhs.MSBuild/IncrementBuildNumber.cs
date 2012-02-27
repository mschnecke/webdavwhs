//----------------------------------------------------------------------------------------
// <copyright file="IncrementBuildNumber.cs" >
//     Copyright (c) 2012, Michael Schnecke, Göran Watzke. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace WebDavWhs.MSBuild
{
	/// <summary>
	/// 	Increments the the build number in the AssemblyInfo.cs file.
	/// </summary>
	public class IncrementBuildNumber : Task
	{
		/// <summary>
		/// 	The full path to the AssemblyInfo file.
		/// </summary>
		private string strFilePath;

		/// <summary>
		/// 	The version in the AssemblyInfo file.
		/// </summary>
		private Version version;

		/// <summary>
		/// Sets the full path to the AssemblyInfo file.
		/// </summary>
		/// <value>
		/// The file path.
		/// </value>
		[Required]
		public string FilePath
		{
			set
			{
				this.strFilePath = ValidateString(value);
			}
		}

		/// <summary>
		/// 	Gets the version string.
		/// </summary>
		/// <value> The version string. </value>
		[Output]
		public string VersionString
		{
			get
			{
				return this.version == null ? string.Empty : this.version.ToString(4);
			}
		}

		/// <summary>
		/// 	Executes this instance.
		/// </summary>
		/// <returns>Task result.</returns>
		public override bool Execute()
		{
			this.Log.LogMessage(MessageImportance.High, "Task: IncrementBuildNumber");

			if(string.IsNullOrEmpty(this.strFilePath))
			{
				this.Log.LogError("The AssemblyInfo file path is not defined.");
				return false;
			}

			this.Log.LogMessage("FilePath:");
			this.Log.LogMessage("\t{0}", this.strFilePath);

			this.version = this.GetVersionFromAssemblyInfo(this.strFilePath);

			if(this.version == null)
			{
				return false;
			}

			this.version = new Version(this.version.Major, this.version.Minor, this.version.Build + 1, this.version.Revision);
			this.Log.LogMessage(MessageImportance.Normal, string.Format("Incremented version: {0}", this.version));

			this.SetVersionInAssemblyInfo(this.strFilePath, this.version);
			return true;
		}

		/// <summary>
		/// 	Gets the version from AssemblyInfo file.
		/// </summary>
		/// <param name="assemblyInfoFile"> The assembly info file. </param>
		/// <returns> The version. </returns>
		private Version GetVersionFromAssemblyInfo(string assemblyInfoFile)
		{
			const string seachItem = "AssemblyVersion";

			try
			{
				Version localVersion = null;

				using(StreamReader sr = new StreamReader(assemblyInfoFile))
				{
					string line;
					while((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf(seachItem, StringComparison.Ordinal) == -1)
						{
							continue;
						}

						string temp = line.Substring(line.IndexOf(seachItem, StringComparison.Ordinal) + seachItem.Length);
						temp = temp.Trim(new[]{
						                      	'[', ']', '(', ')', '"', '*', '.'
						                      });
						localVersion = new Version(temp);
					}
				}

				return localVersion;
			}
			catch(Exception exception)
			{
				this.Log.LogErrorFromException(exception, true);
				return null;
			}
		}

		/// <summary>
		/// 	Sets the version in AssemblyInfo file.
		/// </summary>
		/// <param name="assemblyInfoFile"> The AssemblyInfo file. </param>
		/// <param name="newVersion"> The new version. </param>
		private void SetVersionInAssemblyInfo(string assemblyInfoFile, Version newVersion)
		{
			const string seachItem1 = "AssemblyVersion";
			const string seachItem2 = "AssemblyFileVersion";

			string assemblyVersion = string.Format("[assembly: AssemblyVersion(\"{0}\")]", newVersion);
			string assemblyFileVersion = string.Format("[assembly: AssemblyFileVersion(\"{0}\")]", newVersion);

			StringBuilder stringBuilder = new StringBuilder();

			try
			{
				using(StreamReader sr = new StreamReader(assemblyInfoFile))
				{
					string line;
					while((line = sr.ReadLine()) != null)
					{
						if(line.IndexOf(seachItem1, StringComparison.Ordinal) != -1)
						{
							stringBuilder.Append(assemblyVersion + Environment.NewLine);
						}
						else if(line.IndexOf(seachItem2, StringComparison.Ordinal) != -1)
						{
							stringBuilder.Append(assemblyFileVersion + Environment.NewLine);
						}
						else
						{
							stringBuilder.Append(line + Environment.NewLine);
						}
					}
				}

				using(StreamWriter streamWriter = new StreamWriter(assemblyInfoFile, false))
				{
					streamWriter.Write(stringBuilder.ToString());
				}
			}
			catch(Exception exception)
			{
				this.Log.LogErrorFromException(exception, true);
			}
		}

		/// <summary>
		/// 	Validates the input string.
		/// </summary>
		/// <param name="input"> The input string. </param>
		/// <returns> Empty String, if input string was null. </returns>
		private static string ValidateString(string input)
		{
			return input ?? string.Empty;
		}
	}
}