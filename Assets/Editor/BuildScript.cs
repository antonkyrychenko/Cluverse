// Assets/Editor/BuildScript.cs
using System;
using System.IO;
using UnityEditor;

public class BuildScript
{
  public static void BuildiOS()
  {
    Console.WriteLine("BuildiOS method started");
    string buildPath = Path.Combine(System.Environment.CurrentDirectory, "ios");
    if (!Directory.Exists(buildPath))
    {
      Console.WriteLine("Build directory doesn't exist. Creating new one");
      Directory.CreateDirectory(buildPath);
    }

    BuildPipeline.BuildPlayer(
        EditorBuildSettings.scenes,
        buildPath,
        BuildTarget.iOS,
        BuildOptions.None
    );

    Console.WriteLine("Xcode project generated at: " + buildPath);
    foreach (var file in Directory.GetFiles(buildPath, "*", SearchOption.AllDirectories))
    {
      Console.WriteLine(file);
    }

    Console.WriteLine("BuildPipeline.BuildPlayer completed");
  }
}
