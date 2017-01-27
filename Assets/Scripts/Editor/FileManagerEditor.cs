using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class FileManagerEditor : MonoBehaviour 
{
	/// <summary>
	/// If should be connected to internet
	/// </summary>
	public static bool Connected;

	/// <summary>
	/// Gets the documents from google drive and save locally
	/// </summary>
	[MenuItem("File/Get Drive Documents")]
	public static void GetDocuments()
	{
		FileManager.SaveAllDocuments();
		Debug.Log("Probably saved all files.");
	}

	/// <summary>
	/// Create variable to be used by google drive to define 
	/// whether or not we are connected to the internet.
	/// </summary>
	[MenuItem(FileManager.UseLocalFiles)]
	public static void UseLocalFiles()
	{
		FileManagerEditor.Connected = !FileManagerEditor.Connected;
		EditorPrefs.SetBool(FileManager.UseLocalFiles, FileManagerEditor.Connected);

		// set editor view
		Menu.SetChecked(FileManager.UseLocalFiles, FileManagerEditor.Connected);
	}

	/// <summary>
	/// Initializes the <see cref="FileManagerEditor"/> class on load.
	/// http://answers.unity3d.com/questions/775869/editor-how-to-add-checkmarks-to-menuitems.html
	/// </summary>
	static FileManagerEditor() 
	{
		FileManagerEditor.Connected = EditorPrefs.GetBool(FileManager.UseLocalFiles, false);
 
		/// Delaying until first editor tick so that the menu will be populated correctly
		EditorApplication.delayCall += () => {
			// getting the opposite of what is expected since the next function call
			// will set the opposite of the value
			FileManagerEditor.Connected = !EditorPrefs.GetBool(FileManager.UseLocalFiles);
			FileManagerEditor.UseLocalFiles();
		};
     }
}
