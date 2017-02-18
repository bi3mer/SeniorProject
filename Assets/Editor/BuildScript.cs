using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class BuildScript : MonoBehaviour
{
    private const string yamlDirectory = "Assets/Resources/YAMLFiles";
    private const string executable = "Highwater";

    /// <summary>
    /// Build for windows including post process steps.
    /// </summary>
    [MenuItem("Build/Windows Build With Packaged YAML files")]
    public static void BuildGame ()
    {
        // Get filename
        string buildPath = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        // Build player
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Path.Combine(buildPath, executable + ".exe"), BuildTarget.StandaloneWindows, BuildOptions.None);

        BuildScript.CopyYAMLFiles(buildPath);
    }

    /// <summary>
    /// Post process step for copying YAML files for build directory
    /// </summary>
    /// <param name="buildPath">The build path.</param>
    public static void CopyYAMLFiles (string buildPath)
    {
        // Copy all yaml files to the build directory
        DirectoryInfo dir = new DirectoryInfo(yamlDirectory);
        FileInfo[] info = dir.GetFiles("*.yml", SearchOption.TopDirectoryOnly);
        foreach (FileInfo file in info)
        {
            string basePath = Path.Combine(buildPath, executable + "_Data");
            string path = Path.Combine(basePath, file.Name);
            FileUtil.CopyFileOrDirectory(file.FullName, path);
        }
    }
}
