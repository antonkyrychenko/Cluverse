// Assets/Editor/BuildScript.cs
using UnityEditor;

public class BuildScript
{
  public static void BuildiOS()
  {
    BuildPipeline.BuildPlayer(
        EditorBuildSettings.scenes,
        "ios",
        BuildTarget.iOS,
        BuildOptions.None
    );
  }
}
