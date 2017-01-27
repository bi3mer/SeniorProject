using UnityEngine;
using System.IO;

public class FileManager 
{
	/// <summary>
	/// The check mark menu.
	/// </summary>
	public const string UseLocalFiles = "File/Use Local Files";

	private const string allFilesLocation = "all_files";

	/// <summary>
	/// Builds the path.
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="fileName">File name.</param>
	public static string BuildPath(string fileName)
	{
		return Path.Combine(Application.persistentDataPath, fileName);
	}

	/// <summary>
	/// Saves content to the file
	/// </summary>
	/// <returns>The file.</returns>
	/// <param name="fileName">File name.</param>
	/// <param name="content">Content.</param>
	public static void SaveFile(string fileName, string content)
	{
		StreamWriter writer = new StreamWriter(FileManager.BuildPath(fileName), false, System.Text.Encoding.UTF8);
		writer.WriteLine(content);
		writer.Close();
	}

	/// <summary>
	/// Gets the the file from a local resource.
	///
	/// This will throw an exception if the file does not exist. 
	/// This is as on purpose as the game will throw an exception 
	/// elsewhere if an empty file is found. Therefore, if this
	/// exception is thrown the system logger will make it easy
	/// for debuggers to find that this is the problem spot.
	/// </summary>
	/// <returns>The local document.</returns>
	/// <param name="fileName">File name.</param>
	private static string getLocalDocument(string fileName)
	{
		SystemLogger.Write("Pulling local document.");

		StreamReader reader = new StreamReader(FileManager.BuildPath(fileName), System.Text.Encoding.UTF8);
		return reader.ReadToEnd();
	}

	/// <summary>
	/// Gets the document using google drive, if possible when in
	/// editor mode.
	/// </summary>
	/// <returns>The document.</returns>
	/// <param name="fileName">File name.</param>
	public static string GetDocument(string fileName)
	{
		SystemLogger.Write("Getting file from google drive");

		string fileContents = null;

		#if UNITY_EDITOR
		if(!UnityEditor.EditorPrefs.GetBool(FileManager.UseLocalFiles) && GoogleDrive.ConnectedToInternet())
		{
			fileContents = GoogleDrive.GetOnlineDriveDocument(fileName);

			// get local document if nothing was found
			if(fileContents == null)
			{
				fileContents = FileManager.getLocalDocument(fileName);
			}
		}
		else
		{
			fileContents = FileManager.getLocalDocument(fileName);
		}
		#else
		// pull local contents when this is a real build to avoid 
		// a mishap where someone accidentally deletes the google
		// drive file and corrupts anyone soul who wants to play
		// this game.
		fileContents = GoogleDrive.getLocalDocument(fileName);
		#endif

		return fileContents;
	}

	/// <summary>
	/// Saves all documents from google drive.
	/// </summary>
	public static void SaveAllDocuments()
	{
		string[] files = FileManager.GetDocument(FileManager.allFilesLocation).Split(' ');

		for(int i = 0; i < files.Length; ++i)
		{
			// This will get the document and save it
			FileManager.GetDocument(files[i]);
		}
	}
}