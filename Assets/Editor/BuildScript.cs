using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class BuildScript : MonoBehaviour
{
    private const string yamlDirectory = "Assets/Resources/YAMLFiles";
    private const string executable = "Highwater.exe";

    /// <summary>
    /// Build for windows including post process steps.
    /// </summary>
    [MenuItem("Build/Windows Build With Postprocess")]
    public static void BuildGame ()
    {
        // Get filename
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        // Build player
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + "/" + executable, BuildTarget.StandaloneWindows, BuildOptions.None);

        BuildScript.CopyYAMLFiles(path);
    }

    /// <summary>
    /// Post process step for copying YAML files for build directory
    /// </summary>
    /// <param name="path">The build path.</param>
    public static void CopyYAMLFiles (string path)
    {
        // Copy all files from the yaml directory 
        DirectoryInfo dir = new DirectoryInfo(yamlDirectory);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo file in info)
        {
            FileUtil.CopyFileOrDirectory(file.FullName, path + '/' + file.Name);
        }
    }
}
