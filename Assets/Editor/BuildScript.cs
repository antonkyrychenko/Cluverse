using System.Linq;
using UnityEditor;

public static class BuildScript
{
  // This method is called by Codemagic to build iOS
  public static void BuildIos()
  {
    // Path to output Xcode project
    string path = "ios";

    // Get all enabled scenes
    string[] scenes = EditorBuildSettings.scenes
        .Where(s => s.enabled)
        .Select(s => s.path)
        .ToArray();

    // Build iOS Xcode project
    BuildPlayerOptions options = new BuildPlayerOptions
    {
      scenes = scenes,
      locationPathName = path,
      target = BuildTarget.iOS,
      options = BuildOptions.None
    };

    BuildPipeline.BuildPlayer(options);
  }
}
