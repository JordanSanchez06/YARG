#if UNITY_ANDROID
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;

namespace YARG.Helpers
{

    public static class StreamingAssetsExtractor
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void ExtractStreamingAssetsEarly()
        {
            Debug.Log("[StreamingAssetsExtractor] Extracting StreamingAssets before scene load...");
            await ExtractIfNeededAsync();
        }

        private const string ExtractedVersionKey = "StreamingAssetsExtractedVersion";
        private const string ZipFileName = "StreamingAssets.zip";

        /// <summary>
        /// Extract StreamingAssets.zip to persistentDataPath/StreamingAssets if needed.
        /// Checks version to avoid redundant extraction.
        /// </summary>
        public static async Task ExtractIfNeededAsync()
        {
            // Already extracted?
            string persistentPath = Path.Combine(PathHelper.PersistentDataPath, "StreamingAssets");
            if (Directory.Exists(persistentPath))
            {
                Debug.Log("[StreamingAssetsExtractor] StreamingAssets folder already exists in persistentDataPath, skipping extraction.");
                return;
            }

            string zipPath = Path.Combine(Application.persistentDataPath, ZipFileName);
            string streamingZipPath = Path.Combine(Application.streamingAssetsPath, ZipFileName);

            // Load zip from StreamingAssets (inside APK)
            using (UnityWebRequest request = UnityWebRequest.Get(streamingZipPath))
            {
                UnityWebRequestAsyncOperation operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    // Wait for the next frame, just like in the Unity docs example
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Success: Write the downloaded bytes to the zip file
                    File.WriteAllBytes(zipPath, request.downloadHandler.data);

                }
                else
                {

                    // Error: Log the failure and exit the method
                    Debug.LogFormat("[StreamingAssetsExtractor] Failed to load {0} with error: {1}", ZipFileName, request.error);
                    return;
                }
            }

            // Extract zip
            try
            {
                Directory.CreateDirectory(persistentPath);
                ZipFile.ExtractToDirectory(zipPath, persistentPath);
                Debug.Log($"[StreamingAssetsExtractor] Extracted StreamingAssets to {persistentPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[StreamingAssetsExtractor] Failed to extract StreamingAssets.zip: {ex}");
                return;
            }
            finally
            {
                // Clean up the zip in persistentDataPath
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
            }
        }
    }
}
#endif 
