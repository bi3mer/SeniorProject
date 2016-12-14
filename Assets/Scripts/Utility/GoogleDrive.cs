using UnityEngine;

public class GoogleDrive
{
	public static string GoogleURL = "http://www.google.com";
	public static string GetterURL = "https://glassprisongames-googlegetter.herokuapp.com";

	/// <summary>
	/// Checks if connected to internet.
	/// Warning: This should only as few times as possible as this is an expensive call.
	/// </summary>
	/// <returns><c>true</c>, if connected to internet, <c>false</c> otherwise.</returns>
	public static bool ConnectedToInternet()
	{
		SystemLogger.Write("Checking if conneceted to internet");

		// Google URL is used instead of Heroku since the server on Heroku can take a few
		// seconds to start back up and may cause an edge case where 0 bytes are downloaded
		// despite developers being connected to the internet.
		WWW www = new WWW(GoogleDrive.GoogleURL);

		return www.bytesDownloaded > 0;
	}

	/// <summary>
	/// Gets the the file from a local resource
	/// </summary>
	/// <returns>The local document.</returns>
	/// <param name="fileName">File name.</param>
	private static string getLocalDocument(string fileName)
	{
		SystemLogger.Write("Pulling local document");

		return "empty for now. This will be updated soon.";
	}

	/// <summary>
	/// Gets google drive document if file found. On fail this will return the local
	/// document. 
	/// </summary>
	/// <returns>The drive document.</returns>
	/// <param name="fileName">File name.</param>
	private static string getDriveDocument(string fileName)
	{
		SystemLogger.Write("Pulling remote document");

		/*
		if(retrievedDocument.status == 200)
		{

		}
		else if (retrievedDocument.status == multiple instance status)
		{

		}
		else
		{
			// no instance found
		}
		*/
		return GoogleDrive.getLocalDocument(fileName);
	}

	/// <summary>
	/// Gets a file from google drive or returns a local version. If neither of these 
	/// options are available than null will be returned. 
	/// </summary>
	/// <returns>The drive document.</returns>
	/// <param name="fileName">File name.</param>
	public static string getDriveDoc(string fileName)
	{
		SystemLogger.Write("Getting file from google drive");

		string fileContents = null;

		if(GoogleDrive.ConnectedToInternet())
		{
			Debug.Log("I'm connected!");
		}
		else
		{
			fileContents = GoogleDrive.getLocalDocument(fileName);
		}

		return fileContents;
	}
}
