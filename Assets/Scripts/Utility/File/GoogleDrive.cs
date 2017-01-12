
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
	/// Gets google drive document if file found. On fail this will return the local
	/// document. 
	/// </summary>
	/// <returns>The drive document.</returns>
	/// <param name="fileName">File name.</param>
	public static string GetOnlineDriveDocument(string fileName)
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
            FileManager.SaveFile(fileName, content);
		}
		catch (WebException e)
		{
            if (e.Status == WebExceptionStatus.ProtocolError && e.Response != null)
            {
                var resp = (HttpWebResponse)e.Response;
                if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    // if this happens this means the server has returned an error in 
                    // the form of a status code of 404. The text associated can be
                    // found on the webpage which will be opened if this is being
                    // run in the unity editor
                    webError = true;
                }
            }

            Debug.LogError(e);
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
        	content = null;
        }

       	return content;	
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
