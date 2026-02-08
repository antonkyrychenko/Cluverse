using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildScript
{
    public static void BuildIos()
    {
        // Force iOS target (important for CI)
        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.iOS,
            BuildTarget.iOS
        );

        string path = "ios";

        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = path,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new System.Exception(
                $"iOS build failed: {report.summary.result}"
            );
        }

        UnityEngine.Debug.Log("✅ iOS Xcode project generated at /ios");
    }
}
