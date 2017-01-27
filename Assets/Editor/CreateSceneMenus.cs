using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateSceneMenus : Editor
{
	public static string FilePath = Path.Combine(Path.Combine("Assets", "Editor"), "SceneMenu.cs");
	public static string OpeningString = "using UnityEngine;using System.Collections;using UnityEditor;\n"
		                               + "public class SceneMenu : Editor {\n";

	public static string EndingString = "public static void OpenScene(string scene){ " 
	  	                              + "if(EditorApplication.SaveCurrentSceneIfUserWantsTo()){"
		                              + "EditorApplication.OpenScene(scene);}}}";

	[MenuItem("Open Scene/Create Scenes")]
	public static void CreateScenes()
	{
		// Start string
		string completeFile = CreateSceneMenus.OpeningString;

		// get all paths
		string[] paths = AssetDatabase.GetAllAssetPaths();

		// loop through each path
		for(int i = 0; i < paths.Length; ++i)
		{
			// check if this is a scene
			if(paths[i].Contains(".unity"))
			{
				// add scene header
				completeFile += "[MenuItem(\"Open Scene/" + paths[i] + "\")]";

				// create function with unique name
				completeFile += "public static void Open" + i.ToString() + "(){SceneMenu.OpenScene(\"" + paths[i] + "\");}\n";
			}
		}

		// add ending
		completeFile += CreateSceneMenus.EndingString;

		// print file to file
		System.IO.StreamWriter writer = System.IO.File.CreateText(CreateSceneMenus.FilePath);
		writer.Write(completeFile);
		writer.Flush();
		writer.Close();

		// Re build unity
		AssetDatabase.Refresh();
	}
}
