using System;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

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
	/// Builds the path.
	/// </summary>
	/// <returns>The path.</returns>
	/// <param name="fileName">File name.</param>
	private static string buildPath(string fileName)
	{
		return Path.Combine(Application.persistentDataPath, fileName);
	}

	/// <summary>
	/// Saves content to the file
	/// </summary>
	/// <returns>The file.</returns>
	/// <param name="fileName">File name.</param>
	/// <param name="content">Content.</param>
	private static void saveFile(string fileName, string content)
	{
		StreamWriter writer = new StreamWriter(GoogleDrive.buildPath(fileName), false, System.Text.Encoding.UTF8);
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

		StreamReader reader = new StreamReader(GoogleDrive.buildPath(fileName), System.Text.Encoding.UTF8);
		return reader.ReadToEnd();
	}

	/// <summary>
	/// Gets google drive document if file found. On fail this will return the local
	/// document. 
	/// </summary>
	/// <returns>The drive document.</returns>
	/// <param name="fileName">File name.</param>
	private static string getOnlineDriveDocument(string fileName)
	{
		// create url
        Uri uri;
        Uri.TryCreate(GoogleDrive.GetterURL, UriKind.Absolute, out uri);
        Uri.TryCreate(uri, fileName, out uri);

        SystemLogger.Write("Pulling remote document from " + uri.AbsoluteUri);

		// variables for code
		string content           = "";
		HttpWebResponse response = null;
		HttpWebRequest request   = null;
		bool webError            = false;

		try
		{
            // set certificates
            ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallback;

			// create request and set method type
			request        = (HttpWebRequest)HttpWebRequest.Create(uri.AbsoluteUri);
            request.Method = GoogleDrive.MethodType;

            // get response from the server
            response = (HttpWebResponse)request.GetResponse();

			// read in data
			StreamReader sr = new StreamReader(response.GetResponseStream());
            content         = sr.ReadToEnd();

            // save content to file for usage in case internet is lost
            GoogleDrive.saveFile(fileName, content);
		}
		catch (WebException e)
		{
			// if this happens this means the server has returned an error in 
			// the form of a status code of 404. The text associated can be
			// found on the webpage which will be opened if this is being
			// run in the unity editor

            Debug.LogError(e);
			webError = true;
		}

        // on error, tell the user what went wrong and return a version of
        // the local document if possible.
        if(webError)
        {
        	// if this is the unity editor, open the webpage so the developer 
        	// can see what is wrong.
			#if UNITY_EDITOR
        	UnityEngine.Application.OpenURL(uri.AbsoluteUri);
        	#endif

        	SystemLogger.Write("Remote document unable to be found.");
			content = GoogleDrive.getLocalDocument(fileName);
        }

       	return content;	
	}

	/// <summary>
	/// Gets a file from google drive or returns a local version. If neither of these 
	/// options are available than null will be returned. 
	/// </summary>
	/// <returns>The drive document.</returns>
	/// <param name="fileName">File name.</param>
	public static string GetDriveDocument(string fileName)
	{
		SystemLogger.Write("Getting file from google drive");

		string fileContents = null;

		#if UNITY_EDITOR
		if(GoogleDrive.ConnectedToInternet())
		{
			fileContents = GoogleDrive.getOnlineDriveDocument(fileName);
		}
		else
		{
			fileContents = GoogleDrive.getLocalDocument(fileName);
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
    /// Validates certification errors that can occur on Windows.
    /// </summary>
    public static bool CertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;

        // If there are errors in the certificate chain, look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; ++i)
            {
                if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                    }
                }
            }
        }

        return isOk;
    }
}
