using UnityEngine;
using System.Net;
using System.IO;

public class GoogleDrive
{
	public static string GoogleURL  = "http://www.google.com";
	public static string GetterURL  = "https://glassprisongames-googlegetter.herokuapp.com";
	public static string MethodType = "GET";

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
	/// Gets google drive document if file found. On fail this will return the local
	/// document. 
	/// </summary>
	/// <returns>The drive document.</returns>
	/// <param name="fileName">File name.</param>
	public static string GetOnlineDriveDocument(string fileName)
	{
		// create url
		string url = Path.Combine(GoogleDrive.GetterURL,fileName);

		SystemLogger.Write("Pulling remote document from " + url);

		// variables for code
		string content           = "";
		HttpWebResponse response = null;
		HttpWebRequest request   = null;
		bool webError            = false;

		try
		{
			// create request and set method type
			request        = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = GoogleDrive.MethodType;

            // get response from the server
            response = (HttpWebResponse)request.GetResponse();

			// read in data
			StreamReader sr = new StreamReader(response.GetResponseStream());
            content         = sr.ReadToEnd();

            // save content to file for usage in case internet is lost
            FileManager.SaveFile(fileName, content);
		}
		catch
		{
			// if this happens this means the server has returned an error in 
			// the form of a status code of 404. The text associated can be
			// found on the webpage which will be opened if this is being
			// run in the unity editor
			webError = true;
		}


        // on error, tell the user what went wrong and return a version of
        // the local document if possible.
        if(webError)
        {
        	// if this is the unity editor, open the webpage so the developer 
        	// can see what is wrong.
			#if UNITY_EDITOR
        	UnityEngine.Application.OpenURL(url);
        	#endif

        	SystemLogger.Write("Remote document unable to be found.");
        	content = null;
        }

       	return content;	
	}
}