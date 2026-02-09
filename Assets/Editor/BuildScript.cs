// Assets/Editor/BuildScript.cs
using System.IO;
using UnityEditor;

public class BuildScript
{
  public static void BuildiOS()
  {
    UnityEngine.Debug.Log("BuildiOS method started");
    string buildPath = Path.Combine(System.Environment.CurrentDirectory, "ios");
    if (!Directory.Exists(buildPath))
    {
      UnityEngine.Debug.Log("Build directory doesn't exist. Creating new one");
      Directory.CreateDirectory(buildPath);
    }

    BuildPipeline.BuildPlayer(
        EditorBuildSettings.scenes,
        buildPath,
        BuildTarget.iOS,
        BuildOptions.None
    );

    UnityEngine.Debug.Log("Xcode project generated at: " + buildPath);
    foreach (var file in Directory.GetFiles(buildPath, "*", SearchOption.AllDirectories))
    {
      UnityEngine.Debug.Log(file);
    }

    UnityEngine.Debug.Log("BuildPipeline.BuildPlayer completed");
  }
}
