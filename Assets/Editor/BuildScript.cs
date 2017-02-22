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
    public static void BuildGameWindows ()
    {
        buildGame(BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    /// <summary>
    /// Development build for windows including post process steps.
    /// </summary>
    [MenuItem("Build/Windows Development Build With Packaged YAML files")]
    public static void BuildGameWindowsDevelopment()
    {
        buildGame(BuildTarget.StandaloneWindows, BuildOptions.Development);
    }

    /// <summary>
    /// Builds for macOSx including post process steps.
    /// </summary>
    [MenuItem("Build/MacOSx Build With Packaged YAML files")]
    public static void BuildGameMacOSx()
    {
        buildGame(BuildTarget.StandaloneOSXIntel64, BuildOptions.None);
    }

    /// <summary>
    /// Development builds for macOSx including post process steps.
    /// </summary>
    [MenuItem("Build/MacOSx Development Build With Packaged YAML files")]
    public static void BuildGameMacOSxDevelopment()
    {
        buildGame(BuildTarget.StandaloneOSXIntel64, BuildOptions.Development);
    }

    /// <summary>
    /// Builds the game with the specified paramters.
    /// </summary>
    /// <param name="target">Target build platform.</param>
    /// <param name="options">Build options</param>
    private static void buildGame(BuildTarget target, BuildOptions options)
    {
        // Set up settings for each platform
        string buildPath = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string executableName = executable;
        if (target == BuildTarget.StandaloneWindows)
        {
            executableName += ".exe";
        }
        else
        {
            executableName += ".app";
        }

        // Build player
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, Path.Combine(buildPath, executableName), target, options);

        if (target == BuildTarget.StandaloneWindows)
        {
            BuildScript.CopyYAMLFilesWindows(buildPath);
        }
        else
        {
            BuildScript.CopyYAMLFilesMac(buildPath);
        }
    } 

    /// <summary>
    /// Post process step for copying YAML files for windows build directory
    /// </summary>
    /// <param name="buildPath">The build path.</param>
    public static void CopyYAMLFilesWindows (string buildPath)
    {
		// Copy all yaml files to the build directory
		DirectoryInfo dir = new DirectoryInfo(yamlDirectory);
		FileInfo[] info = dir.GetFiles("*.yml", SearchOption.TopDirectoryOnly);

		// create directory for yaml files in the buildpath location
		string basePath = Path.Combine(buildPath, executable + "_Data");
		System.IO.Directory.CreateDirectory (basePath);

		// copy each yaml file to new location
		foreach (FileInfo file in info)
		{
			string path = Path.Combine(basePath, file.Name);
			FileUtil.CopyFileOrDirectory(file.FullName, path);
		}
    }

	/// <summary>
	/// Post process step for copying YAML files for mac build directory
	/// </summary>
	/// <param name="buildPath">The build path.</param>
	public static void CopyYAMLFilesMac (string buildPath)
	{
		// Copy all yaml files to the build directory
		DirectoryInfo dir = new DirectoryInfo(yamlDirectory);
		FileInfo[] info = dir.GetFiles("*.yml", SearchOption.TopDirectoryOnly);

		// create directory for yaml files in the buildpath location
		string basePath = Path.Combine(buildPath, executable + ".app/Contents");
		System.IO.Directory.CreateDirectory (basePath);

		// copy each yaml file to new location
		foreach (FileInfo file in info)
		{
			string path = Path.Combine(basePath, file.Name);
			FileUtil.CopyFileOrDirectory(file.FullName, path);
		}
	}
}
