using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AppCenterEditor 
{
    public class AppCenterEditor : EditorWindow 
    {
        public static string latestEdExVersion = string.Empty;
        internal static AppCenterEditor window;

        #region unity loops & methods
        void OnEnable()
        {
            if (window == null)
            {
                window = this;
                window.minSize = new Vector2(320, 0);
            }

            GetLatestEdExVersion();
        }

        void OnDisable()
        {
            AppCenterEditorPrefsSO.Instance.PanelIsShown = false;

        }

        void OnFocus()
        {
            OnEnable();
        }

        [MenuItem("Window/AppCenter/Editor Extensions")]
        static void AppCenterServices()
        {
            var editorAsm = typeof(UnityEditor.Editor).Assembly;
            var inspWndType = editorAsm.GetType("UnityEditor.SceneHierarchyWindow");

            if (inspWndType == null)
            {
                inspWndType = editorAsm.GetType("UnityEditor.InspectorWindow");
            }

            window = GetWindow<AppCenterEditor>(inspWndType);
            window.titleContent = new GUIContent("AppCenter EdEx");
            AppCenterEditorPrefsSO.Instance.PanelIsShown = true;
        }

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                if (AppCenterEditorPrefsSO.Instance.PanelIsShown || !AppCenterEditorSDKTools.IsInstalled)
                {
                     EditorCoroutine.Start(OpenPlayServices());
                }
            }
        }

        static IEnumerator OpenPlayServices()
        {
            yield return new WaitForSeconds(1f);
            if (!Application.isPlaying)
            {
                AppCenterServices();
            }
        }

        private void OnGUI()
        {
            HideRepaintErrors(OnGuiInternal);
        }

        private static void HideRepaintErrors(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (!e.Message.ToLower().Contains("repaint"))
                    throw;
                // Hide any repaint issues when recompiling
            }
        }

        private void OnGuiInternal()
        {
            GUI.skin = AppCenterEditorHelper.uiStyle;

            using (new UnityVertical())
            {
                //Run all updaters prior to drawing;
                //AppCenterEditorHeader.DrawHeader();

                //AppCenterEditorMenu.DrawMenu();
                AppCenterEditorSDKTools.DrawSdkPanel();

                using (new UnityVertical(AppCenterEditorHelper.uiStyle.GetStyle("gpStyleGray1"), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)))
                {
                    GUILayout.FlexibleSpace();
                }
            }

            Repaint();
        }

        #endregion

        private static void GetLatestEdExVersion()
        {
            var threshold = AppCenterEditorPrefsSO.Instance.EdSet_lastEdExVersionCheck != DateTime.MinValue ? AppCenterEditorPrefsSO.Instance.EdSet_lastEdExVersionCheck.AddHours(1) : DateTime.MinValue;

            if (DateTime.Today > threshold)
            {
                AppCenterEditorHttp.MakeGitHubApiCall("https://api.github.com/repos/Microsoft/AppCenter-SDK-Unity/git/refs/tags", (version) =>
                {
                    latestEdExVersion = version ?? "Unknown";
                    AppCenterEditorPrefsSO.Instance.EdSet_latestEdExVersion = latestEdExVersion;
                });
            }
            else
            {
                latestEdExVersion = AppCenterEditorPrefsSO.Instance.EdSet_latestEdExVersion;
            }
        }

        private static bool ShowEdExUpgrade()
        {
            if (string.IsNullOrEmpty(latestEdExVersion) || latestEdExVersion == "Unknown")
                return false;

            if (string.IsNullOrEmpty(AppCenterEditorHelper.EDEX_VERSION) || AppCenterEditorHelper.EDEX_VERSION == "Unknown")
                return true;

            string[] currrent = AppCenterEditorHelper.EDEX_VERSION.Split('.');
            if (currrent.Length != 3)
                return true;

            string[] latest = latestEdExVersion.Split('.');
            return latest.Length != 3
                || int.Parse(latest[0]) > int.Parse(currrent[0])
                || int.Parse(latest[1]) > int.Parse(currrent[1])
                || int.Parse(latest[2]) > int.Parse(currrent[2]);
        }

        private static void RemoveEdEx(bool prompt = true)
        {
            if (prompt && !EditorUtility.DisplayDialog("Confirm Editor Extensions Removal", "This action will remove App Center Editor Extensions from the current project.", "Confirm", "Cancel"))
                return;

            try
            {
                window.Close();
                var edExRoot = new DirectoryInfo(AppCenterEditorHelper.EDEX_ROOT);
                FileUtil.DeleteFileOrDirectory(edExRoot.Parent.FullName);
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }

        private static void UpgradeEdEx()
        {
            if (EditorUtility.DisplayDialog("Confirm EdEx Upgrade", "This action will remove the current App Center Editor Extensions and install the lastet version.", "Confirm", "Cancel"))
            {
                window.Close();
                ImportLatestEdEx();
            }
        }

        private static void ImportLatestEdEx()
        {

        }
    }
}