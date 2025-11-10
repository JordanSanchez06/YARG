#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

public static class StoragePermissionHelper
{
    public static void PromptAllFilesAccess()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
        using (var intent = new AndroidJavaObject(
            "android.content.Intent",
            "android.settings.MANAGE_APP_ALL_FILES_ACCESS_PERMISSION",
            new AndroidJavaClass("android.net.Uri")
                .CallStatic<AndroidJavaObject>("parse", "package:" + Application.identifier)))
        {
            Debug.Log("Prompting user for MANAGE_APP_ALL_FILES_ACCESS_PERMISSION");
            activity.Call("startActivity", intent);
        }
    }

    public static bool HasAllFilesAccess()
    {
        using (var env = new AndroidJavaClass("android.os.Environment"))
        {
            bool hasAccess = env.CallStatic<bool>("isExternalStorageManager");
            Debug.Log("HasAllFilesAccess(): " + hasAccess);
            return hasAccess;
        }
    }
}
#endif
