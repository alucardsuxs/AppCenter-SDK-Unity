using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AppCenterEditor
{
    public class AppCenterEditorSDKTools : Editor
    {
        public static bool IsInstalled { get { return GetAppCenterSettings() != null; } }
        private static Type appCenterSettingsType = null;
        private static bool isInitialized; //used to check once, gets reset after each compile;
        private static UnityEngine.Object sdkFolder;

        public static Type GetAppCenterSettings()
        {
            if (appCenterSettingsType == typeof(object))
                return null; // Sentinel value to indicate that PlayFabSettings doesn't exist
            if (appCenterSettingsType != null)
                return appCenterSettingsType;

            appCenterSettingsType = typeof(object); // Sentinel value to indicate that PlayFabSettings doesn't exist
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssemblies)
                foreach (var eachType in assembly.GetTypes())
                    if (eachType.Name == AppCenterEditorHelper.APPCENTER_SETTINGS_TYPENAME)
                        appCenterSettingsType = eachType;
            //if (playFabSettingsType == typeof(object))
            //    Debug.LogWarning("Should not have gotten here: "  + allAssemblies.Length);
            //else
            //    Debug.Log("Found Settings: " + allAssemblies.Length + ", " + playFabSettingsType.Assembly.FullName);
            return appCenterSettingsType == typeof(object) ? null : appCenterSettingsType;
        }

        public static void DrawSdkPanel()
        {
            if (!isInitialized)
            {
                //SDK is installed.
                CheckSdkVersion();
                isInitialized = true;
                GetLatestSdkVersion();
                sdkFolder = FindSdkAsset();

                if (sdkFolder != null)
                {
                    AppCenterEditorPrefsSO.Instance.SdkPath = AssetDatabase.GetAssetPath(sdkFolder);
                    //PlayFabEditorDataService.SaveEnvDetails();
                }
            }

            //if (IsInstalled)
            //{
            //    ShowSdkInstalledMenu();
            //}

            //else
            //{
                ShowSdkNotInstalledMenu();
            //}
        }

        private static void CheckSdkVersion()
        {

        }

        private static void GetLatestSdkVersion()
        {

        }

        private static UnityEngine.Object FindSdkAsset() 
        {
            UnityEngine.Object sdkAsset = null;

            // look in editor prefs
            if (AppCenterEditorPrefsSO.Instance.SdkPath != null)
            {
                sdkAsset = AssetDatabase.LoadAssetAtPath(AppCenterEditorPrefsSO.Instance.SdkPath, typeof(UnityEngine.Object));
            }
            if (sdkAsset != null)
                return sdkAsset;

            sdkAsset = AssetDatabase.LoadAssetAtPath(AppCenterEditorHelper.DEFAULT_SDK_LOCATION, typeof(UnityEngine.Object));
            if (sdkAsset != null)
                return sdkAsset;

            var fileList = Directory.GetDirectories(Application.dataPath, "*AppCenterSdk", SearchOption.AllDirectories);
            if (fileList.Length == 0)
                return null;

            var relPath = fileList[0].Substring(fileList[0].LastIndexOf("Assets/"));
            return AssetDatabase.LoadAssetAtPath(relPath, typeof(UnityEngine.Object));
        }

        private static void ShowSdkInstalledMenu()
        {

        }

        public static void ImportLatestSDK()
        {

        }

        private static void ShowSdkNotInstalledMenu()
        {
            using (new UnityVertical(AppCenterEditorHelper.uiStyle.GetStyle("gpStyleGray1")))
            {
                var labelStyle = new GUIStyle(AppCenterEditorHelper.uiStyle.GetStyle("titleLabel"));

                EditorGUILayout.LabelField("No SDK is installed.", labelStyle, GUILayout.MinWidth(EditorGUIUtility.currentViewWidth));
                GUILayout.Space(20);

                using (new UnityHorizontal(AppCenterEditorHelper.uiStyle.GetStyle("gpStyleGray1")))
                {
                    var buttonWidth = 200;

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Refresh", AppCenterEditorHelper.uiStyle.GetStyle("Button"), GUILayout.MaxWidth(buttonWidth), GUILayout.MinHeight(32)))
                        appCenterSettingsType = null;
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Install App Center SDK", AppCenterEditorHelper.uiStyle.GetStyle("Button"), GUILayout.MaxWidth(buttonWidth), GUILayout.MinHeight(32)))
                        ImportLatestSDK();
                    GUILayout.FlexibleSpace();
                }
            }
        }
    }
}