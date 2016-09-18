using System.IO;
using System.Diagnostics;
using UnityEngine;

public static class SystemLogger
{
	// File to log to
	private static string outputFilePath = "logfile.txt";

	// Configuration for whether to use logging in build or not
	// NOTE: for all pushes this should have builds set to false, only 
	//       change for testing.
	#if UNITY_EDITOR
	public static bool logToFile = true;
	#else
	public static bool logToFile = false;
	#endif

	// Initializes FileStream for output into specified log file
	private static StreamWriter writer = new StreamWriter(outputFilePath, false, System.Text.Encoding.UTF8);

	/// <summary>
	///     Writes event to log file specified above.
	/// </summary>
	/// <param name="output"></param>
	public static void write(string output)
	{
		if (logToFile)
		{
			StackTrace st = new StackTrace(true);

			int lineNumber = st.GetFrame(1).GetFileLineNumber();

			// Output format:  "(Time delta from program start in format m:ss.sss) :: (Log message)"
			writer.WriteLine(string.Format("{0:00}:{1:00.000}", Time.realtimeSinceStartup / 60, Time.realtimeSinceStartup % 60f) + " :: \"" + st.GetFrame(1).GetMethod().ReflectedType.Name + "\" (Line: " + lineNumber + ") :: " + output);

			// Flushes buffer to force a write
			writer.Flush();
		}
	}
}