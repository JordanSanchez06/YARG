using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Application = UnityEngine.Application;
using CompressionLevel = System.IO.Compression.CompressionLevel;

public class BuildStreamingAssetsZipper : IPostprocessBuildWithReport
{
    public int callbackOrder => 999;

    public void OnPostprocessBuild(BuildReport report)
    {
        // Only run for Android builds
        if (report.summary.platform != BuildTarget.Android)
        {
            Debug.Log("[BuildStreamingAssetsZipper] Skipping zip — not an Android build.");
            return;
        }

        string tempZipPath = Path.Combine(Application.dataPath, "StreamingAssets.zip");
        string finalZipPath = Path.Combine(Application.streamingAssetsPath, "StreamingAssets.zip");

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Debug.LogWarning("[BuildStreamingAssetsZipper] No StreamingAssets folder found — skipping zip step.");
            return;
        }

        // Delete old zip if it exists
        if (File.Exists(tempZipPath))
        {
            Debug.Log("deleting zip in assets");
            File.Delete(tempZipPath);
        }
        if (File.Exists(finalZipPath))
        {
            Debug.Log("deleting zip in streamingassets");
            File.Delete(finalZipPath);
        }

        try
        {
            ZipFile.CreateFromDirectory(Application.streamingAssetsPath, tempZipPath, CompressionLevel.Optimal, includeBaseDirectory: false);
            Debug.Log($"[BuildStreamingAssetsZipper] Created zip: {tempZipPath}");
            File.Move(tempZipPath, finalZipPath);
            Debug.Log($"[BuildStreamingAssetsZipper] Moved zip to final destination: {Application.streamingAssetsPath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[BuildStreamingAssetsZipper] Failed to zip StreamingAssets: {ex}");
        }
    }
}
